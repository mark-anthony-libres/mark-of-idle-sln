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


    class BootAtStartUp
    {
        public string boot_start_path;
        private const string taskName = "mark_of_idle_start_on_boot";
        private const string registryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public BootAtStartUp(string scriptsFolder)
        {
            this.boot_start_path = Path.Combine(scriptsFolder, "Infra", "boot_start.vbs");
        }

        public void SetBootAtStartUp()
        {
            if (this.IsBootStartUp) return;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(BootAtStartUp.registryKey, true))
            {
                if (key != null)
                {
                    string command = $"wscript.exe \"{this.boot_start_path}\"";
                    key.SetValue(BootAtStartUp.taskName, command);
                    Debug.WriteLine($"VBS script added to registry for all users: {this.boot_start_path}");
                }
                else
                {
                    Debug.WriteLine("Failed to open registry key.");
                }
            }


        }

        public void RemoveBootAtStartUp()
        {
            if (!this.IsBootStartUp) return;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(BootAtStartUp.registryKey, true))
            {
                if (key != null)
                {
                    // Remove the registry entry by its taskName
                    key.DeleteValue(BootAtStartUp.taskName, false); // The 'false' means do not throw an exception if the value doesn't exist.

                    Debug.WriteLine($"VBS script removed from registry: {this.boot_start_path}");
                }
                else
                {
                    Debug.WriteLine("Failed to open registry key.");
                }
            }
        }

        public bool IsBootStartUp
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(BootAtStartUp.registryKey, true))
                {
                    return key.GetValue(BootAtStartUp.taskName) != null;
                }
            }
        }


    }

    class Logs
    {
        private string folder_path;
        private string latest_file_path;
        public Logs(string scriptsFolder)
        {
            this.folder_path = Path.Combine(scriptsFolder, "logs");
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
                return null;
            }
        }


        public string contents()
        {

            this.latest_file_path = this.LoadLatestLogFile();

            if(this.latest_file_path == null)
            {
                return "";
            }

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

        public Settings(string script_folder)
        {
            this.data_path = Path.Combine(script_folder, "data.json");

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
        public string scriptsFolder;
        public string mainScript;
        public string batFilePath;
        public Settings settings;
        public Logs logs;
        public BootAtStartUp boot_start_up;

        public Script()
        {
            this.project_path = Environment.GetEnvironmentVariable("MARKOFIDLE");
            this.scriptsFolder = Path.Combine(this.project_path, "scripts");

            if (string.IsNullOrEmpty(this.project_path))
            {
                throw new InvalidOperationException("Environment variable 'MARKOFIDLE' is not set.");
            }


            this.venvActivationScript = Path.Combine(this.scriptsFolder,"venv", "Scripts", "activate.bat");


            if (!File.Exists(this.venvActivationScript))
            {
                throw new InvalidOperationException($"Activation script not found at: {this.project_path}");
            }

            this.mainScript = Path.Combine(this.scriptsFolder, "main.py");
            this.batFilePath = Path.Combine(this.scriptsFolder, "Infra", "run_venv.bat");

            this.settings = new Settings(this.scriptsFolder);
            this.logs = new Logs(this.scriptsFolder);
            this.boot_start_up = new BootAtStartUp(this.scriptsFolder);

        }

        public void activate()
        {
           
            string command = $"/C cd \"{this.project_path}\" && .\\scripts\\Infra\\run_venv.bat";
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
                CreateNoWindow = true // Don't open the command prompt window
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

    }
}
