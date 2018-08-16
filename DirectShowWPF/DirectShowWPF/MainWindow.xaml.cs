using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCamera;

namespace DirectShowWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.cameraCapture.SelectedDevice = (DirectShowDevice.AvailableDevices.ToArray()[0] as DirectShowLib.DsDevice).Name;
          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            imagen.Source = cameraCapture.GetCapturedBitmap();
        }
    }
}