using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace RADMLIB
{
    [Serializable]
    public class Lienzo
    {
        public int Alto
        {
            get;
            set;
        }
        public string  NombreArchivo
        {
            get;
            set;
        }

        public int Ancho
        {
            get;
            set;
        }


        [XmlIgnore]
        public System.Drawing.Bitmap ImagenBoton
        {
            get;
            set;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("ImagenBoton")]
        public byte[] ImagenBotonSerialized
        {
            get
            { // serialize
                if (ImagenBoton == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    ImagenBoton.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    ImagenBoton = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        ImagenBoton = new Bitmap(ms);
                    }
                }
            }
        }

        public List<Items> Fotos
        {
            get;
            set;
        }
        Items banner;
        public Items Banner
        {
            get { return banner; }
            set
            {
                value.Tipo = TipoImagen.Banner;
                banner = value;
            }
        }
        Items fondo;
        public Items Fondo
        {
            get { return fondo; }
            set {
                value.Tipo = TipoImagen.Fondo;
                fondo = value;
            }
        }
    }
    [Serializable]
  public  class Items
    {
        [XmlIgnore]
        public System.Drawing.Bitmap Imagen
        {
            get;
            set;
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Imagen")]
        public byte[] ImagenSerialized
        {
            get
            { // serialize
                if (Imagen == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Imagen.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    Imagen = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        Imagen = new Bitmap(ms);
                    }
                }
            }
        }


        public Point PosicionDeItems
        {
            get;
            set;
        }

        public Size Tamano
        {
            get;
            set;
        }
        public int PorcentajeAlto
        {
            get;
            set;
        }

        public int PorcentajeAncho
        {
            get;
            set;
        }
        public TipoImagen Tipo
        {
            get;
            set;
        }
    }

    [Serializable]
    public enum TipoImagen
    {
        Foto,
        Banner,
        Fondo,
        Texto
    }
}
