using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace WebElementMonitor.Services
{
    internal class ProfileManager
    {
        private readonly string _profilesFilePath;
        private List<MonitoringProfile> _profiles = new List<MonitoringProfile>();

        public ProfileManager()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataFolder, "WebElementMonitor");
            Directory.CreateDirectory(appFolder);
            _profilesFilePath = Path.Combine(appFolder, "profiles.json");
            LoadProfilesFromFile();
        }

        private void LoadProfilesFromFile()
        {
            try
            {
                if (File.Exists(_profilesFilePath))
                {
                    string json = File.ReadAllText(_profilesFilePath);
                    var loadedProfiles = JsonSerializer.Deserialize<List<MonitoringProfile>>(json);

                    
                    if (loadedProfiles != null)
                    {
                        _profiles = loadedProfiles;
                    }
                   
                }
               
            }
            catch (JsonException jsonEx) 
            {
                Console.WriteLine($"Error deserializing profiles: {jsonEx.Message}. Starting with empty list.");
                _profiles = new List<MonitoringProfile>(); 
            }
            catch (Exception ex) // Hvatanje ostalih I/O ili drugih grešaka
            {
                Console.WriteLine($"Error loading profiles file: {ex.Message}. Starting with empty list.");
                _profiles = new List<MonitoringProfile>();
            }
        }

        public List<MonitoringProfile> GetAllProfiles()
        {
            return _profiles;
        }

        public void AddProfile(MonitoringProfile profile)
        {
            if (profile != null)
            {
                _profiles.Add(profile);
            }
        }

        public void DeleteProfile(MonitoringProfile profile)
        {
            if (profile != null) _profiles.Remove(profile);
        }
        public bool SaveProfilesToFile()
        {
            try {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_profiles, options);
                File.WriteAllText(_profilesFilePath, json);
                return true;
            
            }catch(Exception ex)
            {
                Console.WriteLine($"Error saving profiles: {ex.Message}");
                return false;
            }
        }
    }
}
