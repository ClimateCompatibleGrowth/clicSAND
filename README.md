# ModelRunner

Software to automate extraction of input data from spreadsheet, execute glpsol, execute cbc and produce results

Licenses
This software is licensed under the Create Commons Zero v1.0 Universal license, details in licences/license.txt
CBC is licensed under the Eclipse Public License - v 2.0, details in licences/CBC_LICENSE.txt
glpk is licensed under the GNU Public License v3.0, details in licences/glpk_license.txt

Prerequisites:
.Net Runtime (the application was compiled against 4.7.2) https://dotnet.microsoft.com/download/dotnet-framework/net472
Microsoft Office - I used Office 365
Nullsoft Scriptable Install System (NSIS) - https://nsis.sourceforge.io/Download

To create a new ModelRunner install package:
Execute createinstaller.bat from the Installer folder

To run data and a model:
Launch ModelRunner.exe
Click the ... button to the right of Data Source to select the xlsm file
Click the ... button to the right of Model to select the model file
Click Run
Wait!

