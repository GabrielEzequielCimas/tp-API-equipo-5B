using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dominio;
using accesoDatos;


namespace negocio
{
    public class ArticuloNegocio
    {   
        public bool BuscarArticulo(string codigo){
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("select case when @codigoArticulo in (select Codigo from ARTICULOS) then 1 else 0 end as flag_codigo");
                datos.setearParametro("CodigoArticulo",  codigo);
                datos.ejecutarLectura();
                datos.Lector.Read();
                int flagCodigo = (int)datos.Lector["flag_codigo"];
                if (flagCodigo == 1)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("select a.Id, Codigo, Nombre, a.Descripcion, a.IdMarca, M.Descripcion as marca, a.IdCategoria, C.Descripcion as categoria, Precio from ARTICULOS A join MARCAS M on a.IdMarca = M.Id join CATEGORIAS C on A.IdCategoria = c.Id; ");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.idArticulo = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.categoria = new Categoria();
                    aux.categoria.idCategoria = (int)datos.Lector["IdCategoria"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Categoria"))))//validar si la DB trae un null
                        aux.categoria.descripcion = (string)datos.Lector["Categoria"];
                    else aux.categoria.descripcion = "";
                    aux.marca = new Marca();
                    aux.marca.idMarca = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["Marca"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    aux.precio = (decimal)datos.Lector["Precio"];
                    ImagenNegocio imagen = new ImagenNegocio();
                    aux.imagenes = imagen.ListarImagenes(aux.idArticulo);
                    lista.Add(aux);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void agregar(Articulo nuevo)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                //datos.setearConsulta("insert into articulos (Codigo, Nombre, Descripcion) values ('" + nuevo.codigo + "','" + nuevo.nombre + "','" + nuevo.descripcion + "');");
                //datos.setearConsulta("insert into articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values ('" + nuevo.codigo + "','" + nuevo.nombre + "','" + nuevo.descripcion + "'," + nuevo.marca.idMarca + "," + nuevo.categoria.idCategoria + "," + nuevo.precio + ")");
                datos.setearConsulta("insert into articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values (@CodigoArticulo,@NombreArticulo,@DescripcionArticulo,@MarcaArticulo,@CategoriaArticulo,@PrecioArticulo)");
                datos.setearParametro("CodigoArticulo", nuevo.codigo);
                datos.setearParametro("NombreArticulo", nuevo.nombre);
                datos.setearParametro("DescripcionArticulo", nuevo.descripcion);
                datos.setearParametro("MarcaArticulo", nuevo.marca.idMarca);
                datos.setearParametro("CategoriaArticulo", nuevo.categoria.idCategoria);
                datos.setearParametro("PrecioArticulo", nuevo.precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Articulo modificar)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, Precio = @Precio where Id = @id");
                datos.setearParametro("@codigo", modificar.codigo);
                datos.setearParametro("@nombre", modificar.nombre);
                datos.setearParametro("@descripcion", modificar.descripcion);
                datos.setearParametro("idMarca", modificar.marca.idMarca);
                datos.setearParametro("idCategoria", modificar.categoria.idCategoria);
                datos.setearParametro("@Precio", modificar.precio);
                datos.setearParametro("@id", modificar.idArticulo);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion(); }
        }
        public void eliminar(Articulo articulo)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("delete from articulos where Id = @id");
                datos.setearParametro("@id", articulo.idArticulo);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            ConexionDB datos = new ConexionDB();
            try
            {
                string consulta = ("select a.Id, Codigo, Nombre, a.Descripcion, a.IdMarca, M.Descripcion as marca, a.IdCategoria, C.Descripcion as categoria, Precio from ARTICULOS A left join MARCAS M on a.IdMarca = M.Id left join CATEGORIAS C on A.IdCategoria = c.Id where ");
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "a.Precio >" + filtro;
                            break;
                        case "Menor a":
                            consulta += "a.Precio <" + filtro;
                            break;
                        default:
                            consulta += "a.Precio =" + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "a.Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "a.Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.Descripción like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "a.Descripción like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "a.Descripción like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.idArticulo = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.categoria = new Categoria();
                    aux.categoria.idCategoria = (int)datos.Lector["IdCategoria"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Categoria"))))//validar si la DB trae un null
                        aux.categoria.descripcion = (string)datos.Lector["Categoria"];
                    else aux.categoria.descripcion = "";
                    aux.marca = new Marca();
                    aux.marca.idMarca = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["Marca"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    aux.precio = (decimal)datos.Lector["Precio"];
                    ImagenNegocio imagen = new ImagenNegocio();
                    aux.imagenes = imagen.ListarImagenes(aux.idArticulo);
                    lista.Add(aux);
                }


                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}