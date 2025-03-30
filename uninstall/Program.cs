using System;
using System.Diagnostics;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Management;
//using System.Management;

namespace uninstall
{
   
    public class StoppingScript
    {
        private string jsonFilePath;

        public StoppingScript()
        {
            Console.WriteLine("Stopping the script...");
            string markOfIdlePath = Environment.GetEnvironmentVariable("MARKOFIDLE");
            if (string.IsNullOrEmpty(markOfIdlePath)) throw new InvalidOperationException("MARKOFIDLE not set.");
            jsonFilePath = Path.Combine(markOfIdlePath, "scripts", "data.json");
        }

        public void StopScript()
        {
            if (!File.Exists(jsonFilePath)) throw new FileNotFoundException($"File not found: {jsonFilePath}");

            try
            {
                var json = File.ReadAllText(jsonFilePath);
                var data = JsonConvert.DeserializeObject<Data>(json);
                data.is_active = false;
                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));
                Console.WriteLine("Script stopped successfully.");
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
    }

    public class Data { 
        public bool is_active { get; set; }

        public Data() { }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.GetEnvironmentVariable("MARKOFIDLE");

            Console.WriteLine($"Path: {path}");
            Console.WriteLine("Are you sure you want to uninstall (Yes/No)");

            string userInput = Console.ReadLine();

            if (string.Equals(userInput, "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Starting to uninstall...");
                stopApp("mark_of_idle.exe");
                StoppingScript stoppingScript = new StoppingScript();
                stoppingScript.StopScript();
                DeleteShortcut("Mark of Idle");
                //DeleteDirectoryContents();

                //UninstallApp("Mark of Idle");
                Environment.SetEnvironmentVariable("MARKOFIDLE", null, EnvironmentVariableTarget.Machine);

                Console.WriteLine("Successfully uninstall the app you can now close this window");

            }
            else if (string.Equals(userInput, "no", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Uninstallation canceled.");
                System.Threading.Thread.Sleep(1000); // Pause for 1 second
                Environment.Exit(0); // Exit the application
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                System.Threading.Thread.Sleep(1000); // Pause for 1 second
                Environment.Exit(0); // Exit the application
            }
           

        }

        static void stopApp(string processName)
        {
            Console.WriteLine("Checking if the application is running...");

            var processes = Process.GetProcessesByName(processName.Replace(".exe", ""));

            if (processes.Length > 0)
            {
                Console.WriteLine("Application found. Stopping the application...");
                foreach (var process in processes) process.Kill();
                Console.WriteLine("Application stopped successfully.");
            }
            else
            {
                Console.WriteLine("Application is not running.");
            }
        }

        static void DeleteShortcut(string shortcutName)
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), shortcutName + ".lnk");

            if (File.Exists(shortcutPath))
            {
                try { File.Delete(shortcutPath); Console.WriteLine($"Shortcut '{shortcutName}.lnk' deleted."); }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
            else Console.WriteLine($"Shortcut '{shortcutName}.lnk' does not exist.");
        }

        public static void DeleteDirectoryContents()
        {
            string path = Environment.GetEnvironmentVariable("MARKOFIDLE");
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

            try
            {
                foreach (var file in Directory.GetFiles(path)) File.Delete(file);
                foreach (var dir in Directory.GetDirectories(path)) Directory.Delete(dir, true);
                Console.WriteLine("Directory contents deleted.");
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            

        }


        public static void UninstallApp(string appName)
        {
            var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Product WHERE Name = '{appName}'");
            foreach (ManagementObject m in searcher.Get())
            {
                try
                {
                    Console.WriteLine($"Uninstalling: {appName}");
                    m.InvokeMethod("Uninstall", null);
                    Console.WriteLine("Uninstallation completed.");
                }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
        }


    }
}
