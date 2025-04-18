using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Net.Http; 
using System.Threading.Tasks;
using HtmlAgilityPack; 
using WebElementMonitor.Services;

namespace WebElementMonitor
{
    public partial class MainForm : Form
    {
        // Service Managers / Engines
        private readonly ProfileManager _profileManager;
        private readonly WebService _webService;
        // private readonly MonitorEngine _monitorEngine; // TODO: Dodati kasnije

        // Monitoring State 
        private System.Windows.Forms.Timer _monitorTimer = new System.Windows.Forms.Timer();
        private string? _lastKnownElementHash = null;
        private volatile bool _isMonitoring = false; 

        public MainForm()
        {
            InitializeComponent();

            // Instanciraj Service Managere
            _profileManager = new ProfileManager();
            _webService = new WebService(); // WebService sam konfigurira svoj HttpClient

            // Učitaj i prikaži profile
            PopulateProfilesComboBox();

            // Postavi inicijalni status
            UpdateStatus("Ready. Select a profile to start monitoring.");
        }

        // --- Metode za Upravljanje Profilima ---

        private void PopulateProfilesComboBox()
        {
            var profiles = _profileManager.GetAllProfiles();
            object? previouslySelectedItem = cmbProfiles.SelectedItem;

            cmbProfiles.DataSource = null;
            cmbProfiles.Items.Clear();

            if (profiles != null && profiles.Count > 0)
            {
                cmbProfiles.DataSource = profiles;
                cmbProfiles.DisplayMember = "ProfileName";

                if (previouslySelectedItem != null && profiles.Contains(previouslySelectedItem as MonitoringProfile))
                {
                    cmbProfiles.SelectedItem = previouslySelectedItem;
                }
                else
                {
                    cmbProfiles.SelectedIndex = 0;
                }
            }
            else
            {
                cmbProfiles.SelectedIndex = -1;
            }

            UpdateProfileActionButtons();
            UpdateSelectedProfileDetails();
        }

        private void UpdateProfileActionButtons()
        {
            bool profileSelected = cmbProfiles.SelectedIndex != -1;
           
            btnEditProfile.Enabled = profileSelected && !_isMonitoring;
            btnDeleteProfile.Enabled = profileSelected && !_isMonitoring;
            btnAddProfile.Enabled = !_isMonitoring; 
        }

        private void UpdateSelectedProfileDetails()
        {
            MonitoringProfile? selectedProfile = null;
            // Sigurno čitanje SelectedItem s UI threada
            if (cmbProfiles.InvokeRequired)
            {
                selectedProfile = (MonitoringProfile?)cmbProfiles.Invoke(new Func<MonitoringProfile?>(() => cmbProfiles.SelectedItem as MonitoringProfile));
            }
            else
            {
                selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            }


            if (selectedProfile != null)
            {
                // Prikazujemo podatke neovisno o _isMonitoring
                lblDisplayUrl.Text = selectedProfile.Url;
                lblDisplayElementId.Text = selectedProfile.ElementId;
                numInterval.Value = Math.Max(numInterval.Minimum,
                                           Math.Min(numInterval.Maximum, selectedProfile.IntervalMinutes));

                // Omogućavanje kontrola ovisi o _isMonitoring
                numInterval.Enabled = !_isMonitoring;
                btnStartStop.Enabled = true; // Start/Stop je uvijek omogućen ako je profil odabran

                if (!_isMonitoring)
                {
                    btnStartStop.Text = "Start Monitoring";
                    UpdateStatus($"Ready to monitor '{selectedProfile.ProfileName}'.");
                }
              
            }
            else
            {
                
                lblDisplayUrl.Text = "(no profile selected)";
                lblDisplayElementId.Text = "(no profile selected)";
                numInterval.Enabled = false;
                numInterval.Value = numInterval.Minimum;
                btnStartStop.Enabled = false; // Onemogući Start/Stop ako nema profila
                btnStartStop.Text = "Start Monitoring";
                UpdateStatus("Select or add a profile.");
            }
            // Ažuriraj i Edit/Delete gumbe za svaki slučaj
            UpdateProfileActionButtons();
        }

        // --- Event Handlers za Gumbe za Upravljanje Profilima ---

        private void btnAddProfile_Click(object sender, EventArgs e)
        {
            // Ne bi trebalo biti moguće kliknuti ako je _isMonitoring true, ali provjerimo
            if (_isMonitoring) return;

            using (ProfileForm profileForm = new ProfileForm())
            {
                if (profileForm.ShowDialog(this) == DialogResult.OK)
                {
                    MonitoringProfile newProfile = profileForm.Profile;
                    _profileManager.AddProfile(newProfile);

                    if (!_profileManager.SaveProfilesToFile())
                    {
                        MessageBox.Show("Failed to save profiles after adding.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    PopulateProfilesComboBox();
                    cmbProfiles.SelectedItem = newProfile;
                }
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (_isMonitoring) return;

            MonitoringProfile? selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            if (selectedProfile == null)
            {
                MessageBox.Show("Please select a profile to edit.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ProfileForm profileForm = new ProfileForm(selectedProfile))
            {
                if (profileForm.ShowDialog(this) == DialogResult.OK)
                {
                    if (!_profileManager.SaveProfilesToFile())
                    {
                        MessageBox.Show("Failed to save profiles after editing.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    PopulateProfilesComboBox(); // Osvježi UI
                }
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (_isMonitoring) return;

            MonitoringProfile? selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            if (selectedProfile == null)
            {
                MessageBox.Show("Please select a profile to delete.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmation = MessageBox.Show($"Are you sure you want to delete the profile '{selectedProfile.ProfileName}'?",
                                                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                _profileManager.DeleteProfile(selectedProfile);
                if (!_profileManager.SaveProfilesToFile())
                {
                    MessageBox.Show("Failed to save profiles after deleting.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                PopulateProfilesComboBox();
            }
        }

        private void cmbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ako je praćenje aktivno, zaustavi ga (sigurno pozivanje s UI threada)
            if (_isMonitoring)
            {
                StopMonitoringInternal();
                UpdateStatus($"Monitoring stopped due to profile change. Select profile and click Start.");
            }
            // Ažuriraj gumbe i detalje (ovo je već na UI threadu)
            UpdateSelectedProfileDetails();
        }


        // --- Logika Praćenja ---

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_isMonitoring)
            {
                StopMonitoringInternal();
            }
            else
            {
                MonitoringProfile? selectedProfile = cmbProfiles.SelectedItem as MonitoringProfile;
                if (selectedProfile != null)
                {
                    StartMonitoringInternal(selectedProfile);
                }
                // Nema potrebe za else, UpdateSelectedProfileDetails osigurava da je gumb onemogućen ako nema profila
            }
        }

        private void StartMonitoringInternal(MonitoringProfile profile)
        {
            _isMonitoring = true;
            _lastKnownElementHash = null;

            // Onemogući kontrole (poziva se s UI threada iz btnStartStop_Click)
            cmbProfiles.Enabled = false;
            numInterval.Enabled = false;
            UpdateProfileActionButtons(); // Onemogućit će Add/Edit/Delete

            _monitorTimer.Interval = Math.Max(1000, profile.IntervalMinutes * 60 * 1000);
            btnStartStop.Text = "Stop Monitoring";
            UpdateStatus($"Starting monitoring for '{profile.ProfileName}'...");

            _monitorTimer.Tick -= MonitorTimer_Tick;
            _monitorTimer.Tick += MonitorTimer_Tick;

            UpdateStatus($"Performing initial check for '{profile.ProfileName}'...");
            // Koristi Task.Run za inicijalnu provjeru da ne blokira UI
            Task.Run(async () =>
            {
                await PerformCheckAsync();
                // Nakon prve provjere, pokreni timer za buduće provjere (ako smo još u stanju praćenja)
                if (_isMonitoring && !_monitorTimer.Enabled)
                {
                    // Osiguraj pokretanje timera na UI threadu
                    SafeStartTimer();
                }
            });
        }

        private void StopMonitoringInternal()
        {
            // Poziva se ili iz UI threada (cmbProfiles_SelectedIndexChanged, btnStartStop_Click)
            // ili iz pozadinskog threada (PerformCheckAsync) preko BeginInvoke
            _monitorTimer.Stop();
            _isMonitoring = false;
            _lastKnownElementHash = null;

            // Omogući kontrole (već smo na UI threadu ili pozvani preko BeginInvoke)
            cmbProfiles.Enabled = true;
            UpdateSelectedProfileDetails(); // Ovo će ažurirati gumbe i kontrole ispravno
            UpdateStatus("Monitoring stopped.");
            // Tekst gumba će se ispraviti unutar UpdateSelectedProfileDetails
        }

        // Sigurno pokretanje timera s UI threada
        private void SafeStartTimer()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => {
                    if (_isMonitoring) _monitorTimer.Start(); 
                }));
            }
            else
            {
                if (_isMonitoring) _monitorTimer.Start(); 
            }
        }


        private async void MonitorTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isMonitoring)
            {
                _monitorTimer.Stop();
                return;
            }
            _monitorTimer.Stop(); // Pauziraj dok traje provjera

            Console.WriteLine($"DEBUG: Timer Tick - Calling PerformCheckAsync at {DateTime.Now}");
            await PerformCheckAsync();

            // Ponovno pokreni timer ako je praćenje još aktivno
            if (_isMonitoring)
            {
                SafeStartTimer(); 
                Console.WriteLine($"DEBUG: Timer Tick - Restarting timer at {DateTime.Now}");
            }
        }

        private async Task PerformCheckAsync()
        {
            MonitoringProfile? currentProfile = null;
            bool checkMonitoringState = _isMonitoring; 

            // Dohvati profil sigurno
            if (this.InvokeRequired)
            {
                currentProfile = (MonitoringProfile?)this.Invoke(new Func<MonitoringProfile?>(() => cmbProfiles.SelectedItem as MonitoringProfile));
            }
            else
            {
                currentProfile = cmbProfiles.SelectedItem as MonitoringProfile;
            }

            if (!checkMonitoringState || currentProfile == null)
            {
                Console.WriteLine($"DEBUG: PerformCheckAsync exiting early. IsMonitoring: {checkMonitoringState}, ProfileNull: {currentProfile == null}");
                // Osiguraj da je sve zaustavljeno ako je stanje nekonzistentno
                if (_isMonitoring) 
                {
                    if (this.InvokeRequired) { this.BeginInvoke(new Action(() => StopMonitoringInternal())); }
                    else { StopMonitoringInternal(); }
                }
                return;
            }

            string profileName = currentProfile.ProfileName ?? "Unknown"; // Za logiranje
            Console.WriteLine($"DEBUG: Starting check logic for {profileName}");
            UpdateStatus($"Checking '{profileName}'...");

            string? htmlContent = null;
            string? elementHtml = null;
            string currentHash = "ERROR_HASH"; // Default hash u slučaju nepredviđene greške

            try
            {
                Console.WriteLine("DEBUG: Calling GetWebsiteContentAsync...");
                htmlContent = await _webService.GetWebsiteContentAsync(currentProfile.Url).ConfigureAwait(false); 
                Console.WriteLine($"DEBUG: GetWebsiteContentAsync finished. Content null: {htmlContent == null}");

                if (htmlContent == null)
                {
                    UpdateStatus($"Failed to fetch content for '{profileName}'. Retrying later.");
                    return; // Ne uspoređuj ako nema sadržaja
                }

                Console.WriteLine("DEBUG: Calling ExtractElementHtml...");
                try
                {
                    elementHtml = _webService.ExtractElementHtml(htmlContent, currentProfile.ElementId);
                    Console.WriteLine($"DEBUG: ExtractElementHtml finished. Element null: {elementHtml == null}");
                    if (elementHtml == null)
                    {
                        UpdateStatus($"Element ID '{currentProfile.ElementId}' not found on {currentProfile.Url}.");
                        
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error parsing HTML for '{profileName}': {ex.Message}");
                    Console.WriteLine($"DEBUG: Error in ExtractElementHtml: {ex.Message}");
                    elementHtml = null; // Greška, tretiraj kao da nije pronađen
                }

                Console.WriteLine("DEBUG: Calculating Hash...");
                if (elementHtml == null)
                {
                    currentHash = "ELEMENT_NOT_FOUND_OR_ERROR"; // Poseban hash
                }
                else
                {
                    currentHash = WebService.CalculateHash(elementHtml); // Statička metoda
                }
                Console.WriteLine($"DEBUG: Hash Calculated: {currentHash}");


                // Usporedba - provjeri opet _isMonitoring prije ažuriranja stanja
                if (_isMonitoring) 
                {
                    if (_lastKnownElementHash == null)
                    {
                        _lastKnownElementHash = currentHash;
                        UpdateStatus($"Initial state captured for '{profileName}'. Monitoring...");
                        Console.WriteLine($"DEBUG: Initial hash set for {profileName}: {_lastKnownElementHash}");
                    }
                    else if (_lastKnownElementHash != currentHash)
                    {
                        string oldHash = _lastKnownElementHash;
                        _lastKnownElementHash = currentHash;

                        string changeMessage = $"Change detected for '{profileName}'!";
                        UpdateStatus(changeMessage);
                        Console.WriteLine($"DEBUG: CHANGE DETECTED for {profileName}. Old: {oldHash}, New: {currentHash}");

                        bool elementNotFoundNow = (currentHash == "ELEMENT_NOT_FOUND_OR_ERROR");
                        ShowChangeNotification(currentProfile, elementNotFoundNow, oldHash, currentHash);
                    }
                    else
                    {
                        UpdateStatus($"'{profileName}' checked. No changes ({DateTime.Now:HH:mm:ss}).");
                        Console.WriteLine($"DEBUG: No changes for {profileName}.");
                    }
                }
                else
                {
                    Console.WriteLine($"DEBUG: Monitoring stopped during check for {profileName}.");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error during check for '{profileName}': {ex.Message}");
                Console.WriteLine($"DEBUG: UNHANDLED EXCEPTION in PerformCheckAsync for {profileName}: {ex}");
              
            }
            finally
            {
                Console.WriteLine($"DEBUG: PerformCheckAsync finished logic for {profileName}");
            }
        }


        // --- Pomoćne Metode ---

        private void UpdateStatus(string message)
        {
            if (lblStatus.InvokeRequired)
            {
                try { lblStatus.BeginInvoke(new Action(() => lblStatus.Text = $"Status: {message}")); } catch (ObjectDisposedException) { /* Zanemari ako se forma zatvara */ }
            }
            else
            {
                if (!lblStatus.IsDisposed) lblStatus.Text = $"Status: {message}";
            }
        }

        private void ShowChangeNotification(MonitoringProfile profile, bool wasElementNotFound, string oldHash, string newHash)
        {
            // TODO: Stvarni prikaz Windows notifikacije
            Console.WriteLine($"--- CHANGE DETECTED ---");
            Console.WriteLine($"Profile: {profile.ProfileName}, Element Found: {!wasElementNotFound}, Time: {DateTime.Now}");
            Console.WriteLine($"Old Hash: {oldHash} -> New Hash: {newHash}");
            Console.WriteLine($"-----------------------");

       
            Action showMsg = () => MessageBox.Show(
                this, // Postavi ownera
                $"Change detected for profile: '{profile.ProfileName}'\n" +
                $"URL: {profile.Url}\n" +
                $"Element Found: {!wasElementNotFound}",
                "Website Change Detected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (this.InvokeRequired) { this.BeginInvoke(showMsg); } else { showMsg(); }

       
        }

        // --- Čišćenje prilikom zatvaranja forme ---
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zaustavi praćenje ako je aktivno (sigurno)
            if (_isMonitoring)
            {
                StopMonitoringInternal();
            }

            // Spremi profile
            _profileManager.SaveProfilesToFile();

            // Oslobodi timer
            _monitorTimer?.Dispose();
        }

        // --- Prazni Event Handleri ---
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}