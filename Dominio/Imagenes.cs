using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Imagenes
    {
        public int idImagen { get; set; }
        public string url { get; set; }
        public int numeroImagen { get; set; }
        public override string ToString()
        {
            return "Imagen " + numeroImagen;
        }
    }
}

