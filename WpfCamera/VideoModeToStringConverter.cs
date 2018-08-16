using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfCamera
{
    public class VideoModeToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            var results = new List<string>();
            var videoModes = (List<VideoMode>)value;

            foreach (VideoMode videoMode in videoModes)
            {
                results.Add(videoMode.Width + "x" + videoMode.Height);
            }

            return results;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var videoModeStrings = value as List<string>;
            var videoModes = new List<VideoMode>();

            if (videoModeStrings != null)
            {
                foreach (string videoModeString in videoModeStrings)
                {
                    string[] widthAndHeight = videoModeString.Split('x');

                    int width = System.Convert.ToInt32(widthAndHeight[0]);
                    int height = System.Convert.ToInt32(widthAndHeight[1]);
                    videoModes.Add(new VideoMode(width, height));
                }
            }
            return videoModes;
        }

        #endregion
    }
}