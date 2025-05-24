using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using accesoDatos;
using dominio;

namespace negocio
{

    public class VoucherNegocio
    {
        public bool BuscarVoucher(string codigo)
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("select case when @codigoVoucher in (select CodigoVoucher from Vouchers) then 1 else 0 end as flag_codigo");
                datos.setearParametro("codigoVoucher", codigo);
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
        public List<Vouchers> Listar()
        {
            List<Vouchers> lista = new List<Vouchers>();
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("select CodigoVoucher, IdCliente, FechaCanje, IdArticulo from vouchers ");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Vouchers aux = new Vouchers();
                    aux.codigoVoucher = (string)datos.Lector["CodigoVoucher"];
                    aux.idCliente = datos.Lector["IdCliente"] != DBNull.Value ? (int?)datos.Lector["IdCliente"] : null;
                    aux.FechaCanje = datos.Lector["FechaCanje"] != DBNull.Value ? (DateTime?)datos.Lector["FechaCanje"] : null;
                    aux.idArticulo = datos.Lector["IdArticulo"] != DBNull.Value ? (int?)datos.Lector["IdArticulo"] : null;
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

        public bool VoucherYaCanjeado(string codigoVoucher)
        {
            ConexionDB conexion = new ConexionDB();
            try
            {
                conexion.setearConsulta("SELECT FechaCanje FROM Vouchers WHERE CodigoVoucher = @codigo");
                conexion.setearParametro("@codigo", codigoVoucher);
                conexion.ejecutarLectura();

                if (conexion.Lector.Read())
                {
                    return !conexion.Lector.IsDBNull(0); // Verifica si FechaCanje no es null
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.cerrarConexion();
            }
        }

        public bool CanjearVoucher(string codigoVoucher, int idCliente, int idArticulo)
        {
            ConexionDB conexion = new ConexionDB();
            try
            {
                conexion.setearConsulta(
                    "UPDATE Vouchers SET IdCliente = @idCliente, FechaCanje = @fecha, IdArticulo = @idArticulo " +
                    "WHERE CodigoVoucher = @codigo AND FechaCanje IS NULL");

                conexion.setearParametro("@codigo", codigoVoucher);
                conexion.setearParametro("@idCliente", idCliente);
                conexion.setearParametro("@fecha", DateTime.Now);
                conexion.setearParametro("@idArticulo", idArticulo);

                conexion.ejecutarAccion();

                return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.cerrarConexion();
            }
        }

        public int InsertarCliente(string documento, string nombre, string apellido, string email,
                                 string direccion, string ciudad, string cp)
        {
            ConexionDB conexion = new ConexionDB();
            try
            {
                conexion.setearConsulta(
                    "INSERT INTO Clientes (Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP) " +
                    "VALUES (@documento, @nombre, @apellido, @email, @direccion, @ciudad, @cp)");

                conexion.setearParametro("@documento", documento);
                conexion.setearParametro("@nombre", nombre);
                conexion.setearParametro("@apellido", apellido);
                conexion.setearParametro("@email", email);
                conexion.setearParametro("@direccion", direccion);
                conexion.setearParametro("@ciudad", ciudad);
                conexion.setearParametro("@cp", cp);

                conexion.ejecutarAccion();
                conexion.cerrarConexion();

                // Obtener el ID del cliente insertado
                conexion.setearConsulta(
                    "SELECT TOP 1 Id FROM Clientes WHERE Documento = @documento ORDER BY Id DESC");
                conexion.setearParametro("@documento", documento);
                conexion.ejecutarLectura();

                if (conexion.Lector.Read())
                {
                    return Convert.ToInt32(conexion.Lector["Id"]);
                }
                throw new Exception("No se pudo obtener el ID del cliente insertado");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.cerrarConexion();
            }
        }
    }
}
