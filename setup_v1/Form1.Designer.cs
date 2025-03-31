namespace setup_v1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            logBox = new RichTextBox();
            install_btn = new Button();
            button2 = new Button();
            progress = new ProgressBar();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(-2, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(597, 93);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(11, 42);
            label2.MaximumSize = new Size(450, 0);
            label2.Name = "label2";
            label2.Size = new Size(402, 32);
            label2.TabIndex = 3;
            label2.Text = "Once you click the 'Install' button, the installation will proceed in the background and will complete even if the window is closed.";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.ImageLocation = "";
            pictureBox1.InitialImage = Properties.Resources.logo;
            pictureBox1.Location = new Point(498, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(83, 76);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(11, 21);
            label1.Name = "label1";
            label1.Size = new Size(65, 16);
            label1.TabIndex = 0;
            label1.Text = "Installing";
            // 
            // logBox
            // 
            logBox.Enabled = false;
            logBox.Location = new Point(8, 117);
            logBox.Name = "logBox";
            logBox.Size = new Size(572, 353);
            logBox.TabIndex = 1;
            logBox.Text = "";
            // 
            // install_btn
            // 
            install_btn.FlatStyle = FlatStyle.Popup;
            install_btn.Location = new Point(377, 487);
            install_btn.Name = "install_btn";
            install_btn.Size = new Size(75, 23);
            install_btn.TabIndex = 2;
            install_btn.Text = "Install";
            install_btn.UseVisualStyleBackColor = true;
            install_btn.Click += button1_Click;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Popup;
            button2.Location = new Point(505, 487);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // progress
            // 
            progress.Location = new Point(8, 93);
            progress.Name = "progress";
            progress.Size = new Size(572, 23);
            progress.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(594, 522);
            Controls.Add(progress);
            Controls.Add(button2);
            Controls.Add(install_btn);
            Controls.Add(logBox);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Setup - Mark of Idle ";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private PictureBox pictureBox1;
        private RichTextBox logBox;
        private Button install_btn;
        private Button button2;
        private ProgressBar progress;
        private Label label2;
    }
}
