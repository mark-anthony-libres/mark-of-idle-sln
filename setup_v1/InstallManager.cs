using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace setup_v1
{
    public class InstallManager
    {
        private readonly RichTextBox _logBox;
        private readonly ProgressBar _progressBar;
        public int _progressValue = 0;
        private string _markOfIdleFolder;
        private const string _executableName = "mark_of_idle.exe";
        public string[] progress_lines = { };

        public string MarkOfIdleFolder => _markOfIdleFolder;
        public string ExecutableName => _executableName;

        public InstallManager(RichTextBox logBox, ProgressBar progressBar)
        {
            _logBox = logBox;
            _progressBar = progressBar;
        }

        public void PreInstallation()
        {
            string programFilesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            _markOfIdleFolder = Path.Combine(programFilesDirectory, "Mark of Idle");

            if (!Directory.Exists(_markOfIdleFolder))
            {

                try
                {
                    Directory.CreateDirectory(_markOfIdleFolder);
                    AppendTextToLog($"\nFolder created: {_markOfIdleFolder}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
            else
            {
                AppendTextToLog($"Folder already exists: {_markOfIdleFolder}");

                foreach (var subdir in Directory.GetDirectories(_markOfIdleFolder))
                    Directory.Delete(subdir, true);

                foreach (var file in Directory.GetFiles(_markOfIdleFolder))
                    File.Delete(file);

                Console.WriteLine("All items have been removed from the folder.");

            }
        }

        public async Task ExtractResourcesAsync()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            var extractor = new ResourceExtractor();

            foreach (var resourceName in resourceNames)
            {
                await extractor.CopyResourceAsync(assembly, resourceName, _markOfIdleFolder, _logBox);
            }
        }

        public async Task<int> RunBatFileAsync(string batFilePath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c cd {_markOfIdleFolder} && .\\setup.bat",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                Process process = new Process { StartInfo = processStartInfo };
                process.OutputDataReceived += (sender, e) => AppendTextToLog(e.Data);
                process.ErrorDataReceived += (sender, e) => AppendTextToLog(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await Task.Run(() => process.WaitForExit());

                return process.ExitCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        public string [] ProgressMaxValue1()
        {
            Debug.WriteLine("Calculating Progress Max Value");

            string [] __line = { };
            string batFilePath = Path.Combine(_markOfIdleFolder, "setup.bat");
            string[] lines = File.ReadAllLines(batFilePath);

            foreach (var line in lines)
            {
                if (line.ToLower().Replace("echo", "").TrimStart().StartsWith("====="))
                {
                    __line = __line.Append(line).ToArray();
                }
            }

            return __line;
        }

        public int ProgressMaxValue()
        {
            Debug.WriteLine("Calculating Progress Max Value");

            int count = 0;
            string batFilePath = Path.Combine(_markOfIdleFolder, "setup.bat");
            string[] lines = File.ReadAllLines(batFilePath);

            foreach (var line in lines)
            {
                if (line.ToLower().Replace("echo", "").TrimStart().StartsWith("====="))
                {
                    Debug.WriteLine(line);
                    count++;
                }
            }

            return count;
        }

        private void UpdateProgressBar(int value)
        {
            // Check if Invoke is required (if we're not on the UI thread)
            if (this._progressBar.InvokeRequired)
            {
                // Use Invoke to update the progress bar on the UI thread
                this._progressBar.Invoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                // Update progress on the UI thread
                this._progressBar.Value = value;
            }
        }

        private void AppendTextToLog(string text)
        {

            if (string.IsNullOrEmpty(text)) return;

            if (text.ToLower().Replace("echo", "").TrimStart().StartsWith("====="))
            {
                Debug.WriteLine("a ==>  " + text);
                this.progress_lines = this.progress_lines.Append(text).ToArray();
                this._progressValue++;
                this.UpdateProgressBar(this._progressValue);
            }

            this._appendTextToLog(text);
        }

        private void _appendTextToLog(string text)
        {


            if (string.IsNullOrEmpty(text)) return;


            if (_logBox.InvokeRequired)
            {
                _logBox.Invoke(new Action<string>(_appendTextToLog), text);
            }
            else
            {
                _logBox.AppendText(Environment.NewLine + text);
                _logBox.SelectionStart = _logBox.Text.Length;
                _logBox.ScrollToCaret();
            }
        }
    }
}
