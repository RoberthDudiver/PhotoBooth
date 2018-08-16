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
using System.IO;
namespace TakeAPicture.Controles
{
    /// <summary>
    /// Interaction logic for EfectosDeFotos.xaml
    /// </summary>
    public partial class EfectosDeFotos : UserControl
    {
        List<System.Drawing.Bitmap> Listaimagenes;
        public List<System.Drawing.Bitmap> ListaImagenes
        {
            get
            {

                if (Listaimagenes == null)
                {
                    Listaimagenes = new List<System.Drawing.Bitmap>();
                }
                return Listaimagenes;
            }
            set { Listaimagenes = value; }
        }

        public List<string> Efectos
        {
            get;
            set;
        }
        public Window Padre
        {
            get;
            set;
        }
        public EfectosDeFotos()
        {
            InitializeComponent();

            this.Width = Utilidades.Ancho;
            this.Height = Utilidades.Alto;
            double r_ancho = ((Utilidades.Ancho * 50) / 100);
            double alto = (Height / Width) * r_ancho;
            double PorcentajeReal = ((Utilidades.Ancho * 70) / 100);

            double PosiAlto = ((Utilidades.Alto * 70) / 100);

            galeria.Margin = new Thickness(50, Utilidades.Alto - PosiAlto, 0, 0);
            double alturabotonNanterior = (1.1458 * Utilidades.Alto) / 100;
            double ANCHUrabotonNanterior = (89 * Utilidades.Ancho) / 100;
            double wanterior = (20 * Utilidades.Ancho) / 100;
            double hanterior = (13.79 * Utilidades.Alto) / 100;
            iraHomeIralAnterior1.Width = wanterior;
            iraHomeIralAnterior1.Height = hanterior;
            iraHomeIralAnterior1.Margin = new Thickness(ANCHUrabotonNanterior, alturabotonNanterior, 0, 0);
            double r_ancho2 = ((Utilidades.Ancho * 30) / 100);
            Imagen.Width = r_ancho2;
            Imagen.Height = (this.Height - galeria.Height - 10);
            Imagen.Margin = new Thickness(0, 20, 0, 0);

        }
        public ControlCamara Anterior
        {
            get;
            set;
        }
        private void iraHomeIralAnterior1_AnteriroClick(object sender, EventArgs e)
        {

            Utilidades.CambiarVentana(PageTransitionType.SlideAndFade, Anterior, this.PaginaPrincipal);

        }
        public PageTransition PaginaPrincipal
        {
            get;
            set;
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            galeria.Width = this.ActualWidth;
            galeria.Height = this.ActualHeight;
            galeria.Cargar();

            // Efectos = new List<string>();
            // Efectos.Add(Utilidades.PathEfectos + @"\E1.jpg");
            // Efectos.Add(Utilidades.PathEfectos + @"\E2.jpg");
            // Efectos.Add(Utilidades.PathEfectos + @"\E3.jpg");
            //foreach (string ef in Efectos)
            //{
            //    galeria.Agregar(ef);
            //}

            foreach (RADMLIB.Lienzo Li in Utilidades.Efectos)
            {
                galeria.Agregar(Utilidades.PathEfectos + @"\" + Li.NombreArchivo + ".png");
            }

        }

        string fotoactual;
        private void galeria_CambiodeSeleccion(object sender, FluidKit.Controls.ElementFlow Elemento)
        {
            string nombre = System.IO.Path.GetFileNameWithoutExtension(Elemento.SelectedValue.ToString());
            var efecto = Utilidades.Efectos.SingleOrDefault(x => x.NombreArchivo == nombre);
            if (efecto != null)
            {
              Imagen.Source=  Utilidades.ProcesarEfecto(efecto,Listaimagenes);
              fotoactual = efecto.NombreArchivo;
                if(!iraHomeIralAnterior1.MostrarBotonFace )
                {
              iraHomeIralAnterior1.MostrarBotonFace = true;
                }
            }

        }

        private void iraHomeIralAnterior1_FaceClick(object sender, EventArgs e)
        {
            FacebookMain FC = new FacebookMain();
            FC.Fotostr = fotoactual;
            Utilidades.CambiarVentana(PageTransitionType.SlideAndFade, FC, this.PaginaPrincipal);

        }
    }
}
