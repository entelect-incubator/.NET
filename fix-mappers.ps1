Set-Location "c:\Dev\Incubator\.NET"
$root = "c:\Dev\Incubator\.NET"
$srcCustomerMapper = Get-Content "$root\Phase 4\src\02. Step1\Common\Mappers\CustomerMapper.cs" -Raw
$srcPizzaMapper = Get-Content "$root\Phase 4\src\02. Step1\Common\Mappers\PizzaMapper.cs" -Raw

# Delete Mapper.cs (Todo mapper) from ALL contaminated phases
$todoMapperPaths = @(
  "Phase 1\src\02. EndSolution",
  "Phase 2\src\01. StartSolution","Phase 2\src\02. EndSolution",
  "Phase 3\src\01. StartSolution","Phase 3\src\02. Step1","Phase 3\src\03. EndSolution",
  "Phase 4\src\01. StartSolution",
  "Phase 5\src\01. StartSolution","Phase 5\src\02. Step 1",
  "Phase 6\src\01. StartSolution","Phase 6\src\02. Step 1","Phase 6\src\03. Step 2",
  "Phase 7\src\01. StartSolution","Phase 7\src\02. Step 1","Phase 7\src\03. Step 2"
)
foreach ($p in $todoMapperPaths) {
  $f = "$root\$p\Common\Mappers\Mapper.cs"
  if (Test-Path $f) { Remove-Item $f -Force; Write-Host "DELETED Mapper.cs: $p" }
}

# Add PizzaMapper.cs to phases that are missing it
$needPizzaMapper = @(
  "Phase 1\src\02. EndSolution",
  "Phase 2\src\01. StartSolution",
  "Phase 5\src\01. StartSolution","Phase 5\src\02. Step 1",
  "Phase 6\src\03. Step 2"
)
foreach ($p in $needPizzaMapper) {
  $f = "$root\$p\Common\Mappers\PizzaMapper.cs"
  if (-not (Test-Path $f)) { Set-Content $f $srcPizzaMapper -Encoding UTF8; Write-Host "ADDED PizzaMapper.cs: $p" }
}

# Add CustomerMapper.cs to phases that are missing it
$needCustomerMapper = @(
  "Phase 5\src\01. StartSolution","Phase 5\src\02. Step 1",
  "Phase 6\src\03. Step 2",
  "Phase 7\src\01. StartSolution"
)
foreach ($p in $needCustomerMapper) {
  $f = "$root\$p\Common\Mappers\CustomerMapper.cs"
  if (-not (Test-Path $f)) { Set-Content $f $srcCustomerMapper -Encoding UTF8; Write-Host "ADDED CustomerMapper.cs: $p" }
}

Write-Host "Mapper fixes complete."
