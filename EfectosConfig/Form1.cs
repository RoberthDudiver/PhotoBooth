using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace EfectosConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void CrealLienzo(Size Tamano)
        {
            Bitmap Esta = (Bitmap)imagenc.BackgroundImage;

            actual = Tamano;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Esta, actual);

            imagenc.BackColor = Color.White;
            imagenc.BackgroundImage = bitmap;
            imagenc.Size = Tamano;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CrealLienzo(new Size(1200, 1800));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CrealLienzo(new Size(1800, 1800));

        }

        private void Pmenu_Paint(object sender, PaintEventArgs e)
        {

        }
        void fmove(object sender, MouseEventArgs e)
        {


            ((PictureBox)sender).BringToFront();
            ((PictureBox)sender).Capture = false;
            if (this.imagenc.Controls.Contains(CSharpTracker))
                this.imagenc.Controls.Remove(CSharpTracker);
            CSharpTracker = new RectTracker(((PictureBox)sender));
            this.imagenc.Controls.Add(CSharpTracker);
            CSharpTracker.BringToFront();
            CSharpTracker.Draw();


            //throw new Exception("The method or operation is not implemented.");
        }
      
        RectTracker CSharpTracker;
        Size actual;
        private void button4_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int i = 0; i <= 3; i++)
            {
                PictureBox f2 = new PictureBox();
                this.imagenc.Controls.Add(f2);
                f2.Width = 80;
                f2.Height = 80;
                f2.BackColor = Color.White;
                f2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                f2.Tag = RADMLIB.TipoImagen.Foto;

                f2.Location = new Point(x);
                x += f2.Width;
                f2.Image = pictureBox1.Image;
                f2.LostFocus += f2_LostFocus;
                f2.MouseDown += new MouseEventHandler(fmove);
                f2.MouseUp += f2_MouseUp;
            }

        }

        void f2_MouseUp(object sender, MouseEventArgs e)
        {
            //if (this.imagenc.Controls.Contains(CSharpTracker))
            //    this.imagenc.Controls.Remove(CSharpTracker);
        }

   

        void f2_LostFocus(object sender, EventArgs e)
        {
            
        }

        public void fondo()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPeg Image|*.jpg|Png Image|*.png|Gif Image|*.gif";
            openFileDialog1.ShowDialog();
            try
            {
                Bitmap Esta =(Bitmap) Bitmap.FromFile(openFileDialog1.FileName);
               
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Esta, actual);

            imagenc.BackgroundImage = bitmap;
            this.imagenc.Height = bitmap.Height;
            this.imagenc.Width = bitmap.Width;
               
             
            }
            catch
            {
            }
        }
        public void Banner()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPeg Image|*.jpg|Png Image|*.png|Gif Image|*.gif";
            openFileDialog1.ShowDialog();
            try
            {
                Bitmap Esta = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);

                //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Esta, actual);

                PictureBox f2 = new PictureBox();
                this.imagenc.Controls.Add(f2);
                f2.Width = Esta.Width;
                f2.Height = Esta.Height;
                f2.BackColor = Color.White;
                f2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

                f2.Tag =RADMLIB.TipoImagen.Banner;
                f2.Image = Esta;
                f2.LostFocus += f2_LostFocus;
                f2.MouseDown += new MouseEventHandler(fmove);
                f2.MouseUp += f2_MouseUp;


            }
            catch
            {
            }
        }

        public void ImagenBoton()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPeg Image|*.jpg|Png Image|*.png|Gif Image|*.gif";
            openFileDialog1.ShowDialog();
            try
            {
                Bitmap Esta = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);

                //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Esta, actual);
                pictureBox2.Image = Esta;
                pictureBox2.Visible = true;

            }
            catch
            {
                pictureBox2.Visible = false;

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Pcontenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            fondo();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Banner();
        }
        bool retornartag(object tag , RADMLIB.TipoImagen tipo)
        {
            if(tag!=null)
            {
            return (((RADMLIB.TipoImagen)tag) == tipo);
            }
            else
            {
                return false;
            }
        }

        //public void Test(string fileName)
        //{
        //    string path = Path.GetDirectoryName(fileName);
        //    string filename_with_ext = Path.GetFileName(fileName);
        //    string filename_without_ext = Path.GetFileNameWithoutExtension(fileName);
        //    string ext_only = Path.GetExtension(fileName);
        //}
        private void button1_Click_1(object sender, EventArgs e)
        {

            SaveFileDialog Save = new SaveFileDialog();
            Save.Filter = "Solo Sistema Item|*.sitm";
            Save.ShowDialog();
           string  nombre = Path.GetFileNameWithoutExtension(Save.FileName);
            
            RADMLIB.Lienzo li = new RADMLIB.Lienzo();
            li.Alto = actual.Width;
            li.NombreArchivo = nombre;
            li.Ancho = actual.Height;
            li.ImagenBoton = (Bitmap)pictureBox2.Image;
            List<Control> Lista = imagenc.Controls.Cast<Control>().ToList();
            var esta = Lista.Where(x => retornartag(x.Tag, RADMLIB.TipoImagen.Foto)).ToList();           
            List<PictureBox> listap = esta.Cast<PictureBox>().ToList();
                List<RADMLIB.Items> Fotos = new List<RADMLIB.Items>();
            foreach (PictureBox Pic in listap)
            {
                Fotos.Add(new RADMLIB.Items() { Tamano = Pic.Size, Tipo = RADMLIB.TipoImagen.Foto, PosicionDeItems = Pic.Location });
            }
            li.Fotos = Fotos;
            Control unico = Lista.SingleOrDefault(x => retornartag(x.Tag, RADMLIB.TipoImagen.Banner));
            if (unico != null)
            {
                li.Banner = new RADMLIB.Items()
                {
                    PosicionDeItems = unico.Location,
                    Tipo = RADMLIB.TipoImagen.Banner,
                    Tamano= unico.Size
                    ,
                    Imagen = (Bitmap)((PictureBox)unico).Image

                };
            }
            li.Fondo = new RADMLIB.Items() { Imagen = (Bitmap)imagenc.BackgroundImage, Tipo=RADMLIB.TipoImagen.Foto };
            RADMLIB.TrabajoDeObjetos Trabajo = new RADMLIB.TrabajoDeObjetos();
           
            try{

                Trabajo.SerializeObject(li,Save.FileName);
                var este=Trabajo.DeSerializeObject<RADMLIB.Lienzo>(Save.FileName);
            }
            catch{}
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ImagenBoton();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            CrealLienzo(new Size(1800, 1200));
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ImagenBoton();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CrealLienzo(new Size(800, 800));

        }
    }
}
