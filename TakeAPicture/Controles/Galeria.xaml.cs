using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluidKit.Controls;
using System.Diagnostics;

namespace TakeAPicture.Controles
{
    /// <summary>
    /// Interaction logic for Galeria.xaml
    /// </summary>
    public partial class Galeria : UserControl
    {
         public Galeria()
        {
            InitializeComponent();
            Loaded += Galeria_Loaded;
            _elementFlow.Click += _elementFlow_Click;
            
        }

         void _elementFlow_Click(object sender, RoutedEventArgs e)
         {
             Debug.WriteLine((sender as ElementFlow).SelectedIndex);
             _elementFlow.PopoutDistance = 0.5;
             if (CambiodeSeleccion != null && Click)
             {
                 CambiodeSeleccion(this, sender as ElementFlow);
             }
         }

        public double _frontItemGapSlider
        {
            get {
                return _elementFlow.ItemGap;
            }
            set {

                _elementFlow.ItemGap = value;
            }
        }

        public int Seleccion
        {
            get {

                return _elementFlow.SelectedIndex;
            }
            set {
                try
                {
                    _elementFlow.SelectedIndex = value;
                }
                catch { }
            }
        }
        public double _tiltAngleSlider
        {
            get
            {
                return _elementFlow.TiltAngle;
            }
            set
            {

                _elementFlow.TiltAngle = value;
            }
        }
        public double _popoutDistanceSlider
        {
              get
            {
                return _elementFlow.PopoutDistance;
            }
            set
            {

                _elementFlow.PopoutDistance = value;
            }
        }
        
        public double _itemGapSlider
        {
            get
            {
                return _elementFlow.FrontItemGap;
            }
            set
            {

                _elementFlow.FrontItemGap = value;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                _viewIndex = (_viewIndex + 1) % _layouts.Length;
                _elementFlow.Layout = _layouts[_viewIndex];             
            }
        }
        protected override void OnTouchMove(TouchEventArgs e)
        {
            if (Click)
            {
                Point Po = e.GetTouchPoint(this).Position;
                PuntosMouse.Add(Po);
            }
        }
        protected override void OnTouchDown(TouchEventArgs e)
        {
            Click = true;
            
        }
        protected override void OnTouchUp(TouchEventArgs e)
        {
            Mover();
            Click = false;
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //Click = true;
            
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

            //Mover();
            //Click = false;

        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Click = true;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (Click)
            //{
            //    Point Po = e.GetPosition(this);
            //    PuntosMouse.Add(Po);
            //}
        }
        List<Point> _PuntosMouse;
        List<Point> PuntosMouse
        {
            get
            {

                if (_PuntosMouse == null)
                {
                    _PuntosMouse = new List<Point>();
                }
                return _PuntosMouse;
            }
            set
            {
                _PuntosMouse = value;
            }
        }
        void Mover()
        {
            if (Click)
            {
                if (PuntosMouse.Count >= 2)
                {
                    if (PuntosMouse[0].X > PuntosMouse[PuntosMouse.Count - 1].X)
                    {
                        Seleccion += 1;
                        PuntosMouse.Clear();
                    }
                    else
                    {
                        if (PuntosMouse[0].X < PuntosMouse[PuntosMouse.Count - 1].X)
                        {
                            Seleccion -= 1;
                            PuntosMouse.Clear();
                        }
                    }
                }

            }
            Click = false;
            PuntosMouse.Clear();
        }
        bool Click;
        void Galeria_Loaded(object sender, RoutedEventArgs e)
        {
           // Cargar();
        }
        bool pasosele;
        public void Cargar()
        {
            if (_dataSource != null)
            {

                _dataSource.Clear();
            }
            _elementFlow.Layout = _layouts[1];

            if (!pasosele)
            {
                _elementFlow.SelectionChanged += _elementFlow_SelectionChanged;
                pasosele = true;
            }
         //   _elementFlow.SelectedIndex = 0;

            _dataSource = FindResource("DataSource") as StringCollection;
           // _elementFlow.SelectedIndex = 0;
        }
         public StringCollection _dataSource;

         public StringCollection _Imagenes
         {
             get
             {
                 if (_dataSource == null)
                 {
                     _dataSource = new StringCollection();
                 }
                 return _dataSource;
             }
             set {
                 _dataSource = value;
             }
         }
        private LayoutBase[] _layouts = {
		                                	new Wall(),
		                                	new SlideDeck(),
		                                	new CoverFlow(),
		                                	new Carousel(),
		                                	new TimeMachine2(),
		                                	new ThreeLane(),
		                                	new VForm(),
		                                	new TimeMachine(),
		                                	new RollerCoaster(),
		                                	new Rolodex(),
		                                };
        private Random _randomizer = new Random();

        private int _viewIndex;
 
       
       public void Remover()
        {
            if (_elementFlow.Items.Count > 0)
            {
                _dataSource.RemoveAt(_randomizer.Next(_dataSource.Count));

                // Update selectedindex slider
                //   _selectedIndexSlider.Maximum = _elementFlow.Items.Count - 1;
            }
        }
     
       public void Agregar(bool A)
        {
            // Button b = sender as Button;
            int index = _randomizer.Next(_dataSource.Count);
            if (A)
            {
                _dataSource.Insert(index, "Images/01.jpg");
            }
            else
            {
                _dataSource.Insert(index, string.Format("Images/{0:00}", _randomizer.Next(1, 13)) + ".jpg");
            }
            // Update selectedindex slider
            //   _selectedIndexSlider.Maximum = _elementFlow.Items.Count - 1;
        }
        
       public void Agregar(string Imagenes)
       {
           // Button b = sender as Button;
           int index = _randomizer.Next(_dataSource.Count);           
           _dataSource.Insert(index, Imagenes);
           
       }
       public delegate void  _Seleccion(object sender, ElementFlow Elemento);
       public event _Seleccion CambiodeSeleccion;

        void _elementFlow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Debug.WriteLine((sender as ElementFlow).SelectedIndex);
            //_elementFlow.PopoutDistance = 0.5;
            //if (CambiodeSeleccion != null && Click)
            //{
            //    CambiodeSeleccion(this, sender as ElementFlow);
            //}

        }

        private void _elementFlow_MouseLeave(object sender, MouseEventArgs e)
        {
            Click = false;
        }


    }
    
}
