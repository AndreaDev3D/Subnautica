@echo off
REM Set variables based on arguments passed to the script
set projectName=%1
set buildPath=%2
set projectDir=%3
set targetDir=%4
set subnauticaFolder=D:\Steam\steamapps\common\SubnauticaZero\BepInEx\plugins\%projectName%\
set zipDestination=%3..\Download\%projectName%.zip

REM Debugging: Echo paths to verify
echo projectName=%projectName%
echo buildPath=%buildPath%
echo projectDir=%projectDir%
echo targetDir=%targetDir%
echo subnauticaFolder=%subnauticaFolder%

REM Create directories if they do not exist
if not exist "%subnauticaFolder%\Assets" (
    mkdir "%subnauticaFolder%\Assets"
)

REM Copy build files
xcopy "%buildPath%" "%subnauticaFolder%" /y
xcopy "%projectDir%*.json" "%subnauticaFolder%" /y
xcopy "%projectDir%Assets\*.asset" "%subnauticaFolder%\Assets\" /y
xcopy "%projectDir%Assets\*.manifest" "%subnauticaFolder%\Assets\" /y
xcopy "%projectDir%Assets\*.png" "%subnauticaFolder%\Assets\" /y

REM Copy AD3D_Common.dll file
xcopy "%targetDir%\AD3D_Common.dll" "%subnauticaFolder%" /y
xcopy "%targetDir%\Assets\*.asset" "%subnauticaFolder%\Assets\" /y
xcopy "%targetDir%\Assets\*.manifest" "%subnauticaFolder%\Assets\" /y
xcopy "%targetDir%\Assets\*.png" "%subnauticaFolder%\Assets\" /y

REM Zip the mod folder
7z a -tzip "%zipDestination%" "%subnauticaFolder%" -mx9

REM Notify completion
echo Mod build and deployment complete.
pause