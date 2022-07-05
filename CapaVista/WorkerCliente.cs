﻿using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;

namespace CapaVista
{
    public class WorkerCliente
    {
        MapperCliente mprClie = new MapperCliente();
        List<Cliente> clientesIDS = new List<Cliente>();
        List<Cliente> clientesA = new List<Cliente>();
        List<Cliente> clientesB = new List<Cliente>();
        List<Cliente> clientesM = new List<Cliente>();
        Log unLog = new Log();

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
                    var rta = await mprClie.AltaNuevoCliente(unCliente);
                    Console.WriteLine("La RTA ALTA DE CLIENTE ES: " + rta.ToString());
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
                    this.ActualizarDatosCliente();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async void ActualizarDatosCliente()
        {
            try
            {
                foreach (Cliente unCliente in clientesM)
                {
                    //Console.WriteLine(unCliente.idCliente + " - " + unCliente.mailComercial );
                    var rta = mprClie.ActualizarDatosCliente(unCliente);
                    Console.WriteLine("La RTA ACTUALIZACION CLIENTE ES: " + rta.Result);
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

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
    }
}