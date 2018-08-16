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


namespace TakeAPicture.Controles
{
    /// <summary>
    /// Interaction logic for ControlCamara.xaml
    /// </summary>
    public partial class ControlCamara : UserControl
    {
        public ControlCamara()
        {
            InitializeComponent();
            this.Width = Utilidades.Ancho;
            this.Height = Utilidades.Alto;
            double r_ancho = ((Utilidades.Ancho * 50) / 100);
            double alto = (Height / Width) * r_ancho;
            double PorcentajeReal = ((Utilidades.Ancho * 70) / 100);

            double PosiAlto = ((Utilidades.Alto * 8) / 100);

            stackPanel1.Margin = new Thickness(0, Utilidades.Alto - PosiAlto, 0, 0);

            double spk = ((Utilidades.Ancho * 90) / 100);
            stackPanel1.Height = (stackPanel1.Height / stackPanel1.Width) * spk;
            stackPanel1.Width = spk;
            double Porcentajewr = ((PorcentajeReal * 25) / 100) - (5);
            Wrp.Width = PorcentajeReal + 30;
            //COntrol.Margin = new Thickness(0, Utilirdades.Alto - PosiAlto, 0, 0);
            COntrol.Height = alto;
            COntrol.Width = r_ancho;
            P1.Width = Porcentajewr;
            P2.Width = Porcentajewr;
            P3.Width = Porcentajewr;
            P4.Width = Porcentajewr;
            double PosiAlto2 = ((Utilidades.Alto * 25) / 100);
            wpr2.Width = r_ancho;
            
            wpr2.Margin = new Thickness(0, Utilidades.Alto - PosiAlto2, 0, 0);
            iniciarcamara();
            conta.Opacity = 0;
            Utilidades.Completa += Utilidades_Completa;
            Controles.ImagenControl Imagen = new Controles.ImagenControl();
        }
        public void iniciarcamara()
        {
            this.cameraCapture.SelectedDevice = (DirectShowDevice.AvailableDevices.ToArray()[0] as DirectShowLib.DsDevice).Name;

        }

        TipoDeFoto FotoActual
        {
            get;
            set;
        }

        bool pasa;
        bool Nuevo;
        void Utilidades_Completa(object sender, EventArgs e)
        {

            if (Nuevo)
            {
                if (!pasa)
                {
                    conta.BeginAnimation(OpacityProperty, Utilidades.Flip(0, conta, OpacityProperty, true));
                    pasa = true;

                }
                else
                {
                    Segundo++;

                    if (Segundo < 4)
                    {
                        playsound(Utilidades.PathInicialSonidos + @"\beep-07.mp3");
                        conta.Texto = Segundo.ToString();
                        txtbtn.Text = Segundo.ToString();

                        conta.BeginAnimation(OpacityProperty, Utilidades.Flip(1, conta, OpacityProperty, true));
                        pasa = false;

                    }
                    else
                    {

                        if (Segundo == 4)
                        {

                            TomarFoto();

                            Segundo = 1;
                        }
                    }


                }
            }
        }
        MediaPlayer player;
        void playsound(string sonido)
        {

            {
                player = new MediaPlayer();
                string ss = sonido;
                System.IO.FileInfo fs = new System.IO.FileInfo(ss);

                player.Open(new Uri(ss, UriKind.RelativeOrAbsolute));
                player.Play();

                player.MediaEnded += player_MediaEnded;

            }

        }
        EfectosDeFotos FrmEfectos
        {
            get;
            set;
        }
        void player_MediaEnded(object sender, EventArgs e)
        {

            if (srt != "" && srt.IndexOf("gracias") == -1)
            {
                srt = "";

                INiciarConteo();
            }
            else
            {
                if (srt.IndexOf("gracias") > -1)
                {
                    if (FrmEfectos == null)
                    {
                        FrmEfectos = new EfectosDeFotos() { ListaImagenes = Listaimagenes, PaginaPrincipal = PaginaPrincipal, Padre = Padre, Anterior = this };
                    }
                    Utilidades.CambiarVentana(PageTransitionType.SlideAndFade, FrmEfectos, PaginaPrincipal);
                    srt = "";
                    //  cameraCapture.cerrar();


                }
            }
        }

        Controles.ImagenControl Imagen;

     
        void TomarFoto()
        {
            playsound(Utilidades.PathInicialSonidos + @"\camera1.wav");

            pageTransitionControl.CurrentPage = null;
            var esta = cameraCapture.GetCapturedBitmap(FotoActual);
            var img = cameraCapture.UltimoBitmap;
            ListaImagenes.Add(img);
            Utilidades.SaveJpeg(Utilidades.PathInicial + @"\demo" + Guid.NewGuid() + ".jpg", img, 100);

            cameraCapture.GuardarImagen("Demo" + Guid.NewGuid());
            Nuevo = false;
            pageTransitionControl.BeginAnimation(OpacityProperty, Utilidades.Flip(1, pageTransitionControl, OpacityProperty, true));

            Utilidades.CambiarVentana(PageTransitionType.Fade, Imagen, pageTransitionControl);


            IniciarDispache();

        }
        DispatcherTimer dispatcherTimer;
        int Segundo;
        int tipo;
        string srt = "";
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            tipo++;
            if (tipo >= 3)
            {

                cameraCapture.Opacity = 1;

                pageTransitionControl.BeginAnimation(OpacityProperty, Utilidades.Flip(0, pageTransitionControl, OpacityProperty, true));
                tipo = 0;
                dispatcherTimer.Stop();
                btn.IsEnabled = true;


                if (Listaimagenes.Count <= 4)
                {
                    srt = "";
                    switch (Listaimagenes.Count)
                    {
                        case 1:
                            srt = @"\hayvamos.mp3";
                            Utilidades.CambiarVentana(PageTransitionType.Slide, new Controles.ImagenControl()
                            {
                                ImagenS = Utilidades.Convertir( Listaimagenes[0])
                            }, P1);
                            break;
                        case 2:
                            srt = @"\vaotra.mp3";
                            Utilidades.CambiarVentana(PageTransitionType.Slide, new Controles.ImagenControl()
                            {
                                ImagenS =Utilidades.Convertir(  Listaimagenes[1])
                            }, P2);
                            break;

                        case 3:
                            srt = @"\Solofalta.mp3";
                            Utilidades.CambiarVentana(PageTransitionType.Slide, new Controles.ImagenControl()
                            {
                                ImagenS =Utilidades.Convertir( Listaimagenes[2])
                            }, P3);
                            break;

                        case 4:
                            srt = @"\gracias.mp3";
                            Utilidades.CambiarVentana(PageTransitionType.Slide, new Controles.ImagenControl()
                            {
                                ImagenS = Utilidades.Convertir(Listaimagenes[3])
                            }, P4);
                       
                            txtbtn.Text = "";

                            break;




                    }
                    playsound(Utilidades.PathInicialSonidos + srt);

                }
            }


        }
        public PageTransition PaginaPrincipal
        {
            get;
            set;
        }
        List<System.Drawing.Bitmap> Listaimagenes;
        List<System.Drawing.Bitmap> ListaImagenes
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

        public Window Padre
        {
            get;
            set;
        }
        void IniciarDispache()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void INiciarConteo()
        {
            Imagen = new Controles.ImagenControl();
            Segundo = 1;
            Nuevo = true;
            pasa = false;
            tipo = 0;
            conta.Texto = Segundo.ToString();
            txtbtn.Text = Segundo.ToString();
            conta.BeginAnimation(OpacityProperty, Utilidades.Flip(1, conta, OpacityProperty, true));
            playsound(Utilidades.PathInicialSonidos + @"\beep-07.mp3");
            btn.IsEnabled = false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Listaimagenes.Clear(); 

            FotoActual = TipoDeFoto.Normal;
            INiciarConteo();


        }

        private void Contador_Loaded_1(object sender, RoutedEventArgs e)
        {
            ListaImagenes.Clear();

        }

        private void btnw_Click_1(object sender, RoutedEventArgs e)
        {
            this.Padre.Close();
        }

        private void btnv_Click_1(object sender, RoutedEventArgs e)
        {
            Listaimagenes.Clear(); 

            FotoActual = TipoDeFoto.Vieja;
            INiciarConteo();
        }

        private void btnc_Click_1(object sender, RoutedEventArgs e)
        {
            Listaimagenes.Clear(); 

            FotoActual = TipoDeFoto.Comic;
            INiciarConteo();
        }

    }
}
