using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace WebElementMonitor
{
    public partial class MainForm : Form
    {

        private List<MonitoringProfile> _profiles = new List<MonitoringProfile>();

        private readonly string _profilesFilePath;


        public MainForm()
        {
            InitializeComponent();

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataFolder, "WebElementMonitor");
            Directory.CreateDirectory(appFolder);
            _profilesFilePath = Path.Combine(appFolder, "profiles.json");
            LoadProfiles();

        }


        private void LoadProfiles()
        {
            try
            {
                if (File.Exists(_profilesFilePath))
                {
                    string json = File.ReadAllText(_profilesFilePath);
                    _profiles = JsonSerializer.Deserialize<List<MonitoringProfile>>(json) ?? new List<MonitoringProfile>();
                }
                else
                {
                    _profiles = new List<MonitoringProfile>();
                }

            }
            catch (Exception ex)
            {
                //Error pri citanju/deserijalizaciji
                MessageBox.Show($"Error loading profiles: {ex.Message}\nStarting with empty profile list.", "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _profiles = new List<MonitoringProfile>();
            }

            //cistimo ComboBox prije punjenja
            cmbProfiles.DataSource = null;
            cmbProfiles.Items.Clear();

            //punimo ComboBox ucitanim profilima
            cmbProfiles.DataSource = _profiles;

            cmbProfiles.DisplayMember = "ProfileName";

            if (_profiles.Count > 0)
            {
                cmbProfiles.SelectedIndex = 0;
            }
            else
            {
                cmbProfiles.SelectedIndex = -1;
                //onemogucimo gumbe i kontrole za pracenje
                btnEditProfile.Enabled = false;
                btnDeleteProfile.Enabled = false;
                numInterval.Enabled = false;
                btnStartStop.Enabled = false;
                lblDisplayUrl.Text = "(no profile selected)";
                lblDisplayElementId.Text = "(no profile selected)";
            }

            UpdateProfileActionButtons();
        }

        private void SaveProfiles()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_profiles, options);
                File.WriteAllText(_profilesFilePath, json);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving profiles: {ex.Message} ", " Saving Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //pomocna metoda za azuriranje Enabled stanja gubma Edit i Delete
        private void UpdateProfileActionButtons()
        {
            bool profileSelected = cmbProfiles.SelectedIndex != -1;
            btnEditProfile.Enabled = profileSelected;
            btnDeleteProfile.Enabled = profileSelected;
        }

        private void RefreshComboBox()
        {
            object previouslySelectedItem = cmbProfiles.SelectedItem;

            //resetiramo datasource da combobox primijeti promjene u listi
            cmbProfiles.DataSource = null;
            cmbProfiles.DataSource = _profiles;
            cmbProfiles.DisplayMember = "ProfileName";

            if (previouslySelectedItem != null && _profiles.Contains(previouslySelectedItem as MonitoringProfile))
            {
                cmbProfiles.SelectedItem = previouslySelectedItem;
            }
            else if (_profiles.Count > 0)
            {
                cmbProfiles.SelectedIndex = 0;
            }
            else
            {
                cmbProfiles.SelectedIndex = -1;
            }
        }


        private void UpdateSelectedProfileDetails()
        {
            MonitoringProfile selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;

            if (selectedProfile != null)
            {
                lblDisplayUrl.Text = selectedProfile.Url;
                lblDisplayElementId.Text = selectedProfile.ElementId;
                // Osiguraj da interval stane u kontrole
                numInterval.Value = Math.Max(numInterval.Minimum,
                                           Math.Min(numInterval.Maximum, selectedProfile.IntervalMinutes));
                numInterval.Enabled = true;
                btnStartStop.Enabled = true;

                lblStatus.Text = $"Status: Ready to monitor '{selectedProfile.ProfileName}'.";
            }
            else
            {
                // Nema odabranog profila
                lblDisplayUrl.Text = "(no profile selected)";
                lblDisplayElementId.Text = "(no profile selected)";
                numInterval.Enabled = false;
                numInterval.Value = numInterval.Minimum; // Resetiraj na min
                btnStartStop.Enabled = false;
                lblStatus.Text = "Status: Select or add a profile.";
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnAddProfile_Click(object sender, EventArgs e)
        {
            using (ProfileForm profileForm = new ProfileForm())
            {
                if (profileForm.ShowDialog(this) == DialogResult.OK)
                {
                    MonitoringProfile newProfile = profileForm.Profile;
                    _profiles.Add(newProfile);

                    RefreshComboBox();

                    cmbProfiles.SelectedItem = newProfile;

                    SaveProfiles();
                    UpdateProfileActionButtons();
                }
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            // Provjeri je li nešto odabrano
            MonitoringProfile selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            if (selectedProfile == null)
            {
                MessageBox.Show("Please select a profile to edit.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ProfileForm profileForm = new ProfileForm(selectedProfile))
            {
                if (profileForm.ShowDialog(this) == DialogResult.OK)
                {

                    RefreshComboBox();
                    cmbProfiles.SelectedItem = selectedProfile;

                    SaveProfiles();
                    UpdateSelectedProfileDetails();
                }
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            MonitoringProfile selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            if (selectedProfile == null)
            {
                MessageBox.Show("Please select a profile to delete.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pitaj za potvrdu
            DialogResult confirmation = MessageBox.Show($"Are you sure you want to delete the profile '{selectedProfile.ProfileName}'?",
                                                        "Confirm Delete",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                int selectedIndex = cmbProfiles.SelectedIndex; // Zapamti indeks za kasnije

                _profiles.Remove(selectedProfile);

                RefreshComboBox();


                if (_profiles.Count > 0)
                {
                    cmbProfiles.SelectedIndex = Math.Min(selectedIndex, _profiles.Count - 1);
                }

                SaveProfiles();
                UpdateProfileActionButtons();
                UpdateSelectedProfileDetails();
            }
        }



        private void cmbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateProfileActionButtons();

            UpdateSelectedProfileDetails();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveProfiles(); 
        }
    }
}
