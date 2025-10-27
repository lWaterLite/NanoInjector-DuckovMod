$SourceFile = "C:\work\Program_space\Rider\NanoInjector\NanoInjector\bin\Debug\netstandard2.1\NanoInjector.dll"
$DestinationPath = "D:\SteamLibrary\steamapps\common\Escape from Duckov\Duckov_Data\Mods\ItemPickHint\"

if (-not (Test-Path $SourceFile)) {
    Write-Error "源文件 '$SourceFile' 不存在！"
    exit 1
}

try {
    Copy-Item -Path $SourceFile -Destination $DestinationPath -Force
    Write-Host "Copy Complete."
} catch {
    Write-Error "文件复制失败: $_"
    exit 1
}