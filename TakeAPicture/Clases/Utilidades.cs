using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfPageTransitions;
using System.IO;
using System.Windows.Media.Animation;
using System.Printing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace TakeAPicture
{
    public  class Utilidades
    {
        public static double Ancho
        {
            get
            {

                return System.Windows.SystemParameters.PrimaryScreenWidth;
            }
        }

        public static UserControl ListaControles
        {
            get;
            set;
        }

        public static bool validado
        {
            get;
            set;
        }
        public static double Alto
        {
            get
            {

                return System.Windows.SystemParameters.PrimaryScreenHeight;
            }
        }

        public static List<RADMLIB.Lienzo> Efectos
        {
            get;
            set;
        }
       static RADMLIB.TrabajoDeObjetos Trabajo = new RADMLIB.TrabajoDeObjetos();
     public  static BitmapSource Convertir(System.Drawing.Bitmap bitmap)
       {
           

           BitmapData bmpData = bitmap.LockBits(
               new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
               ImageLockMode.ReadOnly, bitmap.PixelFormat);         

           bitmap.UnlockBits(bmpData);        
                           

           IntPtr hBitmap = bitmap.GetHbitmap();

           BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
               hBitmap,
               IntPtr.Zero,
               Int32Rect.Empty,
               BitmapSizeOptions.FromEmptyOptions());          

           return bitmapSource;
       }


     public static void SaveJpeg(string path, System.Drawing.Image img, int quality)
     {
         if (quality < 0 || quality > 100)
             throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


         // Encoder parameter for image quality 
         EncoderParameter qualityParam =
             new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
         // Jpeg image codec 
         ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

         EncoderParameters encoderParams = new EncoderParameters(1);
         encoderParams.Param[0] = qualityParam;

         img.Save(path, jpegCodec, encoderParams);
     }

     /// <summary> 
     /// Returns the image codec with the given mime type 
     /// </summary> 
     private static ImageCodecInfo GetEncoderInfo(string mimeType)
     {
         // Get image codecs for all image formats 
         ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

         // Find the correct image codec 
         for (int i = 0; i < codecs.Length; i++)
             if (codecs[i].MimeType == mimeType)
                 return codecs[i];
         return null;
     }  
     public static BitmapSource ProcesarEfecto(RADMLIB.Lienzo Lienzo, List<System.Drawing.Bitmap> Lista)
       {
           System.Drawing.Bitmap Bac = (System.Drawing.Bitmap)Lienzo.Fondo.Imagen;
           System.Drawing.Graphics r = System.Drawing.Graphics.FromImage(Bac);
           if( Lienzo.Banner!=null)
           {
               r.DrawImage((System.Drawing.Bitmap)Lienzo.Banner.Imagen, Lienzo.Banner.PosicionDeItems.X, Lienzo.Banner.PosicionDeItems.Y, Lienzo.Banner.Tamano.Width, Lienzo.Banner.Tamano.Height);
           }
           if (Lienzo.Fotos != null)
           {
               if (Lienzo.Fotos.Count > 0)
               {
                   int i=0;
                   foreach (RADMLIB.Items item in Lienzo.Fotos)
                   {

                       System.Drawing.Bitmap img = (System.Drawing.Bitmap)Lista[i];
                       r.DrawImage(img, item.PosicionDeItems.X, item.PosicionDeItems.Y, item.Tamano.Width, item.Tamano.Height);
                       i++;
                   }
               }
           }

           SaveJpeg(Utilidades.PathInicial + @"\Efecto_" + Lienzo.NombreArchivo + ".jpg", Bac, 50);
           return Convertir(Bac);
       }
      static  public void CagarEfectos()
        {
            DirectoryInfo Dire = new DirectoryInfo(Utilidades.PathEfectos);
            Efectos = new List<RADMLIB.Lienzo>();
            List<FileInfo> Lista = Dire.GetFiles().Cast<FileInfo>().ToList();
            var listaefecto = Lista.Where(x => x.Extension.ToString() == ".sitm").ToList();

            foreach (FileInfo info in listaefecto)
            {
                FileInfo Infon = new FileInfo(Utilidades.PathEfectos + @"\" + info.Name.Replace(info.Extension, "")+".png");
                RADMLIB.Lienzo un= Trabajo.DeSerializeObject<RADMLIB.Lienzo>(info.FullName);
                if (!Infon.Exists)
                {
                    System.Drawing.Image Img = (System.Drawing.Bitmap)un.ImagenBoton;
                    Img.Save(Infon.FullName);
                }
                Efectos.Add(un);
            }
         
        }
        public static string PathRun
        {
            get
            {
                return Directory.GetCurrentDirectory();

            }
        }

        public static int IdBits
        {
            get;
            set;
        }

        public static string PathInicialSonidos
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\Sonidos";

            }
        }
        public static string PathInicial
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\Imagenes";

            }
        }

        public static string PathEfectos
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\Efectos";

            }
        }
        public static string PathInicialAvatares
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\Avatares\";

            }
        }
        public enum Idioma
        {
            Español
            , Ingles
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static ImageSource ImagenUri(string URI)
        {

            try
            {
                BitmapImage empty1 = new BitmapImage();
                empty1.BeginInit();
                empty1.UriSource = new Uri(URI, UriKind.RelativeOrAbsolute);
                empty1.EndInit();


                return empty1;
            }
            catch
            {

                return null;
            }


        }

        public DoubleAnimation Flip(double degrees, UIElement svi, DependencyProperty OBNG, bool crearmetodo)
        {
            DoubleAnimation OrientationAnimation = new DoubleAnimation();

            OrientationAnimation.To = degrees;
            OrientationAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0,30));
            OrientationAnimation.AccelerationRatio = 0.1;
            OrientationAnimation.DecelerationRatio = 0.1;
            OrientationAnimation.FillBehavior = FillBehavior.Stop;

            if (!crearmetodo)
            {
                OrientationAnimation.Completed += delegate(object sender, EventArgs e)
                {

                    if (OBNG.Name == "Opacity")
                    {
                        svi.Opacity = degrees;
                    }

                };
            }
            else
            {
                OrientationAnimation.Completed += delegate(object sender, EventArgs e)
                {

                    if (OBNG.Name == "Opacity")
                    {
                        svi.Opacity = degrees;
                    }

                    if (Completa != null)
                    {
                        Completa(sender, e);
                    }

                };
            }


            return OrientationAnimation;

        }
       
        public static event  EventHandler Completa;
        public static DoubleAnimation Flip(double degrees, UserControl svi, DependencyProperty OBNG, bool crearmetodo)
        {
            DoubleAnimation OrientationAnimation = new DoubleAnimation();

            OrientationAnimation.To = degrees;
            OrientationAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0,600));
            OrientationAnimation.AccelerationRatio = 0.5;
            OrientationAnimation.DecelerationRatio = 0.5;
            OrientationAnimation.FillBehavior = FillBehavior.Stop;

            if (!crearmetodo)
            {
                OrientationAnimation.Completed += delegate(object sender, EventArgs e)
                {

                    if (OBNG.Name == "Opacity")
                    {
                        svi.Opacity = degrees;
                    }

                };
            }
            else
            {
                OrientationAnimation.Completed += delegate(object sender, EventArgs e)
                {

                    if (OBNG.Name == "Opacity")
                    {
                        svi.Opacity = degrees;
                    }

                    if (Completa != null)
                    {
                        Completa(sender, e);
                    }

                };
            }


            return OrientationAnimation;

        }


        public static void Flip(string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, Window PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(1);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }
        public static void Flip(double degrees, string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, Window PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(degrees);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(degrees))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }
        public static void Flip(string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, Page PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(1);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }
        public static void Flip(double degrees, string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, Page PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(degrees);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(degrees))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }
        public static void Flip(string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, UserControl PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(1);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }
        public static void Flip(double degrees, string Nombre, UIElement svi, double valor, ref double intPreviousValue, bool XY, UserControl PS)
        {
            try
            {
                double intGridColumn = valor;
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                svi.RenderTransform = animatedTranslateTransform;
                PS.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

                DoubleAnimationUsingKeyFrames translationAnimation = new DoubleAnimationUsingKeyFrames();
                translationAnimation.Duration = TimeSpan.FromSeconds(degrees);
                //translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intPreviousValue, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(.4))));
                translationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(intGridColumn, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(degrees))));


                //DoubleAnimation dblaHeight = new DoubleAnimation();
                //dblaHeight.BeginTime = TimeSpan.FromSeconds(.2);
                //dblaHeight.Duration = TimeSpan.FromSeconds(.1);
                //dblaHeight.To = 48;

                intPreviousValue = intGridColumn;

                Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
                if (XY)
                {
                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.XProperty));
                }
                else
                {

                    Storyboard.SetTargetProperty(translationAnimation, new PropertyPath(TranslateTransform.YProperty));

                }


                Storyboard strbStoryboard = new Storyboard();
                strbStoryboard.Children.Add(translationAnimation);
                strbStoryboard.Completed += delegate(object sender, EventArgs e)
                {
                    strbStoryboard.Stop();


                };
                strbStoryboard.Begin(PS);
                PS.UnregisterName("AnimatedTranslateTransform");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                PS.UnregisterName("AnimatedTranslateTransform");

            }

        }




        public static bool Verificardetalle(string Carpeta, string photoPuzzlesPath)
        {

            string file = photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta) + @"\DetalleAvanzado";
            return Directory.Exists(file);


        }

        public static string DirectorioActual
        {
            get
            {

                return Directory.GetCurrentDirectory();
            }
        }

        public static void CambiarVentana(WpfPageTransitions.PageTransitionType Tipo, UserControl newPage, PageTransition pageTransitionControl)
        {

            pageTransitionControl.TransitionType = Tipo;
            pageTransitionControl.ShowPage(newPage);
        }
        public static void FadeAnimation(UIElement svi)
        {
            var ObjAnimation = new DoubleAnimation(0,(Duration)TimeSpan.FromSeconds(0.5));
         //   ObjAnimation.Completed +=ObjAnimation_Completed;
        svi.BeginAnimation(UIElement.OpacityProperty, ObjAnimation);

        }

        public static void FadeAnimation(UserControl svi)
        {
            var ObjAnimation = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.5));
            //   ObjAnimation.Completed +=ObjAnimation_Completed;
            svi.BeginAnimation(UIElement.OpacityProperty, ObjAnimation);

        }
        public static string DirectorioActualInterfaz
        {
            get
            {

                return Directory.GetCurrentDirectory() + @"\Interfaz"; ;
            }
        }
        public static DateTime FechaActualizacionBD
        {
            get;
            set;
        }


        static void EliminarArchivos(string ruta)
        {
            DirectoryInfo Direc = new DirectoryInfo(ruta);
            if (Direc.Exists)
            {
                foreach (FileInfo fi in Direc.GetFiles())
                {
                    if (fi.Extension.ToLower() == ".jpg" || fi.Extension.ToLower() == ".png")
                    {
                        if (fi.Name.IndexOf("Back") == -1 && fi.Name.IndexOf("Thumbn") == -1 && fi.Name.IndexOf("IMG") == -1)
                        {
                            fi.Delete();
                        }
                    }
                }
            }
        }
  
       

        public static void Verificarfile(string file)
        {

            FileInfo DI = new FileInfo(file);

            if (DI.Exists)
            {
                DI.Delete();
            }
        }
        public static string DirectorioActualImagenes
        {
            get
            {

                return Directory.GetCurrentDirectory() + @"\Imagenes"; ;
            }
        }
        public static string ImagenBack(string Carpeta, string photoPuzzlesPath)
        {
            try
            {
                return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), "back.jpg")[0];
            }
            catch
            {
                //   return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), @"\Back.jpg")[0];
                return null;
            }
        }

        public static string ImagenPrincipal(string Carpeta, string photoPuzzlesPath)
        {
            try
            {
                return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), "IMGPrincipal.jpg")[0];
            }
            catch
            {
                //   return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), @"\Back.jpg")[0];
                return null;
            }
        }


        public static string ImagenSegundaria(string Carpeta, string photoPuzzlesPath)
        {
            try
            {
                return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), "IMGSegundaria.jpg")[0];
            }
            catch
            {
                //   return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), @"\Back.jpg")[0];
                return null;
            }
        }
        public static string ImagenThumbn(string Carpeta, string photoPuzzlesPath)
        {
            try
            {
                return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), "Thumbn.jpg")[0];
            }
            catch
            {
                //return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), @"\Thumbn.jpg")[0];
                return null;

            }
        }
        public static string Info(string Carpeta, string photoPuzzlesPath)
        {
            try
            {
                return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), "info.xml")[0];
            }
            catch
            {
                //  return Directory.GetFiles(photoPuzzlesPath + (Carpeta.IndexOf(@"\") == -1 ? @"\" + Carpeta : Carpeta), @"\info.xml")[0];
                return null;

            }
        }

        public static void Imprimir(UIElement Elemento, string descripcion)
        {

            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() != true)
            { return; }
            Canvas Can = new Canvas();
            Can.Children.Add(Elemento);
            dialog.PrintVisual(Can, descripcion);
        }
    }
}
