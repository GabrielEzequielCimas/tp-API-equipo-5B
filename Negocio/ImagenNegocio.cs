using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using accesoDatos;

namespace negocio
{
    public class ImagenNegocio
    {
        public void agregar(Imagenes nuevo,int IdArticulo)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                if (IdArticulo == 0)
                {
                    datos.setearConsulta("insert into Imagenes (IdArticulo,ImagenUrl) select max(id),@ImagenUrl from articulos");
                }
                else
                { 
                    datos.setearConsulta("insert into Imagenes (IdArticulo,ImagenUrl) values (@IdArticuloImgaen,@ImagenUrl)");
                    datos.setearParametro("IdArticuloImgaen", IdArticulo);
                }
                datos.setearParametro("ImagenUrl", nuevo.url);
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
        public List<Imagenes> ListarImagenes(int idArticulo)
        {
            List<Imagenes> lista = new List<Imagenes>();
            ConexionDB imagenes = new ConexionDB();
            //imagenes.setearConsulta("select ImagenUrl from Imagenes where IdArticulo = " + idArticulo + ";");
            imagenes.setearConsulta("select Id,ImagenUrl from Imagenes where IdArticulo = @IdArticulo;");
            imagenes.setearParametro("IdArticulo", idArticulo);
            imagenes.ejecutarLectura();
            try
            {
                int contador = 0;
                while (imagenes.Lector.Read())
                {
                    Imagenes aux = new Imagenes();
                    aux.idImagen = (int)imagenes.Lector["Id"];
                    aux.url = (string)imagenes.Lector["ImagenUrl"];
                    aux.numeroImagen = contador += 1;
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void modificar(int id,string url)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("update IMAGENES set ImagenUrl = @url where Id = @id");
                datos.setearParametro("@url", url);
                datos.setearParametro("@id", id);


                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion(); }
        }
        public void eliminar(int id)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("delete from IMAGENES where Id = @id");
                datos.setearParametro("@id", id);


                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion(); }
        }
    }
}
