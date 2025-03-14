using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mark_of_idle
{

    class Logs
    {
        private string folder_path;
        private string latest_file_path;
        public Logs(string project_path)
        {
            this.folder_path = Path.Combine(project_path, "logs");
            this.latest_file_path = this.LoadLatestLogFile();
        }

        private string LoadLatestLogFile()
        {
            var logFiles = Directory.GetFiles(this.folder_path, "logfile.log.*")
                                    .OrderByDescending(f => File.GetLastWriteTime(f))  // Sort by last modified time
                                    .ToList();
                                    //.OrderByDescending(f => GetDateFromFilename(f))
                                    //.ToList();

            if (logFiles.Any())
            {
                return logFiles.First();
            }
            else
            {
                throw new FileNotFoundException("No log files found.");
            }
        }


        public string contents()
        {

            this.latest_file_path = this.LoadLatestLogFile();
            using (FileStream fs = new FileStream(this.latest_file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                string content = sr.ReadToEnd(); // Read the entire content of the file
                return content; // Return the file content as a string
            }


        }

        public List<string> ExcludeDifferences(string text1)
        {

            string newtext = this.contents();

            // Split both texts into individual lines
            var lines1 = text1.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim()) // Strip whitespace from each line
                .ToList();
            var lines2 = newtext.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim()) // Strip whitespace from each line
                .ToList();

            if (lines1.Count <= 0)
            {
                return lines2;
            }

            string lastLineInText1 = lines1.Last();
            int lastLineIndexInText2 = lines2.IndexOf(lastLineInText1);

            if (lastLineIndexInText2 >= 0)
            {
                // Get all logs after the last line found in text2
                return lines2.Skip(lastLineIndexInText2 + 1).ToList();
            }
            else
            {
                // If not found, return all the logs in text2
                return lines2;
            }
        }


    }
 

    class Settings
    {
        private string data_path;
        public Data result;

        public class Data
        {
            public int threshold { get; set; }
            public bool is_active { get; set; }

            public bool start_on_boot { get; set; }

            public Data copy()
            {
                return (Data)this.MemberwiseClone();
            }
        }

        public Settings(string project_path)
        {
            this.data_path = Path.Combine(project_path, "data.json");

            if (!File.Exists(this.data_path))
            {
                throw new InvalidOperationException("The file does not exist.");
            }

            string fileContent = File.ReadAllText(this.data_path);

            this.result = JsonConvert.DeserializeObject<Data>(fileContent);

        }

        public void set(Data value)
        {
            string jsonString = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(this.data_path, jsonString);
        }



    }
 
    class Script
    {
        public string project_path;
        public string venvActivationScript;
        public string mainScript;
        public string batFilePath;
        public string boot_start_path;
        private const string taskName = "mark_of_idle_start_on_boot";
        private const string registryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public Settings settings;
        public Logs logs;
        public Script()
        {
            this.project_path = Environment.GetEnvironmentVariable("MARKOFIDLE");

            if (string.IsNullOrEmpty(this.project_path))
            {
                throw new InvalidOperationException("Environment variable 'MARKOFIDLE' is not set.");
            }


            this.venvActivationScript = Path.Combine(this.project_path, "venv", "Scripts", "activate.bat");


            if (!File.Exists(this.venvActivationScript))
            {
                throw new InvalidOperationException($"Activation script not found at: {this.project_path}");
            }

            this.mainScript = Path.Combine(this.project_path, "main.py");
            this.batFilePath = Path.Combine(this.project_path, "Infra", "run_venv.bat");
            this.boot_start_path = Path.Combine(this.project_path, "Infra", "boot_start.vbs");
            this.settings = new Settings(this.project_path);
            this.logs = new Logs(this.project_path);

            this.SetBootAtStartUp();

            //bool is_task_exists = this.DoesTaskExist();

            //Debug.WriteLine(is_task_exists);



        }

        public void activate()
        {

            string command = $"/C cd \"{this.project_path}\" && .\\Infra\\run_venv.bat";
            this.doProccess(command);

        }

        private void doProccess(string command, string filename = "cmd.exe")
        {

            Debug.WriteLine("Do proccess =>> ");
            Debug.WriteLine(command);

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = filename,
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false // Don't open the command prompt window
            };

            using (Process process = Process.Start(startInfo))
            {
                // Capture and display the output and errors
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Display the output (if any)
                Debug.WriteLine("Output:");
                Debug.WriteLine(output);

                // Display errors (if any)
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.WriteLine("Error:");
                    Debug.WriteLine(error);
                }
            }

        }

        private void SetBootAtStartUp()
        {

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(Script.registryKey, true))
            {
                if (key != null)
                {
                    string command = $"wscript.exe \"{this.boot_start_path}\"";
                    key.SetValue(Script.taskName, command);
                    Debug.WriteLine($"VBS script added to registry for all users: {this.boot_start_path}");
                }
                else
                {
                    Debug.WriteLine("Failed to open registry key.");
                }
            }


           // string taskName = "mark_of_idle_start_on_boot";
           // string taskAction = $"/c start '' '{this.boot_start_path}'";  // Correctly quoting the path

           // // Construct the correct schtasks command
           // string taskCommand = $"schtasks /create /tn \"{this.taskName}\" /tr \"cmd.exe {taskAction}\" /sc onlogon /f /ru SYSTEM";


           // Debug.WriteLine(taskCommand);
           //this.doProccess(taskCommand);

        }

        private void RemoveBootAtStartUp(string taskName)
        {

            //string taskCommand = $"schtasks /delete /tn '{this.taskName}' /f";
            //this.doProccess(taskCommand);

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(Script.registryKey, true))
            {
                if (key != null)
                {
                    // Remove the registry entry by its taskName
                    key.DeleteValue(Script.taskName, false); // The 'false' means do not throw an exception if the value doesn't exist.

                    Debug.WriteLine($"VBS script removed from registry: {this.boot_start_path}");
                }
                else
                {
                    Debug.WriteLine("Failed to open registry key.");
                }
            }
        }

        private bool DoesTaskExist()
        {
            // Run schtasks /query to list all scheduled tasks
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "schtasks",
                Arguments = $"/query",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();



                // If the task exists, schtasks /query will output information about it
                if (!string.IsNullOrEmpty(output) && !output.Contains("ERROR"))
                {
                    return true; // Task exists
                }
                return false; // Task does not exist
            }

        }






        }
}
