using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;

namespace CapaVista
{
    public class WorkerSuscripcion
    {
        MapperSuscripcion mprSuscipcion = new MapperSuscripcion();
        List<Suscripcion> suscripcionesIDS = new List<Suscripcion>();
        List<Suscripcion> suscripcionesA = new List<Suscripcion>();
        List<Suscripcion> suscripcionesB = new List<Suscripcion>();
        List<Suscripcion> suscripcionesM = new List<Suscripcion>();
        // ------------------------------------------------------S U S C R I P C I O N E S-------------------------------------------------------------------
        public async Task verificarDatosSuscripciones()
        {
            try
            {
                this.ObtenerIDSSuscripcionesAltas();
                this.VerificarSuscripcionesModificadas();
                this.VerificarSuscripcionesEliminadas();
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        public async void ObtenerIDSSuscripcionesAltas()
        {
            try
            {
                suscripcionesIDS = mprSuscipcion.ConsultarIDSSuscripcionesAlta();
                if (suscripcionesIDS.Count() > 0)
                {
                    foreach (Suscripcion unaSusc in suscripcionesIDS)
                    {
                        suscripcionesA = mprSuscipcion.ConsultarDatosSuscripcion((int)unaSusc.idCliente, (int)unaSusc.idProducto);
                        //Si es mayor a 0 la cantidad de altas..
                        if (suscripcionesA.Count() > 0)
                        {
                            this.AltaNuevaSuscripcion();
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

        public async void AltaNuevaSuscripcion()
        {
            try
            {
                foreach (Suscripcion unaSuscipcion in suscripcionesA)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );
                    var rta = await mprSuscipcion.AltaNuevaSuscripcion(unaSuscipcion);
                    Console.WriteLine("La RTA ALTA DE SUSCRIPCION ES: " + rta.ToString());
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
                    this.ActualizarDatosSuscripciones();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public async void ActualizarDatosSuscripciones()
        {
            try
            {
                foreach (Suscripcion susc in suscripcionesM)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );
                    var rta = await mprSuscipcion.ActualizarDatosSuscripcion(susc);
                    Console.WriteLine("La RTA ACTUALIZACION DE LA SUSCRIPCION ES: " + rta.ToString());
                }
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
                    this.EliminarSuscripcion();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void EliminarSuscripcion()
        {
            try
            {
                foreach (Suscripcion susc in suscripcionesB)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );
                    var rta = await mprSuscipcion.EliminarSuscripcion(susc);
                    Console.WriteLine("ELIMINACION DE LA SUSCRIPCION ES: " + rta.ToString());
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
