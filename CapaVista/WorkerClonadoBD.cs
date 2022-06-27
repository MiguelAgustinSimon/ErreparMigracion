using System.Data;
using System.Data.SqlClient;
using System.Timers;
using CapaDatos;
using CapaNegocio;

namespace CapaVista
{
    public class WorkerClonadoBD 
    {
        Boolean bandera = false;

        public async Task <bool> realizarClonadoTablas()
        {
            try
            {
                MapperClonadoBD mprCBD = new MapperClonadoBD();// al llamar al constructor asigno sus propiedades

                var rtaClienteTest = mprCBD.PreguntarExistencia(mprCBD.tablaDestinoDC);
                var rtaSuscripcionesTest = mprCBD.PreguntarExistencia(mprCBD.tablaDestinoSA);

                if (rtaClienteTest.Result == false)
                {
                    //hacer copia entera
                    var rta = mprCBD.InsertarCopia(mprCBD.tablaDestinoDC, mprCBD.tablaOrigenDC);
                    Console.WriteLine("La Copia de Tabla Clientes es: " + rta.Result);
                    this.bandera = true;
                }
                else
                {
                    Console.WriteLine("TABLA CLIENTES EXISTENTE OK ");
                    this.bandera = false;
                }

                if (rtaSuscripcionesTest.Result == false)
                {
                    //hacer copia entera
                    var rta = mprCBD.InsertarCopia(mprCBD.tablaDestinoSA, mprCBD.tablaOrigenSA);
                    Console.WriteLine("La Copia de Tabla Suscripciones es: " + rta.Result);
                    this.bandera = true;
                }
                else
                {
                    Console.WriteLine("TABLA SUSCRIPCIONES EXISTENTE OK ");
                    this.bandera = false;
                }
                return this.bandera;
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
