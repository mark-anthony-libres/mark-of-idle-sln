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

            threshold_field.Value = this.script_instance.settings.result.threshold;

            start_on_boot_yes.Checked = false;
            start_on_boot_no.Checked = true;

            if (this.script_instance.boot_start_up.IsBootStartUp)
            {
                start_on_boot_yes.Checked = true;
                start_on_boot_no.Checked = false;
            }

            if (!this.script_instance.settings.result.is_active)
            {
                //when program is not active
                launch_btn.Text = "Start";
                logs_viewer.ScrollBars = RichTextBoxScrollBars.Vertical;
                start_on_boot_yes.Enabled = true;
                start_on_boot_no.Enabled = true;
            }
            else
            {
                //when program is already running 
                launch_btn.Text = "Stop";
                threshold_field.Enabled = false;
                saveBtn.Enabled = false;
                logs_viewer.ScrollBars = RichTextBoxScrollBars.None;
                start_on_boot_yes.Enabled = false;
                start_on_boot_no.Enabled = false;


                //Task.Run(() => this.script_instance.activate());

            }

            System.Windows.Forms.Timer addLogTimer = new System.Windows.Forms.Timer();
            addLogTimer.Interval = 1000;  // 1000 milliseconds = 1 second
            addLogTimer.Tick += (sender, e) =>
            {

                List<string> newContent = this.script_instance.logs.ExcludeDifferences(logs_viewer.Text);

                if (newContent.Count > 0)
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

        private void launch_btn_Click(object sender, EventArgs e)
        {
            this.script_instance = new Script();

            start_on_boot_yes.Checked = false;
            start_on_boot_no.Checked = true;

            if (this.script_instance.boot_start_up.IsBootStartUp)
            {
                start_on_boot_yes.Checked = true;
                start_on_boot_no.Checked = false;
            }

            if (!this.script_instance.settings.result.is_active)
            {
                //when user press Start btn

                launch_btn.Text = "Stop";
                threshold_field.Enabled = false;
                saveBtn.Enabled = false;
                logs_viewer.ScrollBars = RichTextBoxScrollBars.None;
                start_on_boot_yes.Enabled = false;
                start_on_boot_no.Enabled = false;

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
                start_on_boot_yes.Enabled = true;
                start_on_boot_no.Enabled = true;

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
            settings_data.threshold = (int)threshold_field.Value;

            this.script_instance.settings.set(settings_data);

            //when user choose "no" button on "start on boot" field
            if (start_on_boot_no.Checked)
            {
                this.script_instance.boot_start_up.RemoveBootAtStartUp();
            }
            else
            {
                //when user choose "yes" button on "start on boot" field
                this.script_instance.boot_start_up.SetBootAtStartUp();
            }

            MessageBox.Show("Data has been successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
