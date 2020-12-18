# ModelRunner

Software to automate extraction of input data from spreadsheet, execute glpsol, execute cbc and produce results

Prerequisites:
.Net Runtime (the application was compiled against 4.7.2) https://dotnet.microsoft.com/download/dotnet-framework/net472
Microsoft Office - I used Office 365
Python installed (I tested with https://www.python.org/ftp/python/3.7.9/python-3.7.9-amd64.exe)
cbc installed (I tested with https://projects.coin-or.org/CoinBinary/export/820/binary/Cbc/Cbc-2.7.5-win64-intel11.1.zip)

Configuration required
ModelRunner.exe.config
cbcLocation - change to the path to the bin folder of cbc on your machine
pythonLocation - change to where Python was installed
pythonScript - change  to the location of CBC_results_ModelRunner.py (in PythonScripts subfolder of ModelRunner)

run pip -r <path to Pythonscripts\requirements.txt> to install required python libraries

Before you can run an Excel spreadsheet and model, you need to inject the contents of Module1.vba in the Macro folder into the Excel spreadsheet
Open the spreadsheet
Click Developer
Click View Code
Click Insert / Module
Paste the contents of Module1.vba at the cursor (it should be in the source code content area)
Save the spreadsheet as Excel Macro-enabled Workbook (*.xlsm)

To run data and a model:
Launch ModelRunner.exe
Click the ... button to the right of Data Source to select the xlsm file
Click the ... button to the right of Model to select the model file
Update the Year field as required
Click Run
Wait!

