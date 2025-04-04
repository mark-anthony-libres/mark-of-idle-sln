using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace setup_v1
{
    public partial class Form1 : Form
    {
        private readonly InstallManager _installManager;

        public Form1()
        {
            InitializeComponent();
            _installManager = new InstallManager(logBox, progress);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            install_btn.Enabled = false;
            _installManager.PreInstallation();
            await _installManager.ExtractResourcesAsync();

            progress.Minimum = 0;
            progress.Maximum = _installManager.ProgressMaxValue();

            string batFilePath = Path.Combine(_installManager.MarkOfIdleFolder, "setup.bat");
            Debug.WriteLine("Setup file path: " + batFilePath);
            int exitCode = await _installManager.RunBatFileAsync(batFilePath);

            logBox.Enabled = true;

            if (exitCode > 0)
            {
                MessageBox.Show("The installation could not be completed. Please refer to the logs for further details.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string app = Path.Combine(_installManager.MarkOfIdleFolder, _installManager.ExecutableName);
            MessageBox.Show("Installation completed successfully! Thank you for choosing Mark of Idle.", "Installation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Debug.WriteLine("Starting the app: " + app);

            Process.Start(app);
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }

    

    
}
