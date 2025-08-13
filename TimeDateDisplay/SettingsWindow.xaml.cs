using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace TimeDateDisplay {
    public partial class SettingsWindow : Window {
        public Settings Settings { get; private set; }
        bool _isInitialized = false;
        public SettingsWindow(Settings currentSettings) {
            Settings = new Settings {
                Opacity = currentSettings.Opacity,
                AlwaysOnTop = currentSettings.AlwaysOnTop,
                FontColor = currentSettings.FontColor,
                BackgroundColor = currentSettings.BackgroundColor,
                FontTimeFamilyName = currentSettings.FontTimeFamilyName,
                FontDateFamilyName = currentSettings.FontDateFamilyName
            };

            this.DataContext = Settings;

            InitializeComponent();
            this.Loaded += (o, e) => LoadSettings();

            FontTimeComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontDateComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            _isInitialized = true;

            LoadSettings();
        }

        private void LoadSettings() {
            OpacitySlider.Value = Settings.Opacity;
            AlwaysOnTopCheckBox.IsChecked = Settings.AlwaysOnTop;
            FontColorTextBox.Text = Settings.FontColor;
            BackgroundColorTextBox.Text = Settings.BackgroundColor;

            FontDateComboBox.SelectedValue = Settings.FontDateFamilyName;
            FontTimeComboBox.SelectedValue = Settings.FontTimeFamilyName;

            // Set color picker values
            try {
                FontColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.FontColor);
                BackgroundColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.BackgroundColor);
            } catch { }

            UpdateOpacityDisplay();
            UpdateFontColorPreview();
            UpdateBackgroundColorPreview();
            UpdatePreview();
        }

        private void FontTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!_isInitialized) return;

            if (FontTimeComboBox.SelectedValue is string selectedFont && selectedFont is not null) {
                Settings.FontTimeFamilyName = selectedFont;
                UpdatePreview();
            }
        }
        private void FontDateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!_isInitialized) return;

            if (FontDateComboBox.SelectedValue is string selectedFont) {
                Settings.FontDateFamilyName = selectedFont;
                UpdatePreview();
            }
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedEventArgs e) {
            if (OpacitySlider != null && OpacityValue != null) {
                Settings.Opacity = OpacitySlider.Value;
                UpdateOpacityDisplay();
                UpdatePreview();
            }
        }

        private void UpdateOpacityDisplay() {
            if (OpacityValue != null) {
                OpacityValue.Text = $"{(int)(Settings.Opacity * 100)}%";
            }
        }

        private void FontColorTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (!_isInitialized) return;

            Settings.FontColor = FontColorTextBox.Text;
            UpdateFontColorPreview();
            UpdatePreview();
        }

        private void BackgroundColorTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (!_isInitialized) return;

            Settings.BackgroundColor = BackgroundColorTextBox.Text;
            UpdateBackgroundColorPreview();
            UpdatePreview();
        }

        private void UpdateFontColorPreview() {
            if (!_isInitialized) return;

            try {
                var color = (Color)ColorConverter.ConvertFromString(Settings.FontColor);
                FontColorPreview.Fill = new SolidColorBrush(color);
            } catch {
                FontColorPreview.Fill = new SolidColorBrush(Colors.White);
            }
        }

        private void UpdateBackgroundColorPreview() {
            if (!_isInitialized) return;

            try {
                var color = (Color)ColorConverter.ConvertFromString(Settings.BackgroundColor);
                BackgroundColorPreview.Fill = new SolidColorBrush(color);
            } catch {
                BackgroundColorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2D2D2D"));
            }
        }

        private void UpdatePreview() {
            if (!_isInitialized) return;

            var fontBrush = Settings.GetFontBrush();
            var backgroundBrush = Settings.GetBackgroundBrush();

            PreviewTime.Foreground = fontBrush;
            PreviewDate.Foreground = fontBrush;
            PreviewBorder.Background = backgroundBrush;

            PreviewTime.FontFamily = Settings.GetTimeFontFamily();
            PreviewDate.FontFamily = Settings.GetDateFontFamily(); 
            
            // Update preview with current time
            PreviewTime.Text = DateTime.Now.ToString("HH:mm:ss");
            PreviewDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        }

        private void FontColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (FontColorPicker.SelectedColor.HasValue) {
                Settings.FontColor = FontColorPicker.SelectedColor.Value.ToString();
                FontColorTextBox.Text = Settings.FontColor;
                UpdateFontColorPreview();
                UpdatePreview();
            }
        }

        private void BackgroundColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (BackgroundColorPicker.SelectedColor.HasValue) {
                Settings.BackgroundColor = BackgroundColorPicker.SelectedColor.Value.ToString();
                BackgroundColorTextBox.Text = Settings.BackgroundColor;
                UpdateBackgroundColorPreview();
                UpdatePreview();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            // Update settings from controls
            Settings.Opacity = OpacitySlider.Value;
            Settings.AlwaysOnTop = AlwaysOnTopCheckBox.IsChecked ?? false;
            Settings.FontColor = FontColorTextBox.Text;
            Settings.BackgroundColor = BackgroundColorTextBox.Text;
            Settings.FontTimeFamilyName = FontTimeComboBox.SelectedValue as string;
            Settings.FontDateFamilyName = FontDateComboBox.SelectedValue as string;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
        }
    }
}