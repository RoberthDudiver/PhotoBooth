using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Image = System.Windows.Controls.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace WpfCamera
{
    public class CameraViewer : Image, IDisposable
    {
        #region Constants

        private const int PixelSize = 3;

        #endregion

        #region Private fields

        public static readonly DependencyProperty AvailableVideoModesProperty =
            DependencyProperty.Register("AvailableVideoModes", typeof (List<VideoMode>), typeof (CameraViewer),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty CaptureHeightProperty =
            DependencyProperty.Register("CaptureHeight", typeof (int), typeof (CameraViewer),
                new FrameworkPropertyMetadata(720, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty CaptureWidthProperty =
            DependencyProperty.Register("CaptureWidth", typeof (int), typeof (CameraViewer),
                new FrameworkPropertyMetadata(1280, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty CapturedImageProperty =
            DependencyProperty.Register("CapturedImage", typeof (ImageSource), typeof (CameraViewer),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty MaxFramerateProperty =
            DependencyProperty.Register("MaxFramerate", typeof (int), typeof (CameraViewer),
                new FrameworkPropertyMetadata(20, FrameworkPropertyMetadataOptions.None,
                    OnMaxFramerateChanged));

        public static readonly DependencyProperty PreviewDividerProperty =
            DependencyProperty.Register("PreviewDivider", typeof (int), typeof (CameraViewer),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof (string), typeof (CameraViewer),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None,
                    OnSelectedDeviceChanged));

        public static readonly DependencyProperty SelectedVieoModeProperty =
            DependencyProperty.Register("SelectedVideoMode", typeof (VideoMode), typeof (CameraViewer),
                new FrameworkPropertyMetadata(new VideoMode(), FrameworkPropertyMetadataOptions.None,
                    OnSelectedVideoModeChanged));

        private DirectShowDevice device;
        private int height;
        private bool isCopyingFrame;
        private int width;

        #endregion

        #region Properties

        public List<VideoMode> AvailableVideoModes
        {
            get { return (List<VideoMode>)GetValue(AvailableVideoModesProperty); }
            private set { SetValue(AvailableVideoModesProperty, value); }
        }

        public bool CameraFound
        {
            get { return device.CameraAvailable; }
        }

        public int CaptureHeight
        {
            get { return (int)GetValue(CaptureHeightProperty); }
            private set { SetValue(CaptureHeightProperty, value); }
        }

        public int CaptureWidth
        {
            get { return (int)GetValue(CaptureWidthProperty); }
            private set { SetValue(CaptureWidthProperty, value); }
        }

        public ImageSource CapturedImage
        {
            get { return (ImageSource)GetValue(CapturedImageProperty); }
            set { SetValue(CapturedImageProperty, value); }
        }

        public BitmapSource CurrentBitmap
        {
            get { return (BitmapSource)Source; }
        }

        public DirectShowDevice Device
        {
            get { return device; }
        }

        public int MaxFramerate
        {
            get { return (int)GetValue(MaxFramerateProperty); }
            set { SetValue(MaxFramerateProperty, value); }
        }

        public int PreviewDivider
        {
            get { return (int)GetValue(PreviewDividerProperty); }
            set { SetValue(PreviewDividerProperty, value); }
        }

        public string SelectedDevice
        {
            get { return (string)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        public VideoMode SelectedVieoMode
        {
            get { return (VideoMode)GetValue(SelectedVieoModeProperty); }
            set { SetValue(SelectedVieoModeProperty, value); }
        }

        #endregion

        #region Ctors

        public CameraViewer()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var src = new BitmapImage();
                src.BeginInit();
                try
                {
                    src.UriSource = new Uri("Imagenes/camera.png", UriKind.Relative);
                }
                catch {
                    
                
                }
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                Source = src;
                return;
            }
            CommandBindings.Add(new CommandBinding(CameraCommands.TakePhoto,
                TakePhoto_Executed, TakePhoto_CanExecute));

            Application.Current.Exit += OnApplicationExit;
            Initialized += CameraViewerInitialized;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        #endregion

        #region Event triggers

        private static void OnMaxFramerateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as CameraViewer;
            if (viewer != null && viewer.device != null)
            {
                viewer.device.FrameTime = (int)e.NewValue;
            }
        }
        public Bitmap EliminarVerde(Bitmap input)
        {
          //  Bitmap input = new Bitmap(@"G:\Greenbox.jpg");

            Bitmap output = new Bitmap(input.Width, input.Height);

            // Iterate over all piels from top to bottom...
            for (int y = 0; y < output.Height; y++)
            {
                // ...and from left to right
                for (int x = 0; x < output.Width; x++)
                {
                    // Determine the pixel color
                    System.Drawing.Color camColor = input.GetPixel(x, y);

                    // Every component (red, green, and blue) can have a value from 0 to 255, so determine the extremes
                    byte max = Math.Max(Math.Max(camColor.R, camColor.G), camColor.B);
                    byte min = Math.Min(Math.Min(camColor.R, camColor.G), camColor.B);

                    // Should the pixel be masked/replaced?
                    bool replace =
                        camColor.G != min // green is not the smallest value
                        && (camColor.G == max // green is the biggest value
                        || max - camColor.G < 8) // or at least almost the biggest value
                        && (max - min) > 96; // minimum difference between smallest/biggest value (avoid grays)

                    if (replace)
                        camColor = System.Drawing.Color.Magenta;

                    // Set the output pixel
                    output.SetPixel(x, y, camColor);
                }
            }
            return output;
        }
        private static void OnSelectedDeviceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as CameraViewer;
            if (viewer != null)
            {
                viewer.AvailableVideoModes = viewer.device.SelectVideoInput((string)e.NewValue, viewer.CaptureWidth,
                    viewer.CaptureHeight);
            }
        }

        private static void OnSelectedVideoModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mode = (VideoMode)e.NewValue;
            var viewer = d as CameraViewer;
            if (viewer != null)
            {
                viewer.CaptureWidth = mode.Width;
                viewer.CaptureHeight = mode.Height;
                viewer.ApplyVideoMode();
                viewer.AvailableVideoModes = viewer.device.SelectVideoInput(viewer.SelectedDevice, viewer.CaptureWidth,
                    viewer.CaptureHeight);
            }
        }

        void OnApplicationExit(object sender, ExitEventArgs e)
        {
            GC.Collect();
            Dispose();
        }

        public void cerrar()
        {
            GC.Collect();

            Dispose();
        }

        #endregion

        #region Event handlers

        private void CameraViewerInitialized(object sender, EventArgs e)
        {
            width = CaptureWidth;
            height = CaptureHeight;
            device = new DirectShowDevice(CaptureWidth, CaptureHeight, PreviewDivider, MaxFramerate);
            device.FrameReady += DeviceFrameReady;
        }

        void DeviceFrameReady(object sender, EventArgs e)
        {
            GC.Collect();
            Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    if (isCopyingFrame == false)
                    {
                        isCopyingFrame = true;
                        var bitmap = new Bitmap(width / PreviewDivider, height / PreviewDivider,
                            PixelFormat.Format24bppRgb);

                        BitmapData bmpData = bitmap.LockBits(
                            new Rectangle(0, 0, bitmap.Width / PreviewDivider, bitmap.Height / PreviewDivider),
                            ImageLockMode.ReadOnly, bitmap.PixelFormat);

                        Marshal.Copy(device.PreviewPixelMap, 0, bmpData.Scan0,
                            width * height * PixelSize / PreviewDivider / PreviewDivider);

                        bitmap.UnlockBits(bmpData);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

                       // bitmap = EliminarVerde(bitmap);
                        //bitmap = (Bitmap)Efectos.ProcesarCartoon(bitmap);
                        IntPtr hBitmap = bitmap.GetHbitmap();

                        BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        Source = bitmapSource;

                        DeleteObject(hBitmap);

                        isCopyingFrame = false;
                    }
                }));
        }

        #endregion

        #region Public methods

        public void ApplyResolutionChange()
        {
            width = CaptureWidth;
            height = CaptureHeight;
            device.SetResolution(width, height);
        }

        public void ApplyVideoMode()
        {
            width = CaptureWidth;
            height = CaptureHeight;
        }
        public void GuardarImagen(string nombre)
        {
            string dondeG = Efectos.AssemblyDirectory + @"\Imagenes";
            System.IO.DirectoryInfo Dire = new System.IO.DirectoryInfo(dondeG);
            if (!Dire.Exists)
            {
                Dire.Create();
            }

            System.Drawing.Image Img = UltimoBitmap;
            Img.Save(dondeG + @"\" + nombre + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public Bitmap UltimoBitmap
        {
            get;
            set;
        }
        public BitmapSource GetCapturedBitmap()
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            Marshal.Copy(device.CapturedPixelMap, 0, bmpData.Scan0, width * height * PixelSize);

            bitmap.UnlockBits(bmpData);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

           //bitmap = (Bitmap)Efectos.ProcesarCartoon(bitmap);
            UltimoBitmap = bitmap;
            
            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return bitmapSource;
        }
        public BitmapSource GetCapturedBitmap(TipoDeFoto Tipo)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            Marshal.Copy(device.CapturedPixelMap, 0, bmpData.Scan0, width * height * PixelSize);

            bitmap.UnlockBits(bmpData);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            if (Tipo == TipoDeFoto.Comic)
            {
                bitmap = (Bitmap)Efectos.ProcesarCartoon(bitmap);
            }

            if (Tipo == TipoDeFoto.Vieja)
            {
                bitmap = (Bitmap)Efectos.ProcesarImagenVieja(bitmap);
            }

            UltimoBitmap = bitmap;
            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return bitmapSource;
        }

        public void SetDefaultDevice()
        {
            device.SelectVideoInput();
        }

        #endregion

        #region Private methods

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private static void TakePhoto_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var viewer = sender as CameraViewer;
            if (viewer != null)
            {
                e.CanExecute = viewer.Device.CameraAvailable;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private static void TakePhoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewer = sender as CameraViewer;
            if (viewer != null)
            {
                viewer.CapturedImage = viewer.GetCapturedBitmap();
            }
        }

        #endregion
    }
    public enum TipoDeFoto
    {
        Normal
        ,Vieja
        ,Comic
    }
}