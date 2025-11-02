#!/usr/bin/env pwsh

# Fix StyleCop SA1001 whitespace before commas
$phases = @(5, 6, 7)

foreach ($p in $phases) {
    $testFiles = Get-ChildItem "d:\Dev\Incubator\.NET\Phase $p\src\01. StartSolution\Test\Core" -Filter "Test*.cs" -Recurse
    
    foreach ($file in $testFiles) {
        $content = Get-Content $file.FullName
        $fixed = $content -replace '\s+,', ','
        if ($fixed -ne $content) {
            Set-Content $file.FullName $fixed
            Write-Host "Fixed: Phase $p - $($file.Name)" -ForegroundColor Green
        }
    }
}
