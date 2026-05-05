param()
$root = "c:\Dev\Incubator\.NET"
$mapperFiles = Get-ChildItem $root -Recurse -Filter "Mapper.cs" -ErrorAction SilentlyContinue |
    Where-Object { (Get-Content $_.FullName -Raw) -match '\[Mapper\]' }

Write-Host "Found $($mapperFiles.Count) Mapper.cs files with [Mapper] attribute"

foreach ($mf in $mapperFiles) {
    $commonDir  = $mf.Directory.Parent.FullName
    $csprojPath = Join-Path $commonDir "Common.csproj"
    $globalPath = Join-Path $commonDir "GlobalUsings.cs"

    if (-not (Test-Path $csprojPath)) { continue }

    # ---- Fix csproj ----
    $csproj = [System.IO.File]::ReadAllText($csprojPath)
    if ($csproj -notmatch 'Riok\.Mapperly') {
        if ($csproj -match '<ItemGroup>') {
            # Insert after first <ItemGroup>
            $newCsproj = $csproj -replace '(<ItemGroup>\s*\r?\n)', "`$1        <PackageReference Include=`"Riok.Mapperly`" Version=`"3.6.0`" />`n"
        } else {
            # No ItemGroup at all — add one before </Project>
            $newCsproj = $csproj -replace '</Project>', "    <ItemGroup>`n        <PackageReference Include=`"Riok.Mapperly`" Version=`"3.6.0`" />`n    </ItemGroup>`n</Project>"
        }
        [System.IO.File]::WriteAllText($csprojPath, $newCsproj)
        Write-Host "  [csproj]       $($csprojPath.Replace($root+'\', ''))"
    }

    # ---- Fix GlobalUsings.cs ----
    if (Test-Path $globalPath) {
        $gu = [System.IO.File]::ReadAllText($globalPath)
        if ($gu -notmatch 'Riok\.Mapperly') {
            $newGu = $gu.TrimEnd() + "`r`nglobal using Riok.Mapperly.Abstractions;"
            [System.IO.File]::WriteAllText($globalPath, $newGu)
            Write-Host "  [GlobalUsings] $($globalPath.Replace($root+'\', ''))"
        }
    }
}

Write-Host "`nDone."
