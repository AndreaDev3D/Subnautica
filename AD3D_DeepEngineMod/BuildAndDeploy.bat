@echo off
REM Set variables
set projectName=$(ProjectName)
set buildPath=$(TargetPath)
set projectDir=$(ProjectDir)
set targetDir=$(TargetDir)
set subnauticaFolder=D:\Steam\steamapps\common\SubnauticaZero\QMods\%projectName%\
set commonModFolder=E:\Mods\Subnautica\AD3D_Common
set zipDestination=$(SolutionDir)Download\%projectName%.zip

REM Create directories if they do not exist
if not exist "%subnauticaFolder%\Assets" (
    mkdir "%subnauticaFolder%\Assets"
)

REM Copy build files
xcopy "%buildPath%" "%subnauticaFolder%" /y
xcopy "%projectDir%mod.json" "%subnauticaFolder%" /y
xcopy "%projectDir%Assets\*.asset" "%subnauticaFolder%\Assets\" /y
xcopy "%projectDir%Assets\*.manifest" "%subnauticaFolder%\Assets\" /y
xcopy "%projectDir%Assets\*.png" "%subnauticaFolder%\Assets\" /y

REM Copy common mod files
xcopy "%targetDir%AD3D_Common.dll" "%subnauticaFolder%" /y
xcopy "%commonModFolder%\Assets\*.asset" "%subnauticaFolder%\Assets\" /y
xcopy "%commonModFolder%\Assets\*.manifest" "%subnauticaFolder%\Assets\" /y

REM Zip the mod folder
if exist "%zipDestination%" (
    del "%zipDestination%"
)

"C:\Program Files\7-Zip\7z.exe" a "%zipDestination%" "%subnauticaFolder%"

REM Notify completion
echo Mod build and deployment complete.
pause
