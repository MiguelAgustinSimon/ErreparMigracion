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
        MapperCliente mprClie = new MapperCliente();
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
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");

                request.AddHeader("authorization", this.tokenApi);

                

                var clicod = unCliente.idCliente.ToString().Trim();
                var cuit = unCliente.cuit.ToString().Trim();
                var email = unCliente.mailComercial.Trim().ToLower();
                var razonSocial = unCliente.razonSocial.Trim();
                var fechaAlta = unCliente.fechaAlta.ToString().Trim();
                var fechaAct = unCliente.fechaActualizacion.ToString().Trim();
                var estaActivo = true;
                var suspendido = false;
                if (unCliente.suscriptorActivo == "N")
                {
                    estaActivo = false;
                }
                if (unCliente.suspendido == "S")
                {
                    suspendido = true;
                }
                var pais = unCliente.pais.ToString().Trim();
                var prov = unCliente.provincia.ToString().Trim();
                var tipoSuscriptor = unCliente.tipoSuscriptor.ToString().Trim();
                var perIIBB = unCliente.perIIBB.ToString().Trim();

                request.AddBody(new
                {
                    clicod = clicod,
                    cuit = cuit,
                    email = email,
                    razon_social = razonSocial,
                    fecha_alta = fechaAlta,
                    fecha_actualizacion_tablas_intermedias = fechaAct,
                    activo = estaActivo,
                    suspendido = suspendido,
                    pais = pais,
                    tipo_suscriptorprovincia = prov,
                    tipo_suscriptor = tipoSuscriptor,
                    perIIBB = perIIBB
                });

                //request.AddBody(new
                //{
                //    clicod = "clicodtest73",
                //    cuit = "1123361273",
                //    email = "hernanpappatest573@gmail.com",
                //    razon_social = "TIENDA DE MASCOTAS73",
                //    fecha_alta = "1/6/2022 08:58:53",
                //    fecha_actualizacion_tablas_intermedias = "1/6/2022 08:58:53",
                //    activo = true,
                //    suspendido = false,
                //    pais = pais,
                //    tipo_suscriptorprovincia = prov,
                //    tipo_suscriptor = tipoSuscriptor,
                //    perIIBB = perIIBB
                //});


                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("createCustomerUserCorpCustomer OK - subscriber_id: " + unCliente.idCliente,true);
                    //Guardar en tabla Novedades!!!
                    await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Alta", "Realizado", response.Content);

                    return true;
                }
                else
                {
                    //Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló createCustomerUserCorpCustomer: ERROR - subscriber_id: " + unCliente.idCliente + " / " + response.StatusDescription + " - Respuesta: " + response.Content,false);
                    //Guardar en tabla Novedades!!!
                    await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Alta", "Pendiente", response.Content);
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
        public async Task<Boolean> updateSubscriberCorpCustomer(Cliente unCliente)
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                var client = new RestClient(this.headerApi);
                var request = new RestRequest(this.updateCustomer, Method.Put);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");

                request.AddHeader("authorization", this.tokenApi);

                var clicod = unCliente.idCliente.ToString().Trim();
                var cuit = unCliente.cuit.ToString().Trim();
                var email = unCliente.mailComercial.Trim().ToLower();
                var razonSocial = unCliente.razonSocial.Trim();
                var fechaAlta = unCliente.fechaAlta.ToString().Trim();
                var fechaAct = unCliente.fechaActualizacion.ToString().Trim();
                var estaActivo = true;
                var suspendido = false;
                if (unCliente.suscriptorActivo == "N")
                {
                    estaActivo = false;
                }
                if (unCliente.suspendido == "S")
                {
                    suspendido = true;
                }
                var pais = unCliente.pais.ToString().Trim();
                var prov = unCliente.provincia.ToString().Trim();
                var tipoSuscriptor = unCliente.tipoSuscriptor.ToString().Trim();
                var perIIBB = unCliente.perIIBB.ToString().Trim();

                request.AddBody(new
                {
                    clicod = clicod,
                    cuit = cuit,
                    email = email,
                    razon_social = razonSocial,
                    fecha_alta = fechaAlta,
                    fecha_actualizacion_tablas_intermedias = fechaAct,
                    activo = estaActivo,
                    suspendido = suspendido,
                    pais = pais,
                    tipo_suscriptorprovincia = prov,
                    tipo_suscriptor = tipoSuscriptor,
                    perIIBB = perIIBB
                });

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Console.WriteLine(response.Content);
                    await mpLog.agregarLogSerilog("updateSubscriberCorpCustomer OK - subscriber_id: " + unCliente.idCliente, true);
                    //Guardar en tabla Novedades!!!
                    await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Modificacion", "Realizado", response.Content);

                    return true;
                }
                else
                {
                    //Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló updateSubscriberCorpCustomer: ERROR - subscriber_id: " + unCliente.idCliente + " / " + response.StatusDescription + " - Respuesta: " + response.Content, false);
                    //Guardar en tabla Novedades!!!
                    await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Modificacion", "Pendiente", response.Content);
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
                    await mpLog.agregarLogSerilog("createProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto, true);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló createProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription, false);
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


        //UPDATE SUSCRIPCION
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
                    await mpLog.agregarLogSerilog("updateProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto, true);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló updateProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription, false);
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
                    await mpLog.agregarLogSerilog("deleteProductCommProduct OK - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto, true);
                    return true;
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                    await mpLog.agregarLogSerilog("Falló deleteProductCommProduct - Cliente: " + unaSuscripcion.idCliente + ", Producto: " + unaSuscripcion.idProducto + " / " + response.StatusDescription, false);
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
