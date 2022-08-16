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
        MapperLog mpLog = new MapperLog();
        public Orquestador()
        {
          
            Connection mConeccion = new Connection();

            //Aca obtengo el nombre de los Endpoints que estan en appsettings.json
            this.tokenApi = mConeccion.ObtenerTokenApi();
            this.headerApi = mConeccion.ObtenerHeaderApi();
            this.createCustomer = mConeccion.ObtenerEndpointAltaSuscriptor();
           


        }
        public async Task<Boolean> createCustomerUserCorpCustomer(Cliente unCliente)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.createCustomer, Method.Post);
                request.RequestFormat = DataFormat.Json;

                request.AddHeader("authorization", this.tokenApi);

                request.AddParameter("clicod", unCliente.idCliente.ToString().Trim());
                request.AddParameter("cuit", unCliente.cuit.ToString().Trim());
                request.AddParameter("email", unCliente.mailComercial.Trim());
                request.AddParameter("razon_social", unCliente.razonSocial.Trim());
                request.AddParameter("fecha_alta", unCliente.fechaAlta.ToString().Trim());
                request.AddParameter("fecha_actualizacion_tablas_intermedias", unCliente.fechaActualizacion.ToString().Trim());
                request.AddParameter("activo", unCliente.suscriptorActivo);
                request.AddParameter("suspendido", unCliente.suspendido);
                request.AddParameter("pais", unCliente.pais.Trim());
                request.AddParameter("provincia", unCliente.provincia.Trim());
                request.AddParameter("tipo_suscriptor", unCliente.tipoSuscriptor.Trim());
                request.AddParameter("perIIBB", unCliente.perIIBB.ToString().Trim());


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
                    await mpLog.agregarLogSerilog("Falló createCustomerUserCorpCustomer: ERROR - subscriber_id: " + unCliente.idCliente + response.StatusDescription);
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
