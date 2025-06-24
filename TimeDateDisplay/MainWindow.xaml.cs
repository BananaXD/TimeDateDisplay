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

namespace TimeDateDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private bool _isAlwaysOnTop = false;

        public MainWindow() {
            InitializeComponent();
            StartClock();
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

        private void AlwaysOnTop_Click(object sender, RoutedEventArgs e) {
            _isAlwaysOnTop = !_isAlwaysOnTop;
            Topmost = _isAlwaysOnTop;
            MessageBox.Show($"Always On Top: {(_isAlwaysOnTop ? "Enabled" : "Disabled")}",
                            "Always On Top",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                DragMove();
            }
        }
    }
}