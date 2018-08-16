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

namespace TakeAPicture.Controles
{
    /// <summary>
    /// Interaction logic for ImagenControl.xaml
    /// </summary>
    public partial class ImagenControl : UserControl
    {
        public ImagenControl()
        {
            InitializeComponent();
        }
        public ImageSource ImagenS
        {
            get {

                return Imagen.Source;
            }
            set {

                Imagen.Source = value;
            }
        }
    }
}
