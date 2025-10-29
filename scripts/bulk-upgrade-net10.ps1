param()

Set-StrictMode -Version Latest

Write-Host "Starting bulk upgrade to .NET 10 on branch upgrade/all-net10"

# Create branch
git checkout -b upgrade/all-net10

# Replace TargetFramework in .csproj files (skip files containing TargetFrameworks or netstandard)
$csprojFiles = Get-ChildItem -Path . -Recurse -Filter *.csproj | Where-Object { -not (Get-Content $_.FullName -Raw) -match '<TargetFrameworks>' }
$updated = @()
foreach ($f in $csprojFiles) {
    $text = Get-Content $f.FullName -Raw
    if ($text -match '<TargetFramework>net(?!standard)[0-9\.]+</TargetFramework>') {
        $new = [regex]::Replace($text, '<TargetFramework>net(?!standard)[0-9\.]+</TargetFramework>', '<TargetFramework>net10.0</TargetFramework>')
        Set-Content -Path $f.FullName -Value $new
        Write-Host "Updated TargetFramework in $($f.FullName)"
        $updated += $f.FullName
    }
}

if ($updated.Count -gt 0) {
    git add $updated
    git commit -m "chore: bulk upgrade TargetFramework -> net10.0"
} else {
    Write-Host "No TargetFramework updates found"
}

# Restore all solutions found
$slnFiles = Get-ChildItem -Path . -Recurse -Filter *.sln
foreach ($s in $slnFiles) {
    Write-Host "Restoring solution $($s.FullName)"
    dotnet restore $s.FullName
}

# For each project, run dotnet list package --outdated and attempt to update to latest
$projFiles = Get-ChildItem -Path . -Recurse -Filter *.csproj
foreach ($p in $projFiles) {
    Write-Host "Checking packages for $($p.FullName)"
    $out = dotnet list $p.FullName package --outdated 2>&1
    # Extract package names from table lines (skip headers)
    foreach ($line in $out) {
        # A package line typically starts with whitespace then PackageId
        if ($line -match '^[ \t]*([A-Za-z0-9_.-]+)\s+\S+\s+\S+\s+\S+') {
            $pkg = $Matches[1]
            Write-Host "Updating package $pkg in $($p.FullName)"
            dotnet add $p.FullName package $pkg || Write-Host "Failed to update $pkg in $($p.FullName)"
        }
    }
}

# Restore and build solutions again
foreach ($s in $slnFiles) {
    Write-Host "Restoring and building solution $($s.FullName)"
    dotnet restore $s.FullName
    dotnet build $s.FullName -v minimal
}

# Commit any changed packages
git add -A
git commit -m "chore: bulk update NuGet packages to latest" || Write-Host "No package updates to commit"

Write-Host "Bulk upgrade script finished. Check terminal output for details."
