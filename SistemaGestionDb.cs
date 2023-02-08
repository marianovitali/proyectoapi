using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreEntregaProyectoFinal.Models
{
    internal class SistemaGestionDb
    {
        private string _connectionString { get => @"Server=DESKTOP-7ML91MM\SQLEXPRESS;Database=SistemaGestion;User Id=sa;Password=asdfmovie12;"; }

        public void MostrarMenu()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("1.Buscar Usuario por ID");
            Console.WriteLine("2.Traer productos");
            Console.WriteLine("3.Traer productos vendidos");
            Console.WriteLine("4.Traer Ventas");
            Console.WriteLine("5.Inicio de sesion");
            Console.WriteLine("==================================================\n");

            int opcion = int.Parse(Console.ReadLine());
            int idUsuario;
            switch (opcion)
            {
                case 1:
                    Console.Write("Escribir Id del Usuario: ");
                    idUsuario = int.Parse(Console.ReadLine());
                    MostrarUsuario(TraerUsuario(idUsuario));
                    break;
                case 2:
                    Console.Write("Escribir Id del Usuario que cargo los productos: ");
                    idUsuario = int.Parse(Console.ReadLine());
                    MostrarProductosCargados(TraerListaProductos(idUsuario));
                    break;
                case 3:
                    Console.Write("Escribir Id del Usuario para ver sus productos vendidos: ");
                    idUsuario = int.Parse(Console.ReadLine());
                    MostrarProductosVendidos(TraerProductosVendidos(idUsuario));
                    break;
            }
        }

        //1. Traer Usuario (recibe un int)
        public UsuarioModel TraerUsuario(long id)
        {
            UsuarioModel usuario = new UsuarioModel();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "SELECT Id, Nombre, Apellido, NombreUsuario, Contrasena, Mail FROM Usuario WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();

                id = dr.GetOrdinal("Id");
                int nombre = dr.GetOrdinal("Nombre");
                int apellido = dr.GetOrdinal("Apellido");
                int nombreUsuario = dr.GetOrdinal("NombreUsuario");
                int contrasena = dr.GetOrdinal("Contrasena");
                int mail = dr.GetOrdinal("Mail");

                while (dr.Read())
                {
                    usuario.Id = dr.GetInt64((int)id);
                    usuario.Nombre = dr.GetString(nombre);
                    usuario.Apellido = dr.GetString(apellido);
                    usuario.NombreUsuario = dr.GetString(nombreUsuario);
                    usuario.Contrasena = dr.GetString(contrasena);
                    usuario.Mail = dr.GetString(mail);
                }
                dr.Close();

            }
            return usuario;
        }
        //2. Traer Productos (recibe un id de usuario y devuelve una lista con todos los productos cargados por ese usuario)

        public List<ProductoModel> TraerListaProductos(long id)
        {
            List<ProductoModel> listaProductos = new List<ProductoModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "SELECT Id, Descripciones, Costo, PrecioVenta, Stock, IdUsuario FROM Producto WHERE IdUsuario = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();

                id = dr.GetOrdinal("Id");
                int descripciones = dr.GetOrdinal("Descripciones");
                int costo = dr.GetOrdinal("Costo");
                int precioVenta = dr.GetOrdinal("PrecioVenta");
                int stock = dr.GetOrdinal("Stock");
                int idUsuario = dr.GetOrdinal("IdUsuario");

                while (dr.Read())
                {
                    ProductoModel producto = new ProductoModel();
                    producto.Id = dr.GetInt64((int)id);
                    producto.Descripciones = dr.GetString(descripciones);
                    producto.Costo = dr.GetDecimal(costo);
                    producto.PrecioVenta = dr.GetDecimal(precioVenta);
                    producto.Stock = dr.GetInt32(stock);
                    producto.IdUsuario = dr.GetInt64((int)idUsuario);
                    listaProductos.Add(producto);
                }
                dr.Close();

            }
            return listaProductos;
        }

        //3. Traer Productos Vendidos (recibe un id de usuario y devuelve una lista de productos vendidos por ese usuario)

        public List<ProductoModel> TraerProductosVendidos(long idUsuario)
        {
            List<long> ListaIdProductos = new List<long>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "SELECT IdProducto FROM Venta INNER JOIN ProductoVendido ON Venta.Id = ProductoVendido.IdVenta WHERE IdUsuario = @idUsuario";

                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                SqlDataReader dr = cmd.ExecuteReader();

                //idUsuario = dr.GetOrdinal("IdUsuario");

                while (dr.Read())
                {   
                    //idUsuario = dr.GetInt64((int)idUsuario);
                    //ListaIdProductos.Add(idUsuario);
                    ListaIdProductos.Add(dr.GetInt64(0));


                }
                dr.Close();


            }
            List<ProductoModel> productos = new List<ProductoModel>();
            foreach (var id in ListaIdProductos)
            {


                ProductoModel prod = ObtenerProducto(id);
                productos.Add(prod);
            }

            return productos;
        }

        public ProductoModel ObtenerProducto(long id)
        {
            ProductoModel producto = new ProductoModel();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "SELECT * FROM Producto WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();

                id = dr.GetOrdinal("Id");
                int descripciones = dr.GetOrdinal("Descripciones");
                int costo = dr.GetOrdinal("Costo");
                int precioVenta = dr.GetOrdinal("PrecioVenta");
                int stock = dr.GetOrdinal("Stock");
                int idUsuario = dr.GetOrdinal("IdUsuario");

                while (dr.Read())
                {
                    producto.Id = dr.GetInt64((int)id);
                    producto.Descripciones = dr.GetString(descripciones);
                    producto.Costo = dr.GetDecimal(costo);
                    producto.PrecioVenta = dr.GetDecimal(precioVenta);
                    producto.Stock = dr.GetInt32(stock);
                    producto.IdUsuario = dr.GetInt64((int)idUsuario);
                }
                dr.Close();
            }
            return producto;
        }



        public void MostrarUsuario(UsuarioModel usuario)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("ID: {0}", usuario.Id);
            Console.WriteLine("Nombre: {0}", usuario.Nombre);
            Console.WriteLine("Apellido: {0}", usuario.Apellido);
            Console.WriteLine("NombreUsuario: {0}", usuario.NombreUsuario);
            Console.WriteLine("Contrasena: {0}", usuario.Contrasena);
            Console.WriteLine("Mail: {0}", usuario.Mail);
            Console.WriteLine("==================================================\n");
        }

        public void MostrarProductosCargados(List<ProductoModel> productos)
        {
            foreach (var item in productos)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine("ID: {0}", item.Id);
                Console.WriteLine("Descripciones: {0}", item.Descripciones);
                Console.WriteLine("Costo: {0}", item.Costo);
                Console.WriteLine("PrecioVenta: {0}", item.PrecioVenta);
                Console.WriteLine("Stock: {0}", item.Stock);
                Console.WriteLine("IdUsuario: {0}", item.IdUsuario);
                Console.WriteLine("==================================================\n");
            }
        }

        public void MostrarProductosVendidos(List<ProductoModel> productos)
        {
            foreach (var item in productos)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine("ID: {0}", item.Id);
                Console.WriteLine("Descripciones: {0}", item.Descripciones);
                Console.WriteLine("Costo: {0}", item.Costo);
                Console.WriteLine("PrecioVenta: {0}", item.PrecioVenta);
                Console.WriteLine("Stock: {0}", item.Stock);
                Console.WriteLine("IdUsuario: {0}", item.IdUsuario);
                Console.WriteLine("==================================================\n");
            }
        }
    }


}
