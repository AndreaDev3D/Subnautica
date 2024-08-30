//https://docs.github.com/en/get-started/importing-your-projects-to-github/importing-source-code-to-github/adding-locally-hosted-code-to-github
# As Developer Go to your mods folder:
 
- First create a new repository on GitHub.com // Mine is 
- execute cmd "git init -b main" // Initialize the local directory as a Git repository.
- execute cmd "git add ." // Add the files in your new local repository. This stages them for the first commit
- execute cmd "git add *.dll -f" // Add the dll in your new local repository. This stages them for the first commit
- execute cmd "git commit -m "First commit"" // Commit the files that you've staged in your local repository

- At the top of your repository on GitHub.com's Quick Setup page, click  to copy the remote repository URL.
- execute cmd "git remote add origin <REMOTE_URL>" // eg "git remote add origin https://github.com/AndreaDev3D/AD3D_LightSolutionMod.git"
- execute cmd "git push -u origin main" // Push the changes in your local repository to GitHub.com.
- Done! Your Mod is ready to be distributed.


# As a User create a git repo in QMod folder(you don't need to commit):

- Navigate into the folder and
- execute cmd "git init -b main"
- Done! Your Project is ready.

To Add a ModRepo:
- execute cmd "git submodule add <REMOTE_URL>" // eg "git submodule add https://github.com/AndreaDev3D/AD3D_LightSolutionMod.git"





# PostBuild Generic cmd

REM Copy Builded Mod into Subnautica folder
xcopy "$(TargetPath)" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\" /y
xcopy "$(ProjectDir)mod.json" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\" /y
xcopy "$(ProjectDir)Assets\*.asset" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\" /y
xcopy "$(ProjectDir)Assets\*.manifest" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\" /y
xcopy "$(ProjectDir)Assets\*.png" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\" /y

REM  Copy Common Mod into Subnautica folder
xcopy "$(TargetDir)AD3D_Common.dll" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\" /y
xcopy "E:\Mods\Subnautica\AD3D_Common\Assets\*.asset" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\" /y
xcopy "E:\Mods\Subnautica\AD3D_Common\Assets\*.manifest" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\" /y

REM  Zip Mod into Git folder
7z a "$(SolutionDir)Download\$(ProjectName).zip" "D:\Steam\steamapps\common\Subnautica\QMods\$(ProjectName)"