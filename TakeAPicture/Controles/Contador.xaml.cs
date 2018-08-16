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
    /// Interaction logic for Contador.xaml
    /// </summary>
    public partial class Contador : UserControl
    {
        public Contador()
        {
            InitializeComponent();
        }

        public string Texto
        {
            get
            {
                return TextoC.Text;
            }
            set{
                TextoC.Text = value;

            }
        }
    }
}
