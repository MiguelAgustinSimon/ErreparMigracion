
using System.Data.SqlClient;
using System.Configuration;

namespace CapaDatos
{
    public class Connection
    {
        public static SqlConnection con;
        public static SqlConnection ConnectionObj()
        {
            string mString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            if (con == null)
            {
                con = new SqlConnection(mString);
            }
            
            //string mString = "Data Source=srv-devi.errepar.com\\qa; Initial Catalog=ErpCloudData; User Id=nicolas.laugas; Password=estoyCansado123";

            return con;

        }


        public string[] ObtenerTablasClientes()
        {
            try
            {
                var tablaOrigen = ConfigurationManager.AppSettings["TablaOrigenDC"];
                var tablaDestino = ConfigurationManager.AppSettings["TablaDestinoDC"];
                string[] tablas = { tablaOrigen, tablaDestino };
                return tablas;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string[] ObtenerTablasSuscripciones()
        {
            try
            {
                var tablaOrigen = ConfigurationManager.AppSettings["TablaOrigenSA"];
                var tablaDestino = ConfigurationManager.AppSettings["TablaDestinoSA"];
                string[] tablas = { tablaOrigen, tablaDestino };
                return tablas;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public string[] ObtenerTablasNovedades()
        {
            try
            {
                var tablaOrigen = ConfigurationManager.AppSettings["TablaSuscriptor"];
                var tablaDestino = ConfigurationManager.AppSettings["TablaSuscripcion"];
                string[] tablas = { tablaOrigen, tablaDestino };
                return tablas;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

    }
}
