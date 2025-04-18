namespace WebElementMonitor
{
    partial class MainForm
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
            groupBox1 = new GroupBox();
            btnDeleteProfile = new Button();
            btnEditProfile = new Button();
            btnAddProfile = new Button();
            cmbProfiles = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnStartStop = new Button();
            numInterval = new NumericUpDown();
            label4 = new Label();
            lblDisplayElementId = new Label();
            label3 = new Label();
            lblDisplayUrl = new Label();
            label2 = new Label();
            lblStatus = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInterval).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDeleteProfile);
            groupBox1.Controls.Add(btnEditProfile);
            groupBox1.Controls.Add(btnAddProfile);
            groupBox1.Controls.Add(cmbProfiles);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(372, 150);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Profile Management";
            // 
            // btnDeleteProfile
            // 
            btnDeleteProfile.Location = new Point(221, 87);
            btnDeleteProfile.Name = "btnDeleteProfile";
            btnDeleteProfile.Size = new Size(75, 23);
            btnDeleteProfile.TabIndex = 4;
            btnDeleteProfile.Text = "Delete";
            btnDeleteProfile.UseVisualStyleBackColor = true;
            btnDeleteProfile.Click += btnDeleteProfile_Click;
            // 
            // btnEditProfile
            // 
            btnEditProfile.Location = new Point(122, 87);
            btnEditProfile.Name = "btnEditProfile";
            btnEditProfile.Size = new Size(75, 23);
            btnEditProfile.TabIndex = 3;
            btnEditProfile.Text = "Edit...";
            btnEditProfile.UseVisualStyleBackColor = true;
            btnEditProfile.Click += btnEditProfile_Click;
            // 
            // btnAddProfile
            // 
            btnAddProfile.Location = new Point(16, 87);
            btnAddProfile.Name = "btnAddProfile";
            btnAddProfile.Size = new Size(75, 23);
            btnAddProfile.TabIndex = 2;
            btnAddProfile.Text = "Add...";
            btnAddProfile.UseVisualStyleBackColor = true;
            btnAddProfile.Click += btnAddProfile_Click;
            // 
            // cmbProfiles
            // 
            cmbProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProfiles.FormattingEnabled = true;
            cmbProfiles.ImeMode = ImeMode.KatakanaHalf;
            cmbProfiles.Location = new Point(140, 32);
            cmbProfiles.Name = "cmbProfiles";
            cmbProfiles.Size = new Size(121, 23);
            cmbProfiles.TabIndex = 1;
            cmbProfiles.SelectedIndexChanged += cmbProfiles_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 35);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 0;
            label1.Text = "Monitoring Profile:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnStartStop);
            groupBox2.Controls.Add(numInterval);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(lblDisplayElementId);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(lblDisplayUrl);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 168);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(372, 187);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Monitoring Control";
            // 
            // btnStartStop
            // 
            btnStartStop.Enabled = false;
            btnStartStop.Location = new Point(96, 136);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(165, 35);
            btnStartStop.TabIndex = 6;
            btnStartStop.Text = "Start Monitoring";
            btnStartStop.UseVisualStyleBackColor = true;
            btnStartStop.Click += btnStartStop_Click;
            // 
            // numInterval
            // 
            numInterval.Enabled = false;
            numInterval.Location = new Point(214, 88);
            numInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numInterval.Name = "numInterval";
            numInterval.Size = new Size(120, 23);
            numInterval.TabIndex = 5;
            numInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 96);
            label4.Name = "label4";
            label4.Size = new Size(139, 15);
            label4.TabIndex = 4;
            label4.Text = "Check Interval (minutes):";
            // 
            // lblDisplayElementId
            // 
            lblDisplayElementId.BorderStyle = BorderStyle.Fixed3D;
            lblDisplayElementId.Location = new Point(91, 66);
            lblDisplayElementId.Name = "lblDisplayElementId";
            lblDisplayElementId.Size = new Size(243, 15);
            lblDisplayElementId.TabIndex = 3;
            lblDisplayElementId.Text = "(select a profile)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 66);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 2;
            label3.Text = "Element ID";
            // 
            // lblDisplayUrl
            // 
            lblDisplayUrl.BorderStyle = BorderStyle.Fixed3D;
            lblDisplayUrl.Location = new Point(91, 38);
            lblDisplayUrl.Name = "lblDisplayUrl";
            lblDisplayUrl.Size = new Size(243, 15);
            lblDisplayUrl.TabIndex = 1;
            lblDisplayUrl.Text = "(select a profile)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 38);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 0;
            label2.Text = "URL";
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 427);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(910, 23);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Status: Ready. Select a profile.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(910, 450);
            Controls.Add(lblStatus);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "MainForm";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInterval).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnAddProfile;
        private ComboBox cmbProfiles;
        private Label label1;
        private Button btnDeleteProfile;
        private Button btnEditProfile;
        private GroupBox groupBox2;
        private Label lblDisplayUrl;
        private Label label2;
        private Label lblDisplayElementId;
        private Label label3;
        private NumericUpDown numInterval;
        private Label label4;
        private Button btnStartStop;
        private Label lblStatus;
    }
}
