param()
$root = "c:\Dev\Incubator\.NET"
$pkgLine = '        <PackageReference Include="Riok.Mapperly" Version="3.6.0" />'
$files = Get-ChildItem $root -Recurse -Filter "Common.csproj" |
    Where-Object { (Get-Content $_.FullName -Raw) -match 'Riok\.Mapperly' }
Write-Host "Found $($files.Count) files with Riok.Mapperly"
foreach ($f in $files) {
    $c = [System.IO.File]::ReadAllText($f.FullName)
    $n = ([regex]::Matches($c, 'Riok\.Mapperly')).Count
    if ($n -le 1) { continue }
    Write-Host "Dedup $n -> 1 : $($f.FullName.Replace($root+'\',''))"
    $lines = ($c -split "`r?`n") | Where-Object { $_ -notmatch 'Riok\.Mapperly' }
    $inserted = $false
    $out = [System.Collections.Generic.List[string]]::new()
    foreach ($l in $lines) {
        if (-not $inserted -and $l -match '^\s*<PackageReference') {
            $out.Add($pkgLine); $inserted = $true
        }
        $out.Add($l)
    }
    if (-not $inserted) {
        $res = [System.Collections.Generic.List[string]]::new()
        foreach ($l in $out) {
            if ($l -match '^\s*</Project>') {
                $res.Add('    <ItemGroup>'); $res.Add($pkgLine); $res.Add('    </ItemGroup>')
            }
            $res.Add($l)
        }
        $out = $res
    }
    [System.IO.File]::WriteAllText($f.FullName, ($out -join "`n"))
    Write-Host "  done: $(([regex]::Matches([System.IO.File]::ReadAllText($f.FullName),'Riok\.Mapperly')).Count) occurrence(s)"
}
Write-Host "Complete."
