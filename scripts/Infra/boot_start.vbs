Set WshShell = CreateObject("WScript.Shell")

' Get the full path of the batch file using the %MARKOFIDLE% environment variable
batchFilePath = WshShell.ExpandEnvironmentStrings("%MARKOFIDLE%") & "Infra\run_venv.bat"

' Add quotes around the path to handle spaces in the file path
batchFilePath = """" & batchFilePath & """"

' Run the batch file using cmd.exe /c to ensure it runs correctly
WshShell.Run "cmd.exe /c " & batchFilePath, 0, False
