using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;

namespace CapaVista
{
    public class WorkerSuscripcion
    {
        Orquestador orquestador = new Orquestador();// al llamar al constructor asigno sus propiedades
        MapperSuscripcion mprSuscipcion = new MapperSuscripcion();
        List<Suscripcion> suscripcionesA = new List<Suscripcion>();
        List<Suscripcion> suscripcionesA2 = new List<Suscripcion>();
        List<Suscripcion> suscripcionesB = new List<Suscripcion>();
        List<Suscripcion> suscripcionesM = new List<Suscripcion>();
        List<Suscripcion> listaSuscripciones = new List<Suscripcion>();
        // ------------------------------------------------------S U S C R I P C I O N E S-------------------------------------------------------------------
        public async Task verificarDatosSuscripciones()
        {
            try
            {
                this.ObtenerSuscripcionesAltas();
                this.VerificarSuscripcionesModificadas();
                this.VerificarSuscripcionesEliminadas();
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        public async void ObtenerSuscripcionesAltas()
        {
            try
            {
                //aca debo borrar la lista por las dudas: clientesIDS
                suscripcionesA.Clear();
                suscripcionesA2.Clear();

                suscripcionesA = mprSuscipcion.ConsultarIDSSuscripcionesAlta();

                if (suscripcionesA.Count() > 0)
                {
                    foreach (Suscripcion unaSusc in suscripcionesA)
                    {
                        //lleno la lista con todos los datos de la suscripcion
                        suscripcionesA2.Add(mprSuscipcion.ConsultarDatosSuscripcion((int)unaSusc.idCliente, (int)unaSusc.idProducto));
                    }

                    if (suscripcionesA2.Count() > 0)
                    {
                        foreach (Suscripcion unaSusc in suscripcionesA2)
                        {
                            this.AltaNuevaSuscripcion(unaSusc);
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


        public async void AltaNuevaSuscripcion(Suscripcion unaSuscipcion)
        {
            try
            {
                //aca tengo que llamar a endpoint 
                var rta1 = await orquestador.startSubscriptionSubscriberCorpCustomer(unaSuscipcion);
                if (rta1 == true)
                {
                    var rta2 = await mprSuscipcion.AltaNuevaSuscripcion(unaSuscipcion);
                    Console.WriteLine("La RTA ALTA DE SUSCRIPCION ES: " + rta2.ToString());
                }

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void VerificarSuscripcionesModificadas()
        {
            try
            {
                //Si es mayor a 0 la cantidad de modificados..
                suscripcionesM = mprSuscipcion.ConsultarSuscripcionesModificadas();
                //Si es mayor a 0 la cantidad de modificados..
                if (suscripcionesM.Count() > 0)
                {
                    foreach (Suscripcion susc in suscripcionesM)
                    {
                        this.ActualizarDatosSuscripciones(susc);
                    }
                        
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void ActualizarDatosSuscripciones(Suscripcion susc)
        {
            try
            {
                //var rta1 = await orquestador.updateProductCommProduct(susc);
                //if (rta1 == true)
                //{
                var rta = await mprSuscipcion.ActualizarDatosSuscripcion(susc);
                Console.WriteLine("La RTA ACTUALIZACION DE LA SUSCRIPCION ES: " + rta.ToString());
                //}
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void VerificarSuscripcionesEliminadas()
        {
            try
            {
                suscripcionesB = mprSuscipcion.ConsultarSuscripcionesBorradas();
                //Si es mayor a 0 la cantidad de eliminados..
                if (suscripcionesB.Count() > 0)
                {
                    foreach (Suscripcion susc in suscripcionesB)
                    {
                        this.EliminarSuscripcion(susc);
                    }
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void EliminarSuscripcion(Suscripcion susc)
        {
            try
            {

                var rta1 = await orquestador.finishSubscriptionSubscriberCorpCustomer(susc);
                if (rta1 == true)
                {
                    var rta2 = await mprSuscipcion.EliminarSuscripcion(susc);
                    Console.WriteLine("La RTA ALTA DE SUSCRIPCION ES: " + rta2.ToString());
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        //Buscara todos los clientes recientemente clonados para darlos de alta
        public async Task ObtenerTodasSuscripciones()
        {
            try
            {
                listaSuscripciones = mprSuscipcion.ObtenerTodasSuscripciones();
                if (listaSuscripciones.Count() > 0)
                {
                    foreach (Suscripcion susc in listaSuscripciones)
                    {
                        this.AltaNuevaSuscripcion(susc);
                    }
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
