
namespace CapaVista // Note: actual namespace depends on the project name.
{
    public class Program 
    {
        Boolean banderaClientes = false;
        Boolean banderaSuscripciones = false;

        //LOAD
        static void Main(string[] args)
        {
            Program p = new Program();
            p.iniciarSistema();
        }

        public async Task iniciarSistema()
        {
            try
            {
                //await this.realizarClonadoClientes();
                await this.realizarClonadoSuscripciones();

                Console.WriteLine("Aguarde mientras se ejecutan las operaciones...");

                if (this.banderaClientes == true)
                {
                    //hizo una copia asi que procedemos a impactar en Orquestador a todos los clientes
                    await this.generarAltaMasivaClientes();
                }
                else
                {
                    await this.verificarClientes();
                }

                if (this.banderaSuscripciones == true)
                {
                    //hizo una copia asi que procedemos a impactar en Orquestador a todos las suscripciones
                    await this.generarAltaMasivaSuscripciones();
                }
                else
                {
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
        public async Task realizarClonadoClientes()
        {
            try
            {
                WorkerClonadoBD wcbd = new WorkerClonadoBD();
                this.banderaClientes = await wcbd.realizarClonadoTablaClientes();

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public async Task realizarClonadoSuscripciones()
        {
            try
            {
                WorkerClonadoBD wcbd = new WorkerClonadoBD();
                this.banderaSuscripciones = await wcbd.realizarClonadoTablaSuscripciones();

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public async Task generarAltaMasivaClientes()
        {
            try
            {
                WorkerCliente wc = new WorkerCliente();
                await wc.ObtenerTodosClientes();

            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public async Task generarAltaMasivaSuscripciones()
        {
            try
            {
                WorkerSuscripcion ws = new WorkerSuscripcion();
                await ws.ObtenerTodasSuscripciones();

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