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

namespace TakeAPicture.Controles
{
    /// <summary>
    /// Interaction logic for IraHomeIralAnterior.xaml
    /// </summary>
    public partial class IraHomeIralAnterior : UserControl
    {
        public IraHomeIralAnterior()
        {
            InitializeComponent();
        }
        public Window Padre
        {
            get;
            set;
        }
        public bool MostrarBotonAnterior
        {

            get
            {

                return (BtnAnterior.Visibility == System.Windows.Visibility.Visible);
            }
            set
            {
                if (value)
                {
                    BtnAnterior.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    BtnAnterior.Visibility = System.Windows.Visibility.Hidden;

                }

            }

        }
        public bool MostrarBotonFace
        {

            get
            {

                return (BtnFace.Visibility == System.Windows.Visibility.Visible);
            }
            set
            {
                if (value)
                {
                    BtnFace.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    BtnFace.Visibility = System.Windows.Visibility.Hidden;

                }

            }

        }

        public bool MostrarBotonHouse
        {

            get
            {

                return (BtnCasa.Visibility == System.Windows.Visibility.Visible);
            }
            set
            {
                if (value)
                {
                    BtnCasa.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    BtnCasa.Visibility = System.Windows.Visibility.Hidden;

                }

            }

        }
        public event EventHandler AnteriroClick;
        public event EventHandler SiguienteClick;
        public event EventHandler FaceClick;

        public int tipo
        {
            get;
            set;
        }


        public UserControl Control
        {
            get;
            set;
        }

        public bool soloclick
        {
            get;
            set;
        }
        public bool SinSonido
        {
            get;
            set;
        }

        public bool Manual
        {
            get;
            set;
        }
        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (AnteriroClick != null)
            {
                AnteriroClick(sender, e);
                if (soloclick)
                {
                    return;
                }

            }
          


        }

        private void BtnCasa_Click(object sender, RoutedEventArgs e)
        {
            if (SiguienteClick != null)
            {
                SiguienteClick(sender, e);
                if (soloclick)
                {
                    return;
                }
            }
         //   ClaseDeManejoDePantallas.IrACasa(Padre.Pagina);

        }

        private void BtnAnterior_TouchDown(object sender, TouchEventArgs e)
        {
            if (AnteriroClick != null)
            {
                AnteriroClick(sender, e);
                if (soloclick)
                {
                    return;
                }
            }
          //  ClaseDeManejoDePantallas.IrAnterior(Padre.Pagina);

        }

        private void BtnCasa_TouchDown(object sender, TouchEventArgs e)
        {
            if (SiguienteClick != null)
            {
                SiguienteClick(sender, e);
                if (soloclick)
                {
                    return;
                }
            }
          //  ClaseDeManejoDePantallas.IrACasa(Padre.Pagina);

        }

        private void BtnFace_Click(object sender, RoutedEventArgs e)
        {
            if (FaceClick != null)
            {
                FaceClick(sender, e);
                
            }
        }
    }
}
