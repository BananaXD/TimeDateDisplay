using System.Windows.Media;

namespace TimeDateDisplay {
    public class Settings {
        public double Opacity { get; set; } = 1.0;
        public bool AlwaysOnTop { get; set; } = true;
        public string FontColor { get; set; } = "#FFFFFF"; // Default white color
        public string BackgroundColor { get; set; } = "#FF2D2D2D"; // Default dark gray color
        public string FontTimeFamilyName { get; set; } = "Segoe UI"; // Default font
        public string FontDateFamilyName { get; set; } = "Segoe UI"; // Default font

        public FontFamily GetDateFontFamily() {
            try {
                return new FontFamily(FontDateFamilyName);
            } catch {
                return new FontFamily("Segoe UI");
            }
        }

        public FontFamily GetTimeFontFamily() {
            try {
                return new FontFamily(FontTimeFamilyName);
            } catch {
                return new FontFamily("Segoe UI");
            }
        }

        // Convert string color to SolidColorBrush
        public SolidColorBrush GetFontBrush() {
            try {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(FontColor));
            } catch {
                return new SolidColorBrush(Colors.White); // Fallback to white
            }
        }

        // Convert string color to SolidColorBrush for background
        public SolidColorBrush GetBackgroundBrush() {
            try {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(BackgroundColor));
            } catch {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2D2D2D")); // Fallback to dark gray
            }
        }

        // Set color from SolidColorBrush
        public void SetFontColor(SolidColorBrush brush) {
            FontColor = brush.Color.ToString();
        }

        // Set background color from SolidColorBrush
        public void SetBackgroundColor(SolidColorBrush brush) {
            BackgroundColor = brush.Color.ToString();
        }
    }
}