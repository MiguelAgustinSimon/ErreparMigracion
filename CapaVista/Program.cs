using System;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;
using static System.Net.Mime.MediaTypeNames;

namespace CapaVista // Note: actual namespace depends on the project name.
{
    public class Program 
    {
        Boolean bandera = false;

        //LOAD
        static void Main(string[] args)
        {
            Program p=new Program();
            p.iniciarSistema();

        }
           
        public async Task iniciarSistema()
        {
            try
            {
                await this.realizarClonadoTablas();

                if (this.bandera == false) //ya hay una copia previa asi que verificaremos caso por caso
                {
                    Console.WriteLine("Aguarde mientras se ejecutan las operaciones...");
                    await this.verificarClientes();
                    await this.verificarSuscripciones();
                }

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("PROCESO FINALIZADO");
                Environment.Exit(0);//Cierra la ventana
            }
        }



        //-------------------------------------------------------------------  CLONADO BD   ---------------------------------------------------
        public async Task realizarClonadoTablas()
        {
            try
            {
                WorkerClonadoBD wcbd = new WorkerClonadoBD();
                this.bandera = await wcbd.realizarClonadoTablas();

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        //-------------------------------------------------------------------  C L I E N T E S   ---------------------------------------------------
        public async Task verificarClientes()
        {
            try
            {
                WorkerCliente wc = new WorkerCliente();
                await wc.verificarDatosClientes();

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


        // ------------------------------------------------------S U S C R I P C I O N E S-------------------------------------------------------------------
        public async Task verificarSuscripciones()
        {
            try
            {
                WorkerSuscripcion ws = new WorkerSuscripcion();
                await ws.verificarDatosSuscripciones();
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

    }
}