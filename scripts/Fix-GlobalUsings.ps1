#!/usr/bin/env pwsh
param([int[]]$Phases = @(5, 6, 7))

# Sorted alphabetically as StyleCop requires
$correctGlobalUsings = @'
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using Common.Entities;
global using Common.Models;
global using FluentValidation;
global using LiteBus.Commands.Abstractions;
global using LiteBus.Queries.Abstractions;
'@

foreach ($p in $Phases) {
    $guPath = "d:\Dev\Incubator\.NET\Phase $p\src\01. StartSolution\Common\GlobalUsings.cs"
    if (Test-Path $guPath) {
        Set-Content -Path $guPath -Value $correctGlobalUsings
        Write-Host "Fixed Phase $p GlobalUsings.cs" -ForegroundColor Green
    }
}

# Phase 8 might use Pezza.Common
$gu8Path = "d:\Dev\Incubator\.NET\Phase 8\src\01. StartSolution\Pezza.Common\GlobalUsings.cs"
if (Test-Path $gu8Path) {
    Set-Content -Path $gu8Path -Value $correctGlobalUsings
    Write-Host "Fixed Phase 8 GlobalUsings.cs" -ForegroundColor Green
}
