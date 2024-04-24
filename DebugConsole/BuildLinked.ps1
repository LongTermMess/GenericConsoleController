# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/p5rDebugConsole/*" -Force -Recurse
dotnet publish "./p5rDebugConsole.csproj" -c Release -o "$env:RELOADEDIIMODS/p5rDebugConsole" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location