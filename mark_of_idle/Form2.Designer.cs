namespace mark_of_idle
{
    partial class Form2
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
            label1 = new Label();
            threshold_field = new NumericUpDown();
            launch_btn = new Button();
            saveBtn = new Button();
            button3 = new Button();
            logs_viewer = new RichTextBox();
            radioButton1 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)threshold_field).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(73, 17);
            label1.TabIndex = 1;
            label1.Text = "Threshold";
            // 
            // threshold_field
            // 
            threshold_field.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            threshold_field.Location = new Point(91, 12);
            threshold_field.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            threshold_field.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            threshold_field.Name = "threshold_field";
            threshold_field.Size = new Size(120, 25);
            threshold_field.TabIndex = 2;
            threshold_field.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // launch_btn
            // 
            launch_btn.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            launch_btn.Location = new Point(12, 444);
            launch_btn.Name = "launch_btn";
            launch_btn.Size = new Size(84, 38);
            launch_btn.TabIndex = 3;
            launch_btn.Text = "Stop";
            launch_btn.UseVisualStyleBackColor = true;
            launch_btn.Click += launch_btn_Click;
            // 
            // saveBtn
            // 
            saveBtn.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            saveBtn.Location = new Point(102, 444);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(84, 38);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "Save";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.Location = new Point(573, 444);
            button3.Name = "button3";
            button3.RightToLeft = RightToLeft.Yes;
            button3.Size = new Size(84, 38);
            button3.TabIndex = 5;
            button3.Text = "Exit";
            button3.UseVisualStyleBackColor = true;
            // 
            // logs_viewer
            // 
            logs_viewer.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logs_viewer.Location = new Point(12, 154);
            logs_viewer.Name = "logs_viewer";
            logs_viewer.ReadOnly = true;
            logs_viewer.Size = new Size(645, 271);
            logs_viewer.TabIndex = 6;
            logs_viewer.Text = "";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(12, 56);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(94, 19);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 507);
            Controls.Add(radioButton1);
            Controls.Add(logs_viewer);
            Controls.Add(button3);
            Controls.Add(saveBtn);
            Controls.Add(launch_btn);
            Controls.Add(threshold_field);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "Form2";
            Text = "Mark of Idle";
            ((System.ComponentModel.ISupportInitialize)threshold_field).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown threshold_field;
        private Button launch_btn;
        private Button saveBtn;
        private Button button3;
        private RichTextBox logs_viewer;
        private RadioButton radioButton1;
    }
}
