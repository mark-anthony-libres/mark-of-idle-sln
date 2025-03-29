Set WshShell = CreateObject("WScript.Shell")

' Get the value of the MARKOFIDLE environment variable
On Error Resume Next
MARKOFIDLE =  WshShell.ExpandEnvironmentStrings("%MARKOFIDLE%")
On Error GoTo 0

' Check if MARKOFIDLE is empty
If MARKOFIDLE = "" Then
    WScript.Echo "ERROR: MARKOFIDLE environment variable is not set."
    WScript.Quit 1
End If

' Echo the value of MARKOFIDLE
WScript.Echo "MARKOFIDLE: " & MARKOFIDLE

' Ensure MARKOFIDLE is pointing to the correct project directory and ends with a backslash
If Right(MARKOFIDLE, 1) <> "\" Then
    MARKOFIDLE = MARKOFIDLE & "\"
End If

' Change directory to %MARKOFIDLE%\scripts
Set objShell = CreateObject("WScript.Shell")
objShell.CurrentDirectory = MARKOFIDLE & "scripts"

' Activate the virtual environment
Call RunCommand(MARKOFIDLE & "venv\Scripts\activate.bat")

' Run the Python script
Call RunCommand("python main.py")

' Optionally deactivate the environment after execution
Call RunCommand("deactivate")

' Function to run a command in the shell
Sub RunCommand(command)
    Set objShell = CreateObject("WScript.Shell")
    objShell.Run command, 1, True
End Sub
