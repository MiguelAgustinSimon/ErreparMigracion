using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace CapaDatos
{
    public class MapperLog
    {
        Connection mConeccion = new Connection();
        public async Task agregarLog(string texto)
        {

            int cont = 0;

            string ruta = @"..\\..\\..\\Logs\\log.txt";

            if (System.IO.Path.GetExtension(ruta).ToLower() == ".txt")
            {
                //Determina si existe el archivo.
                if (File.Exists(ruta))
                {
                    //El archivo existe, añadimos un log
                    string[] lineas = File.ReadAllLines(ruta);
                    List<string> lista = new List<string>(lineas.ToList());
                    lista.Add("Evento sucedido a las: " + DateTime.Now);
                    lista.Add(texto);
                    lista.Add("____________________________________________________");

                    File.WriteAllLines(ruta, lista);
                }
                else
                {
                    //El archivo no existe, lo creamos
                    StreamWriter OurStream;
                    OurStream = File.CreateText(ruta);
                    OurStream.WriteLine("Evento sucedido a las: " + DateTime.Now);
                    OurStream.WriteLine(texto);
                    OurStream.WriteLine("____________________________________________________");
                    OurStream.Close();
                }
            }
            else
            {
                //El archivo no es un PDF, continua sin realizar acción.
                Console.WriteLine("El archivo " + ruta + " no es un .txt.");
            }

        }

        public async Task agregarLogSerilog(string texto, bool resultado)
        {
            try
            {
                string ruta = mConeccion.ObtenerRutaSerilog();
                string sLogCompruebaMesActual = ruta + DateTime.Today.ToString("yyyy-MM") + ".txt";
                //string sLogCompruebaMesActual = ruta + DateTime.Today.AddMonths(+1).ToString("yyyy-MM") + ".txt";

                if (File.Exists(sLogCompruebaMesActual))
                {
                    Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(sLogCompruebaMesActual)
                    .MinimumLevel.Verbose()
                    .CreateLogger();
                    if (resultado == true)
                    {
                        Log.Information(texto);
                        Log.Debug("________________________________________________________________________________________________________");
                    }
                    else
                    {
                        Log.Error(texto);
                        Log.Debug("________________________________________________________________________________________________________");
                    }
                }
                else
                {
                    string archivo = ruta + DateTime.Today.ToString("yyyy-MM") + ".txt";
                    Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(archivo)
                    .MinimumLevel.Verbose()
                    .CreateLogger();
                    if (resultado == true)
                    {
                        Log.Information(texto);
                    }
                    else
                    {
                        Log.Error(texto);
                    }
                }

            }
            catch (Exception e)
            {

                Log.Error("Algo ha salido mal ", e);
            }
            Log.CloseAndFlush();
        }


    }

 
}
