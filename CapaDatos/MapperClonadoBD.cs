using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaDatos;
using CapaNegocio;


namespace CapaDatos
{
    public class MapperClonadoBD
    {
        public string tablaOrigenDC;
        public string tablaDestinoDC;
        public string tablaOrigenSA;
        public string tablaDestinoSA;
        public string tablaNovedadesSuscriptor;
        public string tablaNovedadesSuscripcion;

        string[] arrTablasDC;
        string[] arrTablasSA;
        string[] arrTablasNovedades;

        public MapperClonadoBD()
        {

            Connection mConeccion = new Connection();
            
            //Aca obtengo el nombre de las tablas que esta en appsettings.json
            this.arrTablasDC = mConeccion.ObtenerTablasClientes();
            this.arrTablasSA = mConeccion.ObtenerTablasSuscripciones();
            this.arrTablasNovedades = mConeccion.ObtenerTablasNovedades();

            foreach (var item in this.arrTablasDC.Select((elemento, i) => new{ i, elemento}))
            {
                switch (item.i)
                {
                    case 0:
                        this.tablaOrigenDC = item.elemento;
                        break;
                    case 1:
                        this.tablaDestinoDC = item.elemento;
                        break;
                }
            }
            foreach (var item in this.arrTablasSA.Select((elemento, i) => new { i, elemento }))
            {
                switch (item.i)
                {
                    case 0:
                        this.tablaOrigenSA = item.elemento;
                        break;
                    case 1:
                        this.tablaDestinoSA = item.elemento;
                        break;
                }
            }
            foreach (var item in this.arrTablasNovedades.Select((elemento, i) => new { i, elemento }))
            {
                switch (item.i)
                {
                    case 0:
                        this.tablaNovedadesSuscriptor = item.elemento;
                        break;
                    case 1:
                        this.tablaNovedadesSuscripcion = item.elemento;
                        break;
                }
            }
        }

        public async Task<Boolean> PreguntarExistencia(string pTabla)
        {
            try
            {
                Command cmd = new Command();
                bool respuesta = await cmd.PreguntarExistencia(pTabla);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Boolean> InsertarCopia(string nuevaTabla, string tablaActual)
        {
            try
            {
                Command cmd = new Command();
                bool respuesta = await cmd.InsertarCopiaCompleta(nuevaTabla, tablaActual);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
