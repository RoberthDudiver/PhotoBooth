using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Reflection;
namespace WpfCamera
{
  public   class Efectos
    {

      public static  Image ProcesarCartoon(Bitmap selectedSource)
        {
            string ruta=AssemblyDirectory;

            Bitmap bitmapResult = null;

            ExtBitmap.SmoothingFilterType filterType =
                    ExtBitmap.SmoothingFilterType.Gaussian3x3;


            bitmapResult = selectedSource.CartoonEffectFilter(
                               60, filterType);



            FileStream fs = new FileStream(ruta + @"\Efectos\T0.png", FileMode.Open, FileAccess.Read);

            var tama = new Size(selectedSource.Width, selectedSource.Height);
            System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(fs);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bitmap1, tama);

            FileStream fss = new FileStream(ruta + @"\Efectos\T1.png", FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap bitmaps = new System.Drawing.Bitmap(fss);


            FileStream fsss = new FileStream(ruta + @"\Efectos\T2.png", FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap fondo = new System.Drawing.Bitmap(fsss);



            FileStream fssss = new FileStream(ruta + @"\Efectos\T3.png", FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap principal = new System.Drawing.Bitmap(fssss);

            System.Drawing.Graphics r = System.Drawing.Graphics.FromImage(bitmap);

            r.DrawImage(bitmapResult, 0, 0, selectedSource.Width, selectedSource.Height);
            r.DrawImage(bitmaps, 0, 0, selectedSource.Width, selectedSource.Height);

            r.DrawImage(fondo, 0, 0, selectedSource.Width, selectedSource.Height);
            r.DrawImage(principal, 0, 0, selectedSource.Width, selectedSource.Height);


            fondo.Dispose();
            bitmaps.Dispose();




            return bitmap;
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
       public static Image ProcesarImagenVieja(Image Imagen)
        {
            string ruta=AssemblyDirectory;
            FileStream fs = new FileStream(ruta + @"\Efectos\2.png", FileMode.Open, FileAccess.Read);
            var tama = new Size(Imagen.Width, Imagen.Height);
            System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(fs);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bitmap1, tama);



            FileStream fss = new FileStream(ruta + @"\Efectos\0.png", FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap bitmaps = new System.Drawing.Bitmap(fss);


            FileStream fsss = new FileStream(ruta + @"\Efectos\1.png", FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap fondo = new System.Drawing.Bitmap(fsss);

            System.Drawing.Graphics r = System.Drawing.Graphics.FromImage(bitmap);




            r.DrawImage(fondo, 0, 0, Imagen.Width, Imagen.Height);
            fondo.Dispose();
            r.DrawImage(MakeGrayscale((Bitmap)Imagen), 0, 0, Imagen.Width, Imagen.Height);

            r.DrawImage(bitmaps, 0, 0, Imagen.Width, Imagen.Height);
            bitmaps.Dispose();
            return bitmap;
        }
      public static Bitmap MakeGrayscale(Bitmap original)
      {
          //make an empty bitmap the same size as original
          Bitmap newBitmap = new Bitmap(original.Width, original.Height);

          for (int i = 0; i < original.Width; i++)
          {
              for (int j = 0; j < original.Height; j++)
              {
                  //get the pixel from the original image
                  Color originalColor = original.GetPixel(i, j);

                  //create the grayscale version of the pixel
                  int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                      + (originalColor.B * .11));

                  //create the color object
                  Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                  //set the new image's pixel to the grayscale version
                  newBitmap.SetPixel(i, j, newColor);
              }
          }

          return newBitmap;
      }
    }
}
