using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int idArticulo { get; set; }
        [DisplayName("Código")]
        public string codigo { get; set; }
        [DisplayName("Nombre")]
        public string nombre { get; set; }
        [DisplayName("Descripción")]
        public string descripcion { get; set; }
        [DisplayName("Marca")]
        public Marca marca { get; set; }
        [DisplayName("Categoría")]
        public Categoria categoria { get; set; }
        public List<Imagenes> imagenes { get; set; } = new List<Imagenes>();
        [DisplayName("Precio")]
        public decimal precio { get; set; }
    }
}
