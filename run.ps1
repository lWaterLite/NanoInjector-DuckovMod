$SourceFile = "C:\work\Program_space\Rider\NanoInjector\NanoInjector\bin\Debug\netstandard2.1\"
$DestinationPath = "D:\SteamLibrary\steamapps\common\Escape from Duckov\Duckov_Data\Mods\ItemPickHint\"

Copy-Item -Path "$SourceFile\NanoInjector.dll" -Destination $DestinationPath -Force
Copy-Item -Path "$SourceFile\Languages\*" -Destination "$DestinationPath\Languages" -Recurse -Force

Write-Host "After-build Copy Complete."