using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeDateDisplay {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Settings _settings;

        public MainWindow() {
            InitializeComponent();

            // Load settings
            _settings = SettingsManager.LoadSettings();
            ApplySettings();

            StartClock();
        }

        private void ApplySettings() {
            // Apply opacity
            Opacity = _settings.Opacity;

            // Apply always on top
            Topmost = _settings.AlwaysOnTop;

            // Apply font color
            var fontBrush = _settings.GetFontBrush();
            TimeText.Foreground = fontBrush;
            DateText.Foreground = fontBrush;

            // Apply font family
            TimeText.FontFamily = _settings.GetTimeFontFamily();
            DateText.FontFamily = _settings.GetDateFontFamily();


            // Apply background color
            var backgroundBrush = _settings.GetBackgroundBrush();
            // Find the main border and update its background
            if (this.Content is Grid mainGrid) {
                if (mainGrid.Children[0] is Border mainBorder) {
                    mainBorder.Background = backgroundBrush;
                }
            }
        }

        private void SaveSettings() {
            SettingsManager.SaveSettings(_settings);
        }

        private void StartClock() {
            DispatcherTimer timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => {
                TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
                DateText.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            };
            timer.Start();
        }

        private void Settings_Click(object sender, RoutedEventArgs e) {
            var settingsWindow = new SettingsWindow(_settings) {
                Owner = this
            };

            if (settingsWindow.ShowDialog() == true) {
                // Update current settings
                _settings = settingsWindow.Settings;
                ApplySettings();
                SaveSettings();
            }
        }

        private void AlwaysOnTop_Click(object sender, RoutedEventArgs e) {
            _settings.AlwaysOnTop = !_settings.AlwaysOnTop;
            Topmost = _settings.AlwaysOnTop;
            SaveSettings();
        }

        private void SetOpacity_Click(object sender, RoutedEventArgs e) {
            if (sender is MenuItem menuItem && menuItem.Tag is string tag) {
                if (double.TryParse(tag, out double opacity)) {
                    _settings.Opacity = opacity;
                    Opacity = opacity;
                    SaveSettings();
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            SaveSettings();
            Application.Current.Shutdown();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                DragMove();
            }
        }

        protected override void OnClosed(EventArgs e) {
            SaveSettings();
            base.OnClosed(e);
        }
    }
}