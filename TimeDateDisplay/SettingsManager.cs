using System.IO;
using System.Text.Json;
using System.Reflection;

namespace TimeDateDisplay {
    public static class SettingsManager {
        private static readonly string SettingsFileName = "TimeDateDisplayConfig_7B3F9D2E8A1C.json";
        private static readonly string SettingsPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory,
            SettingsFileName
        );

        public static Settings LoadSettings() {
            try {
                if (File.Exists(SettingsPath)) {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<Settings>(json);
                    return settings ?? new Settings();
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            return new Settings(); // Return default settings if loading fails
        }

        public static void SaveSettings(Settings settings) {
            try {
                var options = new JsonSerializerOptions {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(SettingsPath, json);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}