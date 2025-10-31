param (
    [string]$mode
)

if ($mode -eq "debug") {
    $SourceFile = "C:\work\Program_space\Rider\NanoInjector\NanoInjector\bin\Debug\netstandard2.1\"
    $DestinationPath = "D:\SteamLibrary\steamapps\common\Escape from Duckov\Duckov_Data\Mods\NanoInjector\"
}
else {
    $SourceFile = "C:\work\Program_space\Rider\NanoInjector\NanoInjector\bin\Release\netstandard2.1\"
    $DestinationPath = "D:\SteamLibrary\steamapps\common\Escape from Duckov\Duckov_Data\Mods\NanoInjector\"
}



Copy-Item -Path "$SourceFile\NanoInjector.dll" -Destination $DestinationPath -Force
Copy-Item -Path "$SourceFile\Languages\*" -Destination "$DestinationPath\Languages" -Recurse -Force
Copy-Item -Path "$SourceFile\config.json" -Destination "$DestinationPath" -Force

Write-Host "After-build Copy Complete."