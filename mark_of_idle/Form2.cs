using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mark_of_idle.Settings;


namespace mark_of_idle
{
    public partial class Form2 : Form
    {
        private Script script_instance;

        public Form2()
        {
            InitializeComponent();
            //CheckIfAdministratorAndElevate();
            this.script_instance = new Script();

            Debug.WriteLine(this.IsAdministrator());

            threshold_field.Value = this.script_instance.settings.result.threshold;

            if (!this.script_instance.settings.result.is_active)
            {
                //when program is not active
                launch_btn.Text = "Start";
                logs_viewer.ScrollBars = RichTextBoxScrollBars.Vertical;
            }
            else
            {
                //when program is already running 
                launch_btn.Text = "Stop";
                threshold_field.Enabled = false;
                saveBtn.Enabled = false;
                logs_viewer.ScrollBars = RichTextBoxScrollBars.None;

                //Task.Run(() => this.script_instance.activate());

            }
            

            
            System.Windows.Forms.Timer addLogTimer = new System.Windows.Forms.Timer();
            addLogTimer.Interval = 1000;  // 1000 milliseconds = 1 second
            addLogTimer.Tick += (sender, e) =>
            {

                List<string> newContent = this.script_instance.logs.ExcludeDifferences(logs_viewer.Text);

                if(newContent.Count > 0)
                {
                    if (logs_viewer.Text.Length > 0)
                    {
                        logs_viewer.AppendText("\n" + string.Join(Environment.NewLine, newContent));
                    }
                    else
                    {
                        logs_viewer.AppendText(string.Join(Environment.NewLine, newContent));
                    }

                    logs_viewer.SelectionStart = logs_viewer.Text.Length; // Set caret to the end
                    logs_viewer.ScrollToCaret(); // Scroll to caret (end)
                }


             
            };
            addLogTimer.Start();
            logs_viewer.SelectionStart = logs_viewer.Text.Length; // Set caret to the end
            logs_viewer.ScrollToCaret(); // Scroll to caret (end)
        }


        // Method to check if the current process is running as Administrator
        private bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Method to elevate the process if it's not already running as administrator
        private void CheckIfAdministratorAndElevate()
        {
            if (!IsAdministrator())
            {
                // If not running as Administrator, restart the application with elevated privileges
                RunAsAdministrator();
                Application.Exit();  // Exit the current non-elevated process immediately
            }
        }

        // Method to restart the application with Administrator privileges
        private void RunAsAdministrator()
        {

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDirectory = System.IO.Path.GetDirectoryName(exePath);
            string exeFile = System.IO.Path.Combine(exeDirectory, "mark_of_idle.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = exeFile, // Path to the current executable
                Verb = "runas", // This will prompt the user for elevated privileges
                UseShellExecute = true
            };

            try
            {
                Process.Start(startInfo); // Start the process with admin rights
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to elevate the process: {ex.Message}");
            }
        }


        private void launch_btn_Click(object sender, EventArgs e)
        {
            this.script_instance = new Script();

            if (!this.script_instance.settings.result.is_active)
            {
                //when user press Start btn

                launch_btn.Text = "Stop";
                threshold_field.Enabled = false;
                saveBtn.Enabled = false;
                logs_viewer.ScrollBars = RichTextBoxScrollBars.None;

                Data settings_data = this.script_instance.settings.result.copy();
                settings_data.is_active = true;

                this.script_instance.settings.set(settings_data);
                Task.Run(() => this.script_instance.activate());

                logs_viewer.SelectionStart = logs_viewer.Text.Length; // Set caret to the end
                logs_viewer.ScrollToCaret(); // Scroll to caret (end)

            }
            else
            {
                //when user press Stop btn
                launch_btn.Text = "Start";
                threshold_field.Enabled = true;
                saveBtn.Enabled = true;
                logs_viewer.ScrollBars = RichTextBoxScrollBars.Vertical;

                Data settings_data = this.script_instance.settings.result.copy();
                settings_data.is_active = false;

                this.script_instance.settings.set(settings_data);

                logs_viewer.SelectionStart = logs_viewer.Text.Length; // Set caret to the end
                logs_viewer.ScrollToCaret(); // Scroll to caret (end)
            }

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            this.script_instance = new Script();
            Data settings_data = this.script_instance.settings.result.copy();
            settings_data.is_active = false;
            settings_data.threshold = (int) threshold_field.Value;

            this.script_instance.settings.set(settings_data);

            MessageBox.Show("Data has been successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


    }
}
