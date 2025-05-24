using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using accesoDatos;

namespace negocio
{
    public class MarcaNegocio
    {
        public void eliminarMarca(int id)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("delete from marcas where Id = @idMarca");
                datos.setearParametro("idMarca", id);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool BuscarMarca(string codigo)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("select case when lower(@descripcion) in (select lower(Descripcion) from marcas) then 1 else 0 end as flag_marca");
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
        public void agregarMarca(string descripcion)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("insert into marcas (Descripcion) values (@descripcion)");
                datos.setearParametro("@descripcion", descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Marca> ListarMarcas()
        {
            List<Marca> lista = new List<Marca>();
            ConexionDB marcas = new ConexionDB();
            marcas.setearConsulta("select Id,Descripcion from marcas;");
            marcas.ejecutarLectura();
            try
            {
                while (marcas.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.idMarca = (int)marcas.Lector["Id"];
                    aux.descripcion = (string)marcas.Lector["Descripcion"];
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
