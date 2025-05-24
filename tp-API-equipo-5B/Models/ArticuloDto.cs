using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using dominio;

namespace tp_API_equipo_5B.Models
{
    public class ArticuloDto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int idMarca { get; set; }
        public int idCategoria { get; set; }
        public string urlImagen { get; set; }
        public decimal precio { get; set; }
    }
}