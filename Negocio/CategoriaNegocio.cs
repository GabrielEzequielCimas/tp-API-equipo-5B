using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using accesoDatos;

namespace negocio
{

    public class CategoriaNegocio
    {
        public void eliminarCategoria(int id)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("delete from categorias where Id = @idCategoria");
                datos.setearParametro("idCategoria", id);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool BuscarCategoria(string codigo)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("select case when lower(@descripcion) in (select lower(Descripcion) from categorias) then 1 else 0 end as flag_marca");
                datos.setearParametro("descripcion", codigo);
                datos.ejecutarLectura();
                datos.Lector.Read();
                int flagCodigo = (int)datos.Lector["flag_marca"];
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
        public void agregarCategoria(string descripcion)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("insert into categorias (Descripcion) values (@descripcion)");
                datos.setearParametro("@descripcion", descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            ConexionDB categorias = new ConexionDB();
            categorias.setearConsulta("select Id, Descripcion from Categorias;");
            categorias.ejecutarLectura();
            try
            {
                while (categorias.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.idCategoria = (int)categorias.Lector["Id"];
                    aux.descripcion = (string)categorias.Lector["Descripcion"];

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
