/*
 * Created by: ppodila
 * Created: Tuesday, November 28, 2006
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FluidKit.Controls
{
	public class ImageButton : Button
	{
		static ImageButton()
		{
			//This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
			//This style is defined in themes\generic.xaml
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
		}


		#region properties

		public string ImageHover
		{
			get { return (string)GetValue(ImageHoverProperty); }
			set { SetValue(ImageHoverProperty, value); }
		}

		public string ImageNormal
		{
			get { return (string)GetValue(ImageNormalProperty); }
			set { SetValue(ImageNormalProperty, value); }
		}

		public string ImagePressed
		{
			get { return (string)GetValue(ImagePressedProperty); }
			set { SetValue(ImagePressedProperty, value); }
		}

		public string ImageDisabled
		{
			get { return (string)GetValue(ImageDisabledProperty); }
			set { SetValue(ImageDisabledProperty, value); }
		}


		#endregion

		#region dependency properties

		public static readonly DependencyProperty ImageNormalProperty =
		   DependencyProperty.Register(
			   "ImageNormal", typeof(string), typeof(ImageButton));


		public static readonly DependencyProperty ImageHoverProperty =
		  DependencyProperty.Register(
			  "ImageHover", typeof(string), typeof(ImageButton));

		public static readonly DependencyProperty ImagePressedProperty =
		DependencyProperty.Register(
			"ImagePressed", typeof(string), typeof(ImageButton));

		public static readonly DependencyProperty ImageDisabledProperty =
		DependencyProperty.Register(
			"ImageDisabled", typeof(string), typeof(ImageButton));

		#endregion

	}
}