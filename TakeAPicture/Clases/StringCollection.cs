using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TakeAPicture
{
    public class StringCollection : ObservableCollection<string>
    {
        public StringCollection()
        {

        }
        public StringCollection(bool True)
        {
          //  Add(@"C:\Users\Usuario\Documents\Visual Studio 2010\Projects\AppTSHotel\AppTSHotel\Images\01.jpg");
           // Add("Images/02.jpg");
          //  Add("Images/03.jpg");

        }

        public bool Boton
        {
            get;
            set;
        }

    }
}
