using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using CapaNegocio;

namespace CapaDatos
{
    public class Orquestador
    {
        public string headerApi;
        public string tokenApi;
        public string createCustomer;
        public string updateCustomer;
        public string createSuscripcion;
        public string updateSuscripcion;
        public string deleteSuscripcion;
        MapperLog mpLog = new MapperLog();
        public Orquestador()
        {
          
            Connection mConeccion = new Connection();

            //Aca obtengo el nombre de los Endpoints que estan en appsettings.json
            this.tokenApi = mConeccion.ObtenerTokenApi();
            this.headerApi = mConeccion.ObtenerHeaderApi();
            this.createCustomer = mConeccion.ObtenerEndpointAltaSuscriptor();
            this.updateCustomer = mConeccion.ObtenerEndpointUpdateSuscriptor();
            this.createSuscripcion = mConeccion.ObtenerEndpointCreateSuscripcion();
            this.updateSuscripcion = mConeccion.ObtenerEndpointUpdateSuscripcion();
            this.deleteSuscripcion = mConeccion.ObtenerEndpointDeleteSuscripcion();
           


        }

        //ALTA DE SUSCRIPTOR
        public async Task<Boolean> createCustomerUserCorpCustomer(Cliente unCliente)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.createCustomer, Method.Post);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);

                //request.AddParameter("clicod", unCliente.idCliente.ToString().Trim());
                //request.AddParameter("cuit", unCliente.cuit.ToString().Trim());
                //request.AddParameter("email", unCliente.mailComercial.Trim());
                //request.AddParameter("razon_social", unCliente.razonSocial.Trim());
                //request.AddParameter("fecha_alta", unCliente.fechaAlta.ToString().Trim());
                //request.AddParameter("fecha_actualizacion_tablas_intermedias", unCliente.fechaActualizacion.ToString().Trim());

                //if (unCliente.suscriptorActivo == "S")
                //{
                //    request.AddParameter("activo", true);
                //}
                //else
                //{
                //    request.AddParameter("activo", false);
                //}

                //if (unCliente.suspendido == "S")
                //{
                //    request.AddParameter("suspendido", true);
                //}
                //else
                //{
                //    request.AddParameter("suspendido", false);
                //}
                //request.AddParameter("pais", unCliente.pais.Trim());
                //request.AddParameter("provincia", unCliente.provincia.Trim());
                //request.AddParameter("tipo_suscriptor", unCliente.tipoSuscriptor.Trim());
                //request.AddParameter("perIIBB", unCliente.perIIBB.ToString().Trim());


                request.AddParameter("clicod", "444957");
                request.AddParameter("cuit", "30693291221");
                request.AddParameter("email", "solange.janin@samconsultt.com");
                request.AddParameter("razon_social", "ERREPAR");
                request.AddParameter("fecha_alta", "2022-06-22T20:10:08.087Z");
                request.AddParameter("fecha_actualizacion_tablas_intermedias", "2022-06-22T20:10:08.087Z");
                request.AddParameter("activo", true);
                request.AddParameter("suspendido", false);
        

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("createCustomerUserCorpCustomer OK - subscriber_id: " + unCliente.idCliente);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló createCustomerUserCorpCustomer: ERROR - subscriber_id: " + unCliente.idCliente + " / " + response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }


        //UPDATE SUSCRIPTOR
        public async Task<Boolean> updateSucriberCorpCustomer(Cliente unCliente)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.updateCustomer, Method.Put);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);


                request.AddParameter("clicod", unCliente.idCliente.ToString().Trim());
                if(unCliente.cuit!=null)
                {
                    request.AddParameter("cuit", unCliente.cuit.ToString().Trim());
                }

                if (unCliente.mailComercial != null)
                {
                    request.AddParameter("email", unCliente.mailComercial.ToString().Trim());
                }

                if (unCliente.razonSocial != null)
                {
                    request.AddParameter("razon_social", unCliente.razonSocial.ToString().Trim());
                }

                if (unCliente.suscriptorActivo != null)
                {
                    if (unCliente.suscriptorActivo == "S")
                    {
                        request.AddParameter("activo", true);
                    }
                    else
                    {
                        request.AddParameter("activo", false);
                    }
                }

                if (unCliente.suspendido != null)
                {
                    if (unCliente.suspendido == "S")
                    {
                        request.AddParameter("suspendido", true);
                    }
                    else
                    {
                        request.AddParameter("suspendido", false);
                    }
                }

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("createCustomerUserCorpCustomer OK - subscriber_id: " + unCliente.idCliente);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló createCustomerUserCorpCustomer: ERROR - subscriber_id: " + unCliente.idCliente + " / " + response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }


        //ALTA DE SUSCRIPCION
        public async Task<Boolean> createProductCommProduct(Suscripcion unaSuscripcion)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.createSuscripcion, Method.Post);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);

                request.AddParameter("clicod", unaSuscripcion.idCliente.ToString());
                request.AddParameter("idProducto", unaSuscripcion.idProducto.ToString());
                request.AddParameter("tema", unaSuscripcion.tema);
                request.AddParameter("vencimiento", unaSuscripcion.vencimiento.ToString());
                request.AddParameter("idEjecutivo", unaSuscripcion.idEjecutivo.ToString());


                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("createProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló createProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }


        //UPDATE SUSCRIPTOR
        public async Task<Boolean> updateProductCommProduct(Suscripcion unaSuscripcion)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.updateSuscripcion, Method.Put);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);

                request.AddParameter("clicod", unaSuscripcion.idCliente.ToString());
                request.AddParameter("idProducto", unaSuscripcion.idProducto.ToString());
                request.AddParameter("tema", unaSuscripcion.tema);
                request.AddParameter("vencimiento", unaSuscripcion.vencimiento.ToString());
                request.AddParameter("idEjecutivo", unaSuscripcion.idEjecutivo.ToString());


                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("updateProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló updateProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        //DELETE SUSCRIPTOR
        public async Task<Boolean> deleteProductCommProduct(Suscripcion unaSuscripcion)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.updateSuscripcion, Method.Delete);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);

                request.AddParameter("clicod", unaSuscripcion.idCliente.ToString());
                request.AddParameter("idProducto", unaSuscripcion.idProducto.ToString());
                request.AddParameter("tema", unaSuscripcion.tema);
                request.AddParameter("vencimiento", unaSuscripcion.vencimiento.ToString());
                request.AddParameter("idEjecutivo", unaSuscripcion.idEjecutivo.ToString());


                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("deleteProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló deleteProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
    }
}
