namespace WebElementMonitor
{
    partial class ProfileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            label1 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtUrl = new TextBox();
            label3 = new Label();
            txtElementId = new TextBox();
            label4 = new Label();
            numIntervalMinutes = new NumericUpDown();
            btnOk = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numIntervalMinutes).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 34);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 1;
            label1.Text = "Profile Name";
            // 
            // txtName
            // 
            txtName.Location = new Point(119, 26);
            txtName.Name = "txtName";
            txtName.Size = new Size(214, 23);
            txtName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 71);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 3;
            label2.Text = "URL";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(119, 63);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(214, 23);
            txtUrl.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 112);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 5;
            label3.Text = "Element ID";
            // 
            // txtElementId
            // 
            txtElementId.Location = new Point(119, 104);
            txtElementId.Name = "txtElementId";
            txtElementId.Size = new Size(212, 23);
            txtElementId.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 146);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 7;
            label4.Text = "Interval (min)";
            // 
            // numIntervalMinutes
            // 
            numIntervalMinutes.Location = new Point(119, 138);
            numIntervalMinutes.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numIntervalMinutes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIntervalMinutes.Name = "numIntervalMinutes";
            numIntervalMinutes.Size = new Size(214, 23);
            numIntervalMinutes.TabIndex = 8;
            numIntervalMinutes.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnOk
            // 
            btnOk.Location = new Point(256, 224);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 9;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(28, 224);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ProfileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(numIntervalMinutes);
            Controls.Add(label4);
            Controls.Add(txtElementId);
            Controls.Add(label3);
            Controls.Add(txtUrl);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ProfileForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Profile Settings";
            ((System.ComponentModel.ISupportInitialize)numIntervalMinutes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private Label label1;
        private TextBox txtName;
        private Label label2;
        private TextBox txtUrl;
        private Label label3;
        private TextBox txtElementId;
        private Label label4;
        private NumericUpDown numIntervalMinutes;
        private Button btnOk;
        private Button btnCancel;
    }
}