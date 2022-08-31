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
        public string headerApiJwt;
        public string tokenApi { get; set; }
        public string jwt2 { get; set; }
        public string apiKey;
        public string getCredencialJwt;
        public string createCustomer;
        public string updateCustomer;
        public string createSuscripcion;
        public string updateSuscripcion;
        public string deleteSuscripcion;
        MapperLog mpLog = new MapperLog();
        MapperCliente mprClie = new MapperCliente();
        MapperSuscripcion mprSuscripcion = new MapperSuscripcion();

        public Orquestador()
        {
          
            Connection mConeccion = new Connection();

            //Aca obtengo el nombre de los Endpoints que estan en AppConfig
           
            this.headerApiJwt = mConeccion.ObtenerHeaderApiJwt();
            this.apiKey = mConeccion.ObtenerApiKey();
            this.getCredencialJwt = mConeccion.ObtenerGetJobCredentialsEAuth();

            this.headerApi = mConeccion.ObtenerHeaderApi();
            this.createCustomer = mConeccion.ObtenerEndpointAltaSuscriptor();
            this.updateCustomer = mConeccion.ObtenerEndpointUpdateSuscriptor();
            this.createSuscripcion = mConeccion.ObtenerEndpointCreateSuscripcion();
            this.updateSuscripcion = mConeccion.ObtenerEndpointUpdateSuscripcion();
            this.deleteSuscripcion = mConeccion.ObtenerEndpointDeleteSuscripcion();
            //this.tokenApi = this.getJobCredentialsEAuth().GetAwaiter().GetResult();
        }

        //OBTENER JWT
        public async Task<String> getJobCredentialsEAuth()
        {
            try
            {
                //headerApi, postSubscriber y demas estan en appConfig
                string jwtOriginal = null;
                var client = new RestClient(this.headerApiJwt);
                var request = new RestRequest(this.getCredencialJwt, Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");

                var apiKey = this.apiKey;

                request.AddBody(new
                {
                    key = apiKey
                });

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Found)
                {
                    Orquestador? unJson= JsonSerializer.Deserialize<Orquestador>(response.Content);
                    jwtOriginal = unJson?.jwt2;
                }
                else
                {
                    await mpLog.agregarLogSerilog($"Falló getJobCredentialsEAuth: ERROR {response.StatusDescription}  - Respuesta: {response.Content}", false);
                }
                return jwtOriginal;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        //ALTA DE SUSCRIPTOR
        public async Task<Boolean> createCustomerUserCorpCustomer(Cliente unCliente)
        {
            try
            {
                bool respuesta = false;

                this.tokenApi = await getJobCredentialsEAuth();
                if (this.tokenApi != null)
                {
                    //headerApi, postSubscriber y demas estan en appConfig
                    var client = new RestClient(this.headerApi);
                    var request = new RestRequest(this.createCustomer, Method.Post);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");

                    request.AddHeader("authorization", "Bearer "+this.tokenApi);

                    var clicod = unCliente.idCliente;
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
                        await mpLog.agregarLogSerilog("createCustomerUserCorpCustomer OK - subscriber_id: " + unCliente.idCliente, true);
                        //Guardar en tabla Novedades!!!
                        await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Alta", "Realizado", response.Content);

                        respuesta=true;
                    }
                    else
                    {
                        await mpLog.agregarLogSerilog($"Falló createCustomerUserCorpCustomer: ERROR - subscriber_id: {unCliente.idCliente}  / {response.StatusDescription} - Respuesta: {response.Content}", false);
                        //Guardar en tabla Novedades!!!
                        await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Alta", "Pendiente", response.Content);
                    }
                }
                return respuesta;
                
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
                bool respuesta = false;

                this.tokenApi = await getJobCredentialsEAuth();
                if (this.tokenApi != null)
                {
                    //headerApi, postSubscriber y demas estan en appConfig
                    var client = new RestClient(this.headerApi);
                    var request = new RestRequest(this.updateCustomer, Method.Put);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");

                    request.AddHeader("authorization", "Bearer " + this.tokenApi);

                      var clicod = unCliente.idCliente;
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

                        respuesta=true;
                    }
                    else
                    {
                        //Console.WriteLine(response.StatusDescription);
                        await mpLog.agregarLogSerilog($"Falló updateSubscriberCorpCustomer: ERROR - subscriber_id: {unCliente.idCliente}  / {response.StatusDescription} - Respuesta: {response.Content}", false);
                        //Guardar en tabla Novedades!!!
                        await mprClie.ActualizarNovedadesSuscriptor(unCliente, "Modificacion", "Pendiente", response.Content);
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }


        //ALTA DE SUSCRIPCION
        public async Task<Boolean> startSubscriptionSubscriberCorpCustomer(Suscripcion susc)
        {
            try
            {
                bool respuesta = false;
                this.tokenApi = await getJobCredentialsEAuth();
                if (this.tokenApi != null)
                {
                    //headerApi, postSubscriber y demas estan en appConfig
                    var client = new RestClient(this.headerApi);
                    var request = new RestRequest(this.createSuscripcion, Method.Post);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");

                    request.AddHeader("authorization", "Bearer " + this.tokenApi);

                    var clicod = susc.idCliente;
                    var producto = susc.idProducto.ToString().Trim();
                    var tema = susc.tema.ToString().Trim();
                    var fecha_vigencia = DateTime.Now.ToString("yyyy-MM-dd");
                    var vencimiento = susc.vencimiento.ToString().Trim();
                    var ejecutivo = susc.idEjecutivo.ToString().Trim();


                    request.AddBody(new
                    {
                        clicod = clicod,
                        id_producto = producto,
                        tema = tema,
                        fecha_vigencia= fecha_vigencia,
                        fecha_vencimiento = vencimiento,
                        ejecutivo = ejecutivo
                    });

                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await mpLog.agregarLogSerilog($"createProduct OK - Cliente: {susc.idCliente} Producto: {susc.idProducto}", true);

                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Alta", "Realizado", response.Content);

                        respuesta=true;
                    }
                    else
                    {
                        await mpLog.agregarLogSerilog($"Falló createProduct: ERROR - Cliente: {susc.idCliente}  / Producto: {susc.idProducto} / {response.StatusDescription} - Respuesta: {response.Content}", false);
                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Alta", "Pendiente", response.Content);
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }


        //UPDATE SUSCRIPCION
        public async Task<Boolean> updateProductCommProduct(Suscripcion susc)
        {
            try
            {
                bool respuesta = false;
                this.tokenApi = await this.getJobCredentialsEAuth();
                if (this.tokenApi != null)
                {
                    //headerApi, postSubscriber y demas estan en appConfig
                    var client = new RestClient(this.headerApi);
                    var request = new RestRequest(this.updateSuscripcion, Method.Put);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");

                    request.AddHeader("authorization", "Bearer " + this.tokenApi);


                    var clicod = susc.idCliente;
                    var producto = susc.idProducto.ToString().Trim();
                    var tema = susc.tema.ToString().Trim();
                    var vencimiento = susc.vencimiento.ToString().Trim();
                    var ejecutivo = susc.idEjecutivo.ToString().Trim();


                    request.AddBody(new
                    {
                        clicod = clicod,
                        producto = producto,
                        tema = tema,
                        vencimiento = vencimiento,
                        ejecutivo = ejecutivo
                    });

                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await mpLog.agregarLogSerilog($"createProduct OK - Cliente: {susc.idCliente} Producto: {susc.idProducto}", true);

                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Alta", "Realizado", response.Content);

                        respuesta = true;
                    }
                    else
                    {
                        await mpLog.agregarLogSerilog($"Falló createProduct: ERROR - Cliente: {susc.idCliente}  / Producto: {susc.idProducto} / {response.StatusDescription} - Respuesta: {response.Content}", false);
                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Alta", "Pendiente", response.Content);
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        //DELETE SUSCRIPTOR
        public async Task<Boolean> deleteProductCommProduct(Suscripcion susc)
        {
            try
            {
                bool respuesta = false;
                this.tokenApi = await getJobCredentialsEAuth();
                if (this.tokenApi != null)
                {
                    //headerApi, postSubscriber y demas estan en appConfig
                    var client = new RestClient(this.headerApi);
                    var request = new RestRequest(this.updateSuscripcion, Method.Delete);
                    request.RequestFormat = RestSharp.DataFormat.Json;
                    request.AddHeader("Content-Type", "application/json");

                    request.AddHeader("authorization", "Bearer " + this.tokenApi);

                    var clicod = susc.idCliente;
                    var producto = susc.idProducto.ToString().Trim();
                    var tema = susc.tema.ToString().Trim();
                    var vencimiento = susc.vencimiento.ToString().Trim();
                    var ejecutivo = susc.idEjecutivo.ToString().Trim();


                    request.AddBody(new
                    {
                        clicod = clicod,
                        producto = producto,
                        tema = tema,
                        vencimiento = vencimiento,
                        ejecutivo = ejecutivo
                    });

                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await mpLog.agregarLogSerilog($"createProduct OK - Cliente: {susc.idCliente} Producto: {susc.idProducto}", true);

                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Eliminacion", "Realizado", response.Content);

                        respuesta = true;
                    }
                    else
                    {
                        await mpLog.agregarLogSerilog($"Falló createProduct: ERROR - Cliente: {susc.idCliente}  / Producto: {susc.idProducto} / {response.StatusDescription} - Respuesta: {response.Content}", false);
                        //Guardar en tabla Novedades!!!
                        await mprSuscripcion.ActualizarNovedadesSuscripcion(susc, "Eliminacion", "Pendiente", response.Content);
                    }
                }
                return respuesta;
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
