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
        public HttpResponseMessage Post([FromBody]ArticuloDto articulo)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();

                // Validación de existencia por nombre o código
                if (negocio.ExisteArticuloPorNombreOCodigo(articulo.nombre, articulo.codigo))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ya existe un artículo con ese nombre o código.");
                }

                Articulo nuevo = new Articulo
                {
                    codigo = articulo.codigo,
                    nombre = articulo.nombre,
                    descripcion = articulo.descripcion,
                    precio = articulo.precio,
                    marca = new Marca { idMarca = articulo.idMarca },
                    categoria = new Categoria { idCategoria = articulo.idCategoria }
                };

                negocio.agregar(nuevo);
                return Request.CreateResponse(HttpStatusCode.Created, "Artículo agregado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT: api/Articulo/5
        public HttpResponseMessage Put(int id, [FromBody] ArticuloDto articulo)
        {
            try
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

                // Validaciones
                if (!negocio.ExisteMarca(articulo.idMarca))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La marca especificada no existe.");

                if (!negocio.ExisteCategoria(articulo.idCategoria))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La categoría especificada no existe.");

                negocio.modificar(modificado);
                return Request.CreateResponse(HttpStatusCode.OK, "Artículo modificado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE: api/Articulo/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();

                if (!negocio.ExisteArticulo(id))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No se encontró un artículo con ID = {id}.");

                negocio.eliminar(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Artículo eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
