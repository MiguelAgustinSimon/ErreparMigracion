//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RestSharp;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Globalization;
//using CapaNegocio;
//using System.Text.Json;
//using System.Text.Json.Serialization;


//namespace CapaDatos
//{
//    public class Endpoint
//    {

//        public string tokenApi;
//        public string tokenApiOrganization;
//        //atributos suscriptor
//        public string headerApi;
//        public string getSubscriber;
//        public string postSubscriber;
//        public string updateSubscriber;

//        //atributos suscripcion
//        public string headerApiSuscripcion;
//        public string postProduct;
//        public string updateProduct;
//        public string getProductosPorSuscriptor;
//        string[] arrApiSuscriptor;
//        string[] arrApiSuscripcion;
//        MapperLog mpLog = new MapperLog();

//        //https://www.luisllamas.es/consumir-un-api-rest-en-c-facilmente-con-restsharp/
//        //https://pokeapi.co/
//        public Endpoint()
//        {

//            Connection mConeccion = new Connection();

//            //Aca obtengo el nombre de los Endpoints que estan en appsettings.json
//            this.arrApiSuscriptor = mConeccion.ObtenerEndpointsSuscriptor();
//            this.arrApiSuscripcion = mConeccion.ObtenerEndpointsSuscripcion();
//            this.tokenApi = mConeccion.ObtenerTokenApi();
//            this.tokenApiOrganization = mConeccion.ObtenerTokenApiOrganizacion();


//            foreach (var item in this.arrApiSuscriptor.Select((elemento, i) => new { i, elemento }))
//            {
//                switch (item.i)
//                {
//                    case 0:
//                        this.headerApi = item.elemento;
//                        break;
//                    case 1:
//                        this.getSubscriber = item.elemento;
//                        break;
//                    case 2:
//                        this.postSubscriber = item.elemento;
//                        break;
//                    case 3:
//                        this.updateSubscriber = item.elemento;
//                        break;
//                }
//            }
//            foreach (var item in this.arrApiSuscripcion.Select((elemento, i) => new { i, elemento }))
//            {
//                switch (item.i)
//                {
//                    case 0:
//                        this.headerApiSuscripcion = item.elemento;
//                        break;
//                    case 1:
//                        this.postProduct = item.elemento;
//                        break;
//                    case 2:
//                        this.updateProduct = item.elemento;
//                        break;
//                    case 3:
//                        this.getProductosPorSuscriptor = item.elemento;
//                        break;
//                }
//            }
//        }
//        public async Task agregarLog(string texto)
//        {

//            int cont = 0;

//            string ruta = @"..\\..\\..\\Logs\\log.txt";

//            if (System.IO.Path.GetExtension(ruta).ToLower() == ".txt")
//            {
//                //Determina si existe el archivo.
//                if (File.Exists(ruta))
//                {
//                    //El archivo existe, añadimos un log
//                    string[] lineas = File.ReadAllLines(ruta);
//                    List<string> lista = new List<string>(lineas.ToList());
//                    lista.Add("Evento sucedido a las: " + DateTime.Now);
//                    lista.Add(texto);
//                    lista.Add("____________________________________________________");

//                    File.WriteAllLines(ruta, lista);
//                }
//                else
//                {
//                    //El archivo no existe, lo creamos
//                    StreamWriter OurStream;
//                    OurStream = File.CreateText(ruta);
//                    OurStream.WriteLine("Evento sucedido a las: " + DateTime.Now);
//                    OurStream.WriteLine(texto);
//                    OurStream.WriteLine("____________________________________________________");
//                    OurStream.Close();
//                }
//            }
//            else
//            {
//                //El archivo no es un PDF, continua sin realizar acción.
//                Console.WriteLine("El archivo " + ruta + " no es un .txt.");
//            }

//        }
//        public void GetItems()
//        {
//            try
//            {
//                var client = new RestClient("https://pokeapi.co/api/v2/");
//                var request = new RestRequest("pokemon/ditto", Method.Get);
//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//            }

//        }

//        //---------------------------------------------------- S U S C R I P T O R ----------------------------------------------------
//        public async Task<string> verificarLoginAccount(string? unMail)
//        {
//            try
//            {
//                var client = new RestClient("https://accounts.errepar.com/login/api/");
//                var request = new RestRequest("loginAccountByemail" + "?email=" + unMail, Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);

//                    return enp?.subscriber_id;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }

//        public async Task<Boolean> assignUserSubscriberCorpEntity(string subscriber_id, string user_id)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient(this.headerApi);
//                var request = new RestRequest("assignUserSubscriberCorpEntity", Method.Post);
//                request.RequestFormat = DataFormat.Json;

//                request.AddHeader("Authentication", this.tokenApi);


//                request.AddParameter("subscriber_id", subscriber_id);
//                request.AddParameter("user_id", user_id);
//                request.AddParameter("creation_user", "SG");
//                request.AddParameter("creation_date", DateTime.Now.ToString("yyyy-MM-dd"));


//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("assignUserSubscriberCorpEntity OK - subscriber_id: " + subscriber_id);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló assignUserSubscriberCorpEntity: ERROR - subscriber_id: " + subscriber_id + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Cliente> getDatosSuscriptor(int? unId)
//        {
//            try
//            {
//                var client = new RestClient(this.headerApi);
//                var request = new RestRequest(this.getSubscriber + "?clicod=" + unId, Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    Cliente? clie = JsonSerializer.Deserialize<Cliente>(content);
//                    //Console.WriteLine($"subscriber_id: {enp?.subscriber_id}");
//                    return clie;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }

//        public async Task<string> getUsersSubscriberCorpEntities(int? unId)
//        {
//            try
//            {
//                var client = new RestClient(this.headerApi);
//                var request = new RestRequest("getUsersSubscriberCorpEntities/" + unId + "?offset=0&limit=9999", Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    Object? clie = JsonSerializer.Deserialize<Object>(content);
//                    //Console.WriteLine($"subscriber_id: {enp?.subscriber_id}");
//                    //return clie.login_account.email; //VER COMO RETORNAR EMAIL
//                    return "aaa@gmail.com";
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }

//        //ALTA SUSCRIPTOR
//        public async Task<Boolean> createSubscriberCorpEntities(Cliente clie)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient(this.headerApi);
//                var request = new RestRequest(this.postSubscriber, Method.Post);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);

//                //traigo el UUID del Cliente
//                Cliente unCliente = await this.getDatosSuscriptor(clie.idCliente);

//                //https://accounts.errepar.com/organization-api/api-docs/#/Organization/createOrganizationCorpEntities
//                //paso 1 -> veo si existe la organizacion
//                var rtaOrg = await this.getByCuitOrganizationCorpEntities(clie.cuit);
//                if (rtaOrg == false)
//                {
//                    //paso 2 creo la organizacion
//                    await this.createOrganizationCorpEntities(clie);
//                }

//                //paso 3 verificar si existe cta Login
//                var rtaLogin = await this.verificarLoginAccount(clie.mailComercial);
//                if (rtaLogin == "false")
//                {

//                    if (unCliente.subscriber_id != null)
//                    {
//                        //paso 4 si no exista cta login, crearla: createLoginAccountCorpEntities (login) y createUserBaseAccountEAuth(cognito)
//                        await this.createLoginAccountCorpEntities(clie);
//                        await this.createUserBaseAccountEAuth(clie);
//                    }

//                }


//                request.AddParameter("clicod", clie.idCliente.ToString());
//                request.AddParameter("subscriber_name", clie.razonSocial);
//                request.AddParameter("organization_cuit", clie.cuit);
//                request.AddParameter("organization_legal_name", clie.razonSocial); //ejemplo: 30582622945
//                request.AddParameter("organization_legal_name", "ERREPAR PLUS");

//                if (clie.suspendido == "S")
//                {
//                    request.AddParameter("subscriber_status_id", 3);
//                }
//                else
//                {
//                    if (clie.suscriptorActivo == "N")
//                    {
//                        //Paso 5->Si esta Inactivo: Reactivacion Suscriptor
//                        await this.reactivarSuscriptor(clie);
//                        //request.AddParameter("subscriber_status_id", 2);
//                        request.AddParameter("subscriber_status_id", 1);
//                    }
//                    else
//                    {
//                        request.AddParameter("subscriber_status_id", 1); //ACTIVO
//                    }
//                }

//                request.AddParameter("subscriber_max_user_count", unCliente.subscriber_max_user_count.ToString());//esto puede cambiar segun los productos que tenga   
//                request.AddParameter("creation_date", clie.timeStamp.ToString());
//                request.AddParameter("creation_user", "INTEGRACION-SG");

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Alta Suscriptor OK - Suscriptor: " + clie.idCliente.ToString());
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Alta Suscriptor AWS: ERROR - Suscriptor: " + clie.idCliente.ToString() + " - " + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        //UPDATE SUSCRIPTOR
//        public async Task<Boolean> actualizarDatosSuscriptor(Cliente clie)
//        {
//            try
//            {
//                Cliente unSusc = await this.getDatosSuscriptor(clie.idCliente);
//                string email = await this.getUsersSubscriberCorpEntities(clie.idCliente); //VER QUE VA A DEVOLVER ESTO...

//                int cantProdPorSuscriptor = await this.getSubscriberSuscriptionCommProduct(unSusc.subscriber_id);


//                if (email != clie.mailComercial)
//                {
//                    //baja logica
//                    if (unSusc.organization_id != null)
//                    {
//                        await this.updateUserOrganizationCorpEntities(unSusc.organization_id, 0);
//                        var idloggingAccount = await this.loginAccountByemail(email);

//                        if (string.IsNullOrEmpty(idloggingAccount)) //compruebo si devuelve string nulo o vacio
//                        {
//                            //Si no existe el email lo doy de alta 
//                            await this.createLoginAccountCorpEntities(clie);
//                            if (await this.createUserBaseAccountEAuth(clie))
//                            {
//                                //createSubscriberRegistrationUserInvite (no se si se va a hacer)
//                            }
//                        }
//                    }
//                }
//                else if (email == clie.mailComercial)
//                {
//                    var idloggingAccount = await this.loginAccountByemail(email);
//                    await this.getUserOrganizationByLoginAccountCorpEntities(idloggingAccount);
//                    if (unSusc.organization_id != null)
//                    {
//                        if (await this.getUserOrganizationByLoginAccountCorpEntities(idloggingAccount))
//                        {
//                            //existe, lo hago admin
//                            await this.updateUserOrganizationCorpEntities(unSusc.organization_id, 1);
//                        }
//                        else
//                        {
//                            //no existe, darlo de alta
//                            await this.addUserOrganizationCorpEntities(unSusc.organization_id, idloggingAccount);
//                        }
//                    }
//                }
//                else //no trae nada, darlo de alta
//                {
//                    string user_id = await this.getUsersSubscriberCorpEntities(clie.idCliente); //VER QUE VA A DEVOLVER ESTO...
//                    if (unSusc.subscriber_id != null && user_id != null)
//                    {
//                        await this.assignUserSubscriberCorpEntity(unSusc.subscriber_id, user_id);
//                    }
//                }


//                var client = new RestClient(this.headerApi);

//                var request = new RestRequest(this.updateSubscriber + unSusc.subscriber_id, Method.Put);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);

//                request.AddParameter("clicod", clie.idCliente.ToString());
//                request.AddParameter("subscriber_name", clie.razonSocial);
//                // request.AddParameter("organization_cuit", clie.cuit);
//                request.AddParameter("organization_cuit", "30582622945");
//                //request.AddParameter("organization_legal_name", clie.razonSocial);
//                request.AddParameter("organization_legal_name", "ERREPAR PLUS");

//                if (clie.suspendido == "S")
//                {
//                    request.AddParameter("subscriber_status_id", 3);
//                }
//                else
//                {
//                    if (clie.suscriptorActivo == "N")
//                    {
//                        request.AddParameter("subscriber_status_id", 2);
//                    }
//                    else
//                    {
//                        request.AddParameter("subscriber_status_id", 1); //ACTIVO
//                    }
//                }


//                if (cantProdPorSuscriptor > 0)
//                {
//                    request.AddParameter("subscriber_max_user_count", cantProdPorSuscriptor);//esto puede cambiar segun los productos que tenga
//                }
//                else
//                {
//                    request.AddParameter("subscriber_max_user_count", 0);//esto puede cambiar segun los productos que tenga
//                }
//                request.AddParameter("modification_date", clie.timeStamp.ToString());
//                request.AddParameter("modification_user", "INTEGRACION-SG");

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Actualizacion Suscriptor OK - Suscriptor: " + clie.idCliente.ToString());
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Actualizacion Suscriptor: ERROR - Suscriptor: " + clie.idCliente.ToString() + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }


//        //Endpoint paralelo a actualizarDatosSuscriptor con el que le pegaré desde disableSubscriptionCommProduct
//        public async Task<Boolean> updateSubscriberCorpEntities(Suscripcion suscripcion)
//        {
//            try
//            {
//                Cliente unSusc = await this.getDatosSuscriptor(suscripcion.idCliente);
//                int cantProdPorSuscriptor = await this.getSubscriberSuscriptionCommProduct(unSusc.subscriber_id);

//                var client = new RestClient(this.headerApi);

//                var request = new RestRequest(this.updateSubscriber + unSusc.subscriber_id, Method.Put);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);

//                request.AddParameter("clicod", suscripcion.idCliente.ToString());
//                request.AddParameter("subscriber_name", unSusc.razonSocial);
//                // request.AddParameter("organization_cuit", clie.cuit);
//                request.AddParameter("organization_cuit", "30582622945");
//                request.AddParameter("organization_legal_name", "ERREPAR PLUS");
//                request.AddParameter("subscriber_status_id", 1); //ACTIVO


//                if (cantProdPorSuscriptor > 0)
//                {
//                    request.AddParameter("subscriber_max_user_count", cantProdPorSuscriptor);//esto puede cambiar segun los productos que tenga
//                }
//                else
//                {
//                    request.AddParameter("subscriber_max_user_count", 0);//esto puede cambiar segun los productos que tenga
//                }
//                request.AddParameter("modification_date", DateTime.Now.ToString());
//                request.AddParameter("modification_user", "INTEGRACION-SG");

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Actualizacion Suscriptor OK updateSubscriberCorpEntities- Suscriptor: " + unSusc.subscriber_id.ToString());
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Actualizacion Suscriptor updateSubscriberCorpEntities: ERROR - Suscriptor: " + unSusc.subscriber_id.ToString() + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> reactivarSuscriptor(Cliente clie)
//        {
//            try
//            {
//                Cliente unSusc = await this.getDatosSuscriptor(clie.idCliente);

//                var client = new RestClient(this.headerApi);

//                var request = new RestRequest(this.updateSubscriber + unSusc.subscriber_id, Method.Put);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);

//                request.AddParameter("subscriber_status_id", 1); //ACTIVO

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Actualizacion Suscriptor OK - Suscriptor: " + clie.idCliente.ToString());
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Actualizacion Suscriptor: ERROR - Suscriptor: " + clie.idCliente.ToString() + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        //---------------------------------------------------- S U S C R I P C I O N ----------------------------------------------------

//        //Traer los Productos por Suscriptor
//        public async Task<int> getSubscriberSuscriptionCommProduct(string? subscriber_id)
//        {
//            try
//            {
//                int total = 0;
//                var client = new RestClient(this.headerApiSuscripcion);
//                var request = new RestRequest(this.getProductosPorSuscriptor + subscriber_id, Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    foreach (var header in response.Headers) //verifico que exista el Total de Productos en los headers
//                    {
//                        if (header.Name == "X-Total-Count")
//                        {
//                            total = Convert.ToInt32(header.Value);
//                        }
//                    }
//                    //var content = response.Content;
//                    //SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);

//                    return total;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return 0;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return 0;
//            }
//        }

//        //Traer UUID PRODUCTO
//        public async Task<string> getProductCommProduct(int? unIdProd)
//        {
//            try
//            {
//                var client = new RestClient(this.headerApiSuscripcion);
//                var request = new RestRequest("getProductCommProduct/" + unIdProd, Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);
//                    //Console.WriteLine($"subscriber_id: {enp?.subscriber_id}");
//                    return enp?.product_id;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }

//        //ALTA SUSCRIPCION
//        public async Task<Boolean> addSubscriptionCommProduct(Suscripcion susc)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient(this.headerApi);
//                var request = new RestRequest(this.postSubscriber, Method.Post);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);


//                //paso 1 -> traigo el UUID subscriber_id
//                Cliente unCli = new Cliente(susc.idCliente);


//                unCli = await this.getDatosSuscriptor(susc.idCliente);
//                string product_id = "";
//                if (unCli.subscriber_id != null)
//                {
//                    //traigo UUID producto
//                    product_id = await this.getProductCommProduct(susc.idProducto);
//                }

//                request.AddParameter("subscriber_id", unCli.subscriber_id);
//                request.AddParameter("product_id", product_id);
//                request.AddParameter("subscription_finish_date", susc.vencimiento.ToString());
//                request.AddParameter("account_executive_ref_id", 1);
//                request.AddParameter("creation_user", "Test");


//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Alta Suscripcion OK - Suscriptor: " + unCli.subscriber_id + ", Producto: " + product_id);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Alta Suscripcion(addSubscriptionCommProduct) - Suscriptor: " + unCli.subscriber_id + ", Producto: " + product_id + " - " + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        //UPDATE SUSCRIPCION
//        public async Task<Boolean> updateProductCommProduct(Suscripcion susc)
//        {
//            return true;
//        }

//        //BAJA SUSCRIPCION
//        public async Task<Boolean> disableSubscriptionCommProduct(Suscripcion susc)
//        {
//            try
//            {
//                Cliente unSusc = await this.getDatosSuscriptor(susc.idCliente);
//                //traigo el UUID del Cliente
//                Cliente unCliente = await this.getDatosSuscriptor(susc.idCliente);

//                //traigo el UUID del Producto
//                string uuidProd = await this.getProductCommProduct(susc.idProducto);

//                //paso 1 -> veo si existe la organizacion
//                var rtaOrg = await this.getByCuitOrganizationCorpEntities(unCliente.cuit);
//                if (rtaOrg == false)
//                {
//                    //no existe la org
//                }
//                else
//                {
//                    await this.updateUserOrganizationCorpEntities(unCliente.cuit, 0);
//                }

//                //llamo a Endpoint 
//                await this.updateSubscriberCorpEntities(susc);


//                var client = new RestClient(this.headerApiSuscripcion);
//                var request = new RestRequest("disableSubscriptionCommProduct" + unSusc.subscriber_id + "/" + uuidProd, Method.Put);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApi);
//                request.AddParameter("is_active", 0);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Actualizacion disableSubscriptionCommProduct OK - Suscriptor: " + susc.idCliente.ToString());
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Actualizacion disableSubscriptionCommProduct: ERROR - Suscriptor: " + susc.idCliente.ToString() + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }



//        //---------------------------------------------------- O R G A N I Z A C I O N ----------------------------------------------------
//        public async Task<Boolean> getUserOrganizationByLoginAccountCorpEntities(string? loginAccountId)
//        {
//            try
//            {
//                var client = new RestClient("https://dev.organization-api.miestudio.dev.errepar.com/api/Organization/");
//                var request = new RestRequest("getUserOrganizationByLoginAccountCorpEntities/" + "?loginAccountId=" + loginAccountId, Method.Get);
//                request.AddHeader("Authorization", this.tokenApiOrganization);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);

//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> getByCuitOrganizationCorpEntities(string? unCuit)
//        {
//            try
//            {
//                var client = new RestClient("https://dev.organization-api.miestudio.dev.errepar.com/api/Organization/");
//                var request = new RestRequest("getByCuitOrganizationCorpEntities/" + unCuit, Method.Get);
//                request.AddHeader("Authorization", this.tokenApiOrganization);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);

//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> updateUserOrganizationCorpEntities(string organizationId, int estado)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient("https://dev.organization-api.miestudio.dev.errepar.com/api/Organization/");
//                var request = new RestRequest("updateUserOrganizationCorpEntities", Method.Post);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApiOrganization);
//                request.AddParameter("organizationUserId", organizationId);
//                request.AddParameter("organizationUserTypeCode", "ORG-ADMIN");
//                request.AddParameter("organizationCommercialUserTypeCode", "SUBSCRIBER");
//                if (estado > 0)
//                {
//                    request.AddParameter("userStatusCode", "ACTIVE");
//                }
//                else
//                {
//                    request.AddParameter("userStatusCode", 0);
//                }

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("updateUserOrganizationCorpEntities OK - organizationId: " + organizationId);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló updateUserOrganizationCorpEntities - organizationId: " + organizationId + " - " + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> addUserOrganizationCorpEntities(string organizationId, string loginAccountId)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient("https://dev.organization-api.miestudio.dev.errepar.com/api/Organization/");
//                var request = new RestRequest("addUserOrganizationCorpEntities", Method.Post);
//                request.RequestFormat = DataFormat.Json;
//                request.AddHeader("Authorization", this.tokenApiOrganization);


//                request.AddParameter("loginAccountId", loginAccountId);
//                request.AddParameter("organizationUserId", organizationId);
//                request.AddParameter("organizationUserTypeCode", "ORG-ADMIN");
//                request.AddParameter("organizationCommercialUserTypeCode", "SUBSCRIBER");
//                request.AddParameter("userStatusCode", "ACTIVE");



//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    //await mpLog.agregarLog("updateUserOrganizationCorpEntities OK - Suscriptor: " + unCli.subscriber_id);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló updateUserOrganizationCorpEntities - Suscriptor: " + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> createOrganizationCorpEntities(Cliente clie)
//        {
//            try
//            {
//                Console.WriteLine("llego");
//                return true;
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient("https://dev.organization-api.miestudio.dev.errepar.com/api/Organization/");
//                var request = new RestRequest("createOrganizationCorpEntities", Method.Post);
//                request.RequestFormat = DataFormat.Json;

//                request.AddHeader("Authentication", this.tokenApiOrganization);

//                request.AddParameter("organizationName", clie.razonSocial);
//                request.AddParameter("organizationTypeCode", "TESTING-ORG");
//                request.AddParameter("organizationLegalName", clie.razonSocial);
//                request.AddParameter("organizationCuit", clie.cuit);
//                request.AddParameter("organizationMaxAccessCount", 1);
//                request.AddParameter("isActive", true);


//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Alta Organizacion OK - Organizacion: " + clie.razonSocial);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Alta Organizacion: ERROR - Organizacion: " + clie.razonSocial + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        //---------------------------------------------------- LOGIN ACCOUNT ----------------------------------------------------
//        public async Task<string> loginAccountByemail(string? unEmail)
//        {
//            try
//            {
//                var client = new RestClient("https://accounts.errepar.com/login/api/");
//                var request = new RestRequest("loginAccountByemail/" + "?email=" + unEmail, Method.Get);
//                request.AddHeader("Authorization", this.tokenApi);

//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    //Console.WriteLine(response.Content);
//                    var content = response.Content;
//                    SuscriptorEndpoint? enp = JsonSerializer.Deserialize<SuscriptorEndpoint>(content);

//                    return enp.idLoginAccount;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }


//        public async Task<Boolean> createLoginAccountCorpEntities(Cliente clie)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient("https://accounts.errepar.com/login/api/");
//                var request = new RestRequest("loginAccountCreate", Method.Post);
//                request.RequestFormat = DataFormat.Json;

//                request.AddHeader("Authentication", this.tokenApi);

//                request.AddParameter("accountExternalRefId", clie.idCliente.ToString());
//                request.AddParameter("email", clie.mailComercial);
//                request.AddParameter("firstName", clie.razonSocial);
//                request.AddParameter("lastName", clie.cuit);
//                request.AddParameter("modificationUser", 1);
//                request.AddParameter("accountStatusCode", "ACTIVE");
//                request.AddParameter("authProviderCode", "Google");


//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("Alta Organizacion OK - Organizacion: " + clie.razonSocial);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Alta Organizacion: ERROR - Organizacion: " + clie.razonSocial + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }

//        public async Task<Boolean> createUserBaseAccountEAuth(Cliente clie)
//        {
//            try
//            {
//                //headerApi, postSubscriber y demas estan en appConfig
//                var client = new RestClient("https://accounts.errepar.com/login/eAuth/create/");
//                var request = new RestRequest("createUserBaseAccountEAuth", Method.Post);
//                request.RequestFormat = DataFormat.Json;

//                request.AddHeader("Authentication", this.tokenApi);


//                request.AddParameter("username", clie.mailComercial);
//                request.AddParameter("password", "123456789As+");


//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                {
//                    Console.WriteLine(response.Content);
//                    await mpLog.agregarLog("createUserBaseAccountEAuth OK - Cliente: " + clie.mailComercial);
//                    return true;
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                    await mpLog.agregarLog("Falló Alta createUserBaseAccountEAuth: ERROR - Cliente: " + clie.idCliente + response.StatusDescription);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //display error message
//                Console.WriteLine("Exception: " + ex.Message);
//                return false;
//            }
//        }


//        private void DeleteItem(int id)
//        {
//            try
//            {
//                var client = new RestClient("http://localhost:8080");
//                var request = new RestRequest($"items/{id}", Method.Delete);
//                var response = client.Execute(request);
//                if (response.StatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    Console.WriteLine(response.Content);
//                }
//                else
//                {
//                    Console.WriteLine(response.StatusDescription);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//            }

//        }

//    }
//}