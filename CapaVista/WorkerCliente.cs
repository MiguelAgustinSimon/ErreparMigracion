using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;

namespace CapaVista
{
    public class WorkerCliente
    {
        MapperCliente mprClie = new MapperCliente();
        Orquestador orquestador = new Orquestador();// al llamar al constructor asigno sus propiedades
        List<Cliente> clientesIDS = new List<Cliente>();
        List<Cliente> clientesAltaMasiva = new List<Cliente>();
        List<Cliente> clientesA = new List<Cliente>();
        List<Cliente> clientesB = new List<Cliente>();
        List<Cliente> clientesM = new List<Cliente>();
        List<Cliente> misClientesMail = new List<Cliente>();
        List<Cliente> misClientesCuit = new List<Cliente>();
        List<Cliente> misClientesRazonSocial = new List<Cliente>();
        List<Cliente> misClientesActivos = new List<Cliente>();
        List<Cliente> misClientesSuspendidos = new List<Cliente>();

        //-------------------------------------------------------------------  C L I E N T E S   ---------------------------------------------------
        public async Task verificarDatosClientes()
        {
            try
            {
                this.ObtenerIDSClientesAltas();
                this.VerificarClientesModificados();
                this.VerificarClientesBorrados();
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void ObtenerIDSClientesAltas()
        {
            try
            {
                //aca debo borrar la lista por las dudas: clientesIDS
                clientesIDS.Clear();
                clientesIDS = mprClie.ConsultarIDSClientesAlta();
                if (clientesIDS.Count() > 0)
                {
                    foreach (Cliente unCliente in clientesIDS)
                    {
                        clientesA = mprClie.ConsultarDatosCliente((int)unCliente.idCliente);
                        //Si es mayor a 0 la cantidad de la lista..
                        if (clientesA.Count() > 0)
                        {
                            this.AltaNuevoCliente();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void AltaNuevoCliente()
        {
            try
            {
                foreach (Cliente unCliente in clientesA)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );

                    //aca tengo que llamar a endpoint AltaCliente: createCustomerUserCorpCustomer
                    var rta1 = await orquestador.createCustomerUserCorpCustomer(unCliente);
                    if (rta1 == true)
                    {
                        var rta2 = await mprClie.AltaNuevoCliente(unCliente);
                        Console.WriteLine("La RTA ALTA DE CLIENTE ES: " + rta2.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void VerificarClientesModificados()
        {
            try
            {
                //Si es mayor a 0 la cantidad de modificados..
                clientesM = mprClie.ConsultarSuscriptoresModificados();
                if (clientesM.Count() > 0)
                {
                    foreach (Cliente unCliente in clientesM)
                    {
                        this.ActualizarDatosCliente(unCliente);
                    }
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public async void ActualizarDatosCliente(Cliente unCliente)
        {
            try
            {
                //aca tengo que llamar a endpoint 
                var rta1 = await orquestador.updateSubscriberCorpCustomer(unCliente);
                if (rta1 == true)
                {
                    var rta2 = mprClie.ActualizarDatosCliente(unCliente);
                    Console.WriteLine("La RTA ACTUALIZACION CLIENTE ES: " + rta2.Result);
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        #region Modificaciones nuevas (mail,cuit, etc)
        //public async void VerificarModificaciones()
        //{
        //    try
        //    {

        //        //Si es mayor a 0 la cantidad de modificados..
        //        this.misClientesMail = mprClie.ConsultarSuscriptoresModificadosMail();
        //        this.misClientesCuit = mprClie.ConsultarSuscriptoresModificadosCuit();
        //        this.misClientesRazonSocial = mprClie.ConsultarSuscriptoresModificadosRazonSocial();
        //        this.misClientesActivos = mprClie.ConsultarSuscriptoresModificadosRazonActivo();
        //        this.misClientesSuspendidos = mprClie.ConsultarSuscriptoresModificadosSuspendido();

        //        if (this.misClientesMail.Count() > 0)
        //        {
        //            this.ActualizarDatos(this.misClientesMail, "MailComercial");
        //        }

        //        if (this.misClientesCuit.Count() > 0)
        //        {
        //            this.ActualizarDatos(this.misClientesCuit, "CUIT");
        //        }

        //        if (this.misClientesRazonSocial.Count() > 0)
        //        {
        //            this.ActualizarDatos(this.misClientesRazonSocial, "RazonSocial");
        //        }

        //        if (this.misClientesActivos.Count() > 0)
        //        {
        //            this.ActualizarDatos(this.misClientesActivos, "SuscriptorActivo");
        //        }

        //        if (this.misClientesSuspendidos.Count() > 0)
        //        {
        //            this.ActualizarDatos(this.misClientesSuspendidos, "Suspendido");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //display error message
        //        Console.WriteLine("Exception: " + ex.Message);
        //    }
        //}

        //public async void ActualizarDatos(List<Cliente> unaLista, string queActualizar)
        //{
        //    try
        //    {
        //        foreach (Cliente unCliente in unaLista)
        //        {
        //            //aca tengo que llamar a endpoint 
        //            var rta1 = await orquestador.updateSubscriberCorpCustomer(unCliente);
        //            if (rta1 == true)
        //            {
        //                var rta2 = mprClie.ActualizarDatosCliente(unCliente, queActualizar);
        //                Console.WriteLine("La RTA ACTUALIZACION CLIENTE ES: " + rta2.Result);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //display error message
        //        Console.WriteLine("Exception: " + ex.Message);
        //    }
        //}

        #endregion



        public async void VerificarClientesBorrados()
        {
            try
            {
                //Si es mayor a 0 la cantidad de eliminados..
                clientesB = mprClie.ConsultarSuscriptoresBorrados();
                if (clientesB.Count() > 0)
                {
                    this.EliminarCliente();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void EliminarCliente()
        {
            try
            {
                foreach (Cliente unCliente in clientesB)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );
                    var rta = mprClie.EliminarCliente(unCliente);
                    Console.WriteLine("ELIMINACION DEL CLIENTE ES: " + rta.Result);
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        //Buscara todos los clientes recientemente clonados para darlos de alta
        public async Task ObtenerTodosClientes()
        {
            try
            {
                clientesAltaMasiva = mprClie.ObtenerTodosClientes();
                if (clientesAltaMasiva.Count() > 0)
                {
                    foreach (Cliente unCliente in clientesAltaMasiva)
                    {
                        this.AltaNuevoCliente(unCliente);
                    }
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public async void AltaNuevoCliente(Cliente unCliente)
        {
            try
            {
                //aca tengo que llamar a endpoint AltaCliente: createCustomerUserCorpCustomer
                var rta1 = await orquestador.createCustomerUserCorpCustomer(unCliente);
                if (rta1 == true)
                {
                    Console.WriteLine("EL ALTA DE CLIENTE ORQUESTADOR ES CORRECTA - CLIENTE:" + unCliente.idCliente);
                }
                
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


    }
}
