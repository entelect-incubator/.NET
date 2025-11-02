param([int[]]$Phases = (3..12), [switch]$DryRun)

$results = @{Processed = 0; Updated = 0; Skipped = 0; Failed = 0}

Write-Host "`n$([char]0x2550 * 70)" -ForegroundColor Cyan
Write-Host "Adding 'sealed' keyword to Commands and Queries" -ForegroundColor Cyan
Write-Host "$([char]0x2550 * 70)`n" -ForegroundColor Cyan

if ($DryRun) { 
    Write-Host "[DRY RUN] No changes will be made`n" -ForegroundColor Yellow 
}

foreach ($phase in $Phases) {
    $path = "Phase $phase"
    
    if (-not (Test-Path $path)) {
        Write-Host "⊘ Phase $phase not found" -ForegroundColor Yellow
        continue
    }
    
    Write-Host "Processing Phase $phase..." -ForegroundColor Cyan
    
    $files = @()
    $files += Get-ChildItem -Path "$path\src" -Include "*Command*.cs" -Recurse -ErrorAction SilentlyContinue
    $files += Get-ChildItem -Path "$path\src" -Include "*Query*.cs" -Recurse -ErrorAction SilentlyContinue
    
    if ($files.Count -eq 0) { 
        Write-Host "  No Command/Query files found`n" -ForegroundColor Gray
        continue 
    }
    
    foreach ($file in $files) {
        $results.Processed++
        $content = Get-Content -Path $file.FullName -Raw
        
        if ($content -match 'public\s+(sealed\s+)?(class|record)\s+\w+(Command|Query)\s*[:(<]') {
            if ($content -match 'public\s+sealed\s+(class|record)') {
                Write-Host "  ✓ Already sealed: $($file.Name)" -ForegroundColor Green
                $results.Skipped++
            }
            else {
                $newContent = $content -replace '(public)\s+(class|record)\s+(\w+(Command|Query))', '$1 sealed $2 $3'
                
                if ($DryRun) {
                    Write-Host "  ◆ Would update: $($file.Name)" -ForegroundColor Yellow
                }
                else {
                    try {
                        Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
                        Write-Host "  ✓ Updated: $($file.Name)" -ForegroundColor Green
                    }
                    catch {
                        Write-Host "  ✗ Error: $($_.Exception.Message)" -ForegroundColor Red
                        $results.Failed++
                    }
                }
                $results.Updated++
            }
        }
        else {
            $results.Skipped++
        }
    }
    
    Write-Host ""
}

Write-Host "$([char]0x2550 * 70)" -ForegroundColor Cyan
Write-Host "Summary: Processed $($results.Processed) | Updated $($results.Updated) | Skipped $($results.Skipped) | Failed $($results.Failed)" -ForegroundColor Green

if ($DryRun) { 
    Write-Host "[DRY RUN] Run without -DryRun to apply changes" -ForegroundColor Cyan 
}

Write-Host "$([char]0x2550 * 70)`n" -ForegroundColor Cyan
