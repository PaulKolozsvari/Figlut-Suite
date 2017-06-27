When debugging the MMC snap-in directly from Visual Studio, make sure you set the Figlut.Configuration.Manager.About.x86
project's output directory (in the Linker) options to output the Figlut.Configuration.Manager.About.x86 to this project's output directory:

$(ProjectDir)\..\Figlut.Configuration.Manager.MMC\bin\Debug\$(ProjectName).dll

otherwise if you're building the Figlut.Configuration.Manager.Installer set the output directory to:

..\Debug\$(ProjectName).dll