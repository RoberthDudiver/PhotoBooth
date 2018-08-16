using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPageTransitions;
using System.Runtime.InteropServices;
using WpfCamera;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.IO;
namespace TakeAPicture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            Utilidades.CagarEfectos();

            WindowBackground.ImageSource = Utilidades.ImagenUri(Utilidades.PathEfectos + @"\Redblind.jpg");
            this.Topmost = true;
        }

        public PageTransition Pagina
        {
            get
            {
                return pageTransitionControl;
            }
        }

        private void pageTransitionControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            Controles.ControlCamara Camara = new Controles.ControlCamara() { PaginaPrincipal = Pagina };
            Camara.Padre = this;
            Utilidades.CambiarVentana(PageTransitionType.SlideAndFade, Camara, this.Pagina);
            Pagina.Height = Utilidades.Alto;
            Pagina.Width = Utilidades.Ancho;

            
        }

    }
}
