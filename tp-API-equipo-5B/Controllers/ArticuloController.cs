using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dominio;
using negocio;
using tp_API_equipo_5B.Models;

namespace tp_API_equipo_5B.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            return negocio.Listar();
        }

        // GET: api/Articulo/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.Listar();
            return lista.Find(x=> x.idArticulo == id);
        }

        // POST: api/Articulo
        public void Post([FromBody]ArticuloDto articulo)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo nuevo = new Articulo();
            nuevo.codigo = articulo.codigo;
            nuevo.nombre = articulo.nombre;
            nuevo.descripcion = articulo.descripcion;
            nuevo.precio = articulo.precio;
            nuevo.marca = new Marca { idMarca = articulo.idMarca };
            nuevo.categoria = new Categoria { idCategoria = articulo.idCategoria };
            negocio.agregar(nuevo);
        }

        // PUT: api/Articulo/5
        public void Put(int id, [FromBody] ArticuloDto articulo)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo modificado = new Articulo();
            modificado.idArticulo = id;
            modificado.codigo = articulo.codigo;
            modificado.nombre = articulo.nombre;
            modificado.descripcion = articulo.descripcion;
            modificado.precio = articulo.precio;
            modificado.marca = new Marca { idMarca = articulo.idMarca };
            modificado.categoria = new Categoria { idCategoria = articulo.idCategoria };
            negocio.modificar(modificado);
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.eliminar(id);
        }
    }
}
