Write-Host "Day`tBld[ms]`tRun[ms]"

$totalBuild = 0
$totalRun = 0

1..25 | 
    ForEach-Object { ([string]$_).PadLeft(2, '0') } | Where-Object { Test-Path $_ -PathType Container } | 
    ForEach-Object { 
        Set-Location $_
        
        & dotnet clean > $null
        
        $build = (Measure-Command { & dotnet build --configuration Release > $null }).TotalMilliseconds
        $run = (Measure-Command { & ".\bin\Release\net6.0\$($_).exe" > $null }).TotalMilliseconds
        
        $totalBuild = $totalBuild + $build
        $totalRun = $totalRun + $run

        Write-Host "$($_)`t$('{0:f}' -f $build)`t$('{0:f}' -f $run)"
        Set-Location .. 
    }

Write-Host "Total`t$('{0:f}' -f $totalBuild)`t$('{0:f}' -f $totalRun)"