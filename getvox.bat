@echo off
copy "C:\Program Files\MagicaVoxel\vox\zepto\*.vox" .\Art\Vox\
del .\Assets\Resources\Vox\*.mtl
copy "C:\Program Files\MagicaVoxel\export\*.mtl" .\Assets\Resources\Vox\
del .\Assets\Resources\Vox\Materials\*.mat
copy "C:\Program Files\MagicaVoxel\export\Materials\*.mat" .\Assets\Resources\Vox\Materials\
del .\Assets\Resources\Vox\*.png
copy "C:\Program Files\MagicaVoxel\export\*.png" .\Assets\Resources\Vox\
del .\Assets\Resources\Vox\*.obj
copy "C:\Program Files\MagicaVoxel\export\*.obj" .\Assets\Resources\Vox\
timeout 2 > NUL
echo %time%