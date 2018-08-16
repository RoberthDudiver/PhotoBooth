namespace WpfCamera
{
    public struct VideoMode
    {
        #region Private fields

        public int Height;
        public int Width;

        #endregion

        #region Properties

        public string VideoModeString
        {
            get { return Width + "x" + Height; }
        }

        #endregion

        #region Ctors

        public VideoMode(int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion
    }
}