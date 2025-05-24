using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using accesoDatos;
using dominio;

namespace negocio
{
    public class ClienteNegocio
    {
        public int BuscarIdCliente()
        {
            try
            {
                ConexionDB datos = new ConexionDB();
                datos.setearConsulta("select max(Id) as Id from clientes");
                datos.ejecutarLectura();
                datos.Lector.Read();
                int id = (int)datos.Lector["Id"];
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("select Id, Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP from clientes ");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Dni = datos.Lector["Documento"].ToString();
                    aux.Nombre = datos.Lector["nombre"].ToString();
                    aux.Apellido = datos.Lector["Apellido"].ToString();
                    aux.Email = datos.Lector["Email"].ToString();
                    aux.Direccion = datos.Lector["Direccion"].ToString();
                    aux.Ciudad = datos.Lector["Ciudad"].ToString();
                    aux.CP = datos.Lector["CP"].ToString();
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

        public void agregar(Cliente nuevo)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                //datos.setearConsulta("insert into articulos (Codigo, Nombre, Descripcion) values ('" + nuevo.codigo + "','" + nuevo.nombre + "','" + nuevo.descripcion + "');");
                //datos.setearConsulta("insert into articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values ('" + nuevo.codigo + "','" + nuevo.nombre + "','" + nuevo.descripcion + "'," + nuevo.marca.idMarca + "," + nuevo.categoria.idCategoria + "," + nuevo.precio + ")");
                datos.setearConsulta("insert into Clientes ( Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP) values ( @Documento, @Nombre, @Apellido, @Email, @Direccion, @Ciudad, @CP)");
                datos.setearParametro("Id", nuevo.Id);
                datos.setearParametro("Documento", nuevo.Dni);
                datos.setearParametro("Nombre", nuevo.Nombre);
                datos.setearParametro("Apellido", nuevo.Apellido);
                datos.setearParametro("Email", nuevo.Email);
                datos.setearParametro("Direccion", nuevo.Direccion);
                datos.setearParametro("Ciudad", nuevo.Ciudad);
                datos.setearParametro("CP", nuevo.CP);
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
        public void modificar(Cliente modificar)
        {
            ConexionDB datos = new ConexionDB();
            try
            {
                datos.setearConsulta("update clientes set Nombre=@Nombre , Apellido=@Apellido , Email=@Email , Direccion=@Direccion , Ciudad=@Ciudad , CP=@CP where id = @id");
                datos.setearParametro("Id", modificar.Id);
                //datos.setearParametro("Documento", modificar.Dni);
                datos.setearParametro("Nombre", modificar.Nombre);
                datos.setearParametro("Apellido", modificar.Apellido);
                datos.setearParametro("Email", modificar.Email);
                datos.setearParametro("Direccion", modificar.Direccion);
                datos.setearParametro("Ciudad", modificar.Ciudad);
                datos.setearParametro("CP", modificar.CP);

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
