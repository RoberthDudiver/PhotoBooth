using System.Windows.Input;

namespace WpfCamera
{
    public static class CameraCommands
    {
        #region Properties

        public static RoutedCommand TakePhoto { get; private set; }

        #endregion

        #region Ctors

        static CameraCommands()
        {
            TakePhoto = new RoutedCommand("CaptureImage", typeof (CameraCommands));
        }

        #endregion
    }
}