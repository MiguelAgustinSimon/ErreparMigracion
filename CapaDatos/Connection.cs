
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

        public string ObtenerTokenApi()
        {
            try
            {
                var tokenApi = ConfigurationManager.AppSettings["TokenApi"];
               
                return tokenApi;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerHeaderApi()
        {
            try
            {
                var HeaderApi = ConfigurationManager.AppSettings["HeaderApi"];

                return HeaderApi;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerHeaderApiJwt()
        {
            try
            {
                var HeaderApiJwt = ConfigurationManager.AppSettings["HeaderApiJwt"];

                return HeaderApiJwt;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerApiKey()
        {
            try
            {
                var apikey = ConfigurationManager.AppSettings["apikey"];

                return apikey;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerGetJobCredentialsEAuth()
        {
            try
            {
                var getJobCredentialsEAuth = ConfigurationManager.AppSettings["getJobCredentialsEAuth"];

                return getJobCredentialsEAuth;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerEndpointAltaSuscriptor()
        {
            try
            {
                var createCustomerUserCorpCustomer = ConfigurationManager.AppSettings["createCustomerUserCorpCustomer"];

                return createCustomerUserCorpCustomer;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerEndpointUpdateSuscriptor()
        {
            try
            {
                var updateSubscriberCorpCustomer = ConfigurationManager.AppSettings["updateSubscriberCorpCustomer"];

                return updateSubscriberCorpCustomer;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerEndpointCreateSuscripcion()
        {
            try
            {
                var createProduct = ConfigurationManager.AppSettings["createProduct"];

                return createProduct;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerEndpointUpdateSuscripcion()
        {
            try
            {
                var updateProduct = ConfigurationManager.AppSettings["updateProduct"];

                return updateProduct;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerEndpointDeleteSuscripcion()
        {
            try
            {
                var deleteProduct = ConfigurationManager.AppSettings["deleteProduct"];

                return deleteProduct;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public string ObtenerRutaSerilog()
        {
            try
            {
                var rutaSerilog = ConfigurationManager.AppSettings["rutaSerilog"];

                return rutaSerilog;
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
