using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using CapaNegocio;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CapaDatos
{
    public class Endpoint
    {

        public string headerApi;
        public string getSubscriber;
        public string postSubscriber;
        public string updateSubscriber;
        string[] arrApi;

        //https://www.luisllamas.es/consumir-un-api-rest-en-c-facilmente-con-restsharp/
        //https://pokeapi.co/
        public Endpoint()
        {

            Connection mConeccion = new Connection();

            //Aca obtengo el nombre de las tablas que esta en appsettings.json
            this.arrApi = mConeccion.ObtenerRutasEndpoint();
            

            foreach (var item in this.arrApi.Select((elemento, i) => new { i, elemento }))
            {
                switch (item.i)
                {
                    case 0:
                        this.headerApi = item.elemento;
                        break;
                    case 1:
                        this.getSubscriber = item.elemento;
                        break;
                    case 2:
                        this.postSubscriber = item.elemento;
                        break;
                    case 3:
                        this.updateSubscriber = item.elemento;
                        break;
                }
            }
        }
            public async Task agregarLog(string texto)
            {

            int cont = 0;

            string ruta = @"..\\..\\..\\Logs\\log.txt";

            if (System.IO.Path.GetExtension(ruta).ToLower() == ".txt")
            {
                //Determina si existe el archivo.
                if (File.Exists(ruta))
                {
                    //El archivo existe, añadimos un log
                    string[] lineas = File.ReadAllLines(ruta);
                    List<string> lista = new List<string>(lineas.ToList());
                    lista.Add("Evento sucedido a las: " + DateTime.Now);
                    lista.Add(texto);
                    lista.Add("____________________________________________________");

                    File.WriteAllLines(ruta, lista);
                }
                else
                {
                    //El archivo no existe, lo creamos
                    StreamWriter OurStream;
                    OurStream = File.CreateText(ruta);
                    OurStream.WriteLine("Evento sucedido a las: " + DateTime.Now);
                    OurStream.WriteLine(texto);
                    OurStream.WriteLine("____________________________________________________");
                    OurStream.Close();
                }
            }
            else
            {
                //El archivo no es un PDF, continua sin realizar acción.
                Console.WriteLine("El archivo " + ruta + " no es un .txt.");
            }

             }
        public void GetItems()
        {
            try
            {
                var client = new RestClient("https://pokeapi.co/api/v2/");
                var request = new RestRequest("pokemon/ditto", Method.Get);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                   Console.WriteLine(response.Content);
                }
                else {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

        public async Task<string> getDatosSuscriptor(int? unId)
        {
            try
            {
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.getSubscriber + unId, Method.Get);

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Console.WriteLine(response.Content);
                    var content = response.Content;
                    SuscriptorEndpoint? enp =JsonSerializer.Deserialize<SuscriptorEndpoint>(content);
                    //Console.WriteLine($"subscriber_id: {enp?.subscriber_id}");
                    return enp?.subscriber_id;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        //ALTA SUSCRIPTOR
        public async Task<Boolean> createSubscriberCorpEntities(Cliente clie)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.postSubscriber, Method.Post);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("clicod", clie.idCliente.ToString());
                request.AddParameter("subscriber_name", clie.razonSocial);
               // request.AddParameter("organization_cuit", clie.cuit);
                request.AddParameter("organization_cuit", "30582622945");
                //request.AddParameter("organization_legal_name", clie.razonSocial);
                request.AddParameter("organization_legal_name", "ERREPAR PLUS");

                if (clie.suspendido == "S")
                {
                    request.AddParameter("subscriber_status_id", 3);
                }
                else
                {
                    if (clie.suscriptorActivo == "N")
                    {
                        request.AddParameter("subscriber_status_id", 2);
                    }
                    else
                    {
                        request.AddParameter("subscriber_status_id", 1); //ACTIVO
                    }
                }
                request.AddParameter("subscriber_max_user_count", 5);//esto puede cambiar segun los productos que tenga
                request.AddParameter("creation_date", clie.timeStamp.ToString());
                request.AddParameter("creation_user", "INTEGRACION-SG");

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine(response.Content);
                    await this.agregarLog("Alta Suscriptor OK - Suscriptor: " + clie.idCliente.ToString());
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await this.agregarLog("Falló Alta Suscriptor: ERROR - Suscriptor: " + clie.idCliente.ToString() + response.StatusDescription);
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
        public async Task<Boolean> actualizarDatosSuscriptor(Cliente clie)
        {
            try
            {
                string suscriberId = await this.getDatosSuscriptor(clie.idCliente);

                var client = new RestClient(this.headerApi);
                //var request = new RestRequest($"updateSubscriberCorpEntities/{suscriberId}", Method.Put);
                var request = new RestRequest(this.updateSubscriber + suscriberId, Method.Put);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("clicod", clie.idCliente.ToString());
                request.AddParameter("subscriber_name", clie.razonSocial);
                // request.AddParameter("organization_cuit", clie.cuit);
                request.AddParameter("organization_cuit", "30582622945");
                //request.AddParameter("organization_legal_name", clie.razonSocial);
                request.AddParameter("organization_legal_name", "ERREPAR PLUS");

                if (clie.suspendido == "S")
                {
                    request.AddParameter("subscriber_status_id", 3);
                }
                else
                {
                    if (clie.suscriptorActivo == "N")
                    {
                        request.AddParameter("subscriber_status_id", 2);
                    }
                    else
                    {
                        request.AddParameter("subscriber_status_id", 1); //ACTIVO
                    }
                }
                request.AddParameter("subscriber_max_user_count", 5);//esto puede cambiar segun los productos que tenga
                request.AddParameter("modification_date", clie.timeStamp.ToString());
                request.AddParameter("modification_user", "INTEGRACION-SG");

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                    await this.agregarLog("Actualizacion Suscriptor OK - Suscriptor: " + clie.idCliente.ToString());
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await this.agregarLog("Falló Actualizacion Suscriptor: ERROR - Suscriptor: " + clie.idCliente.ToString() + response.StatusDescription);
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

      
        private void DeleteItem(int id)
        {
            try
            {
                var client = new RestClient("http://localhost:8080");
                var request = new RestRequest($"items/{id}", Method.Delete);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

    }
}
