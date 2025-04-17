using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebElementMonitor
{
    public partial class ProfileForm : Form
    {

        public MonitoringProfile Profile { get;private set; }

        public ProfileForm(MonitoringProfile profileToEdit=null)
        {
            InitializeComponent();
            if (profileToEdit != null)
            {
                this.Profile = profileToEdit;
                this.Text = "Edit Profile";
                txtName.Text = Profile.ProfileName;
                txtUrl.Text = Profile.Url;
                txtElementId.Text = Profile.ElementId;
                numIntervalMinutes.Value = Math.Max(numIntervalMinutes.Minimum, Math.Min(numIntervalMinutes.Maximum, Profile.IntervalMinutes));
            }
            else
            {
                this.Profile = new MonitoringProfile();
                this.Text = "Add new Profile";
                txtName.Text = Profile.ProfileName;
                txtUrl.Text = Profile.Url;
                txtElementId.Text = Profile.ElementId;
                numIntervalMinutes.Value = Profile.IntervalMinutes;
            }
        }


        //EVENT HANDLER FOR OK BUTTON

        private void btnOk_Click(object sender,EventArgs e)
        {

            //VALIDATIONS
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Profile Name cannot be empty", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtUrl.Text) || !Uri.TryCreate(txtUrl.Text,UriKind.Absolute,out _))
            {
                MessageBox.Show("Please enter a valid URL (e.g., https://www.example.com).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtElementId.Text))
            {
                MessageBox.Show("Element ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtElementId.Focus();
                return;
            }

            //IF VALIDATIONS OK
            Profile.ProfileName = txtName.Text.Trim();
            Profile.Url = txtUrl.Text.Trim();
            Profile.ElementId = txtElementId.Text.Trim();
            Profile.IntervalMinutes = (int)numIntervalMinutes.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
       
    }
}
