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
    public class MapperSuscripcion
    {
        Command cmd = new Command();

        public List<Suscripcion> ConsultarIDSSuscripcionesAlta()
        {
            DataTable DTabla = cmd.ObtenerIDSSuscripcionesAlta();
            List<Suscripcion> unaLista = new List<Suscripcion>();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        int? idClie = !Convert.IsDBNull(x[0]) ? (int?)x[0] : null;
                        int? idProd = !Convert.IsDBNull(x[1]) ? (int?)x[1] : null;

                        Suscripcion susc = new Suscripcion(idClie, idProd);
                        unaLista.Add(susc);
                    }
                }
            }
            return unaLista;
        }

        public Suscripcion ConsultarDatosSuscripcion(int idClie, int idProd)
        {
            DataTable DTabla = cmd.ConsultarDatosSuscripcion(idClie, idProd);
            Suscripcion unaSuscripcion = new Suscripcion();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        string? tema = !Convert.IsDBNull(x[2]) ? (string?)x[2] : null;
                        int? ejecutivo = !Convert.IsDBNull(x[4]) ? (int?)x[4] : null;

                        Suscripcion susc = new Suscripcion((int)x[0], (int)x[1], tema, (DateTime)x[3], ejecutivo);
                        unaSuscripcion = susc;
                    }
                }
            }
            return unaSuscripcion;
        }

        public List<Suscripcion> ConsultarSuscripcionesModificadas()
        {
            DataTable DTabla = cmd.ObtenerSuscripcionesModificadas();
            List<Suscripcion> unaLista = new List<Suscripcion>();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        string? tema = !Convert.IsDBNull(x[2]) ? (string?)x[2] : null;

                        Suscripcion susc = new Suscripcion((int)x[0], (int)x[1], tema, (DateTime)x[3], (int)x[4]);
                        unaLista.Add(susc);
                    }
                }
            }
            return unaLista;
        }//FinFuncion ConsultarSuscripcionesModificadas 

        public List<Suscripcion> ConsultarSuscripcionesBorradas()
        {
            DataTable DTabla = cmd.ObtenerSuscripcionesBorradas();
            List<Suscripcion> unaLista = new List<Suscripcion>();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        int? idClie = !Convert.IsDBNull(x[0]) ? (int?)x[0] : null;
                        int? idProd = !Convert.IsDBNull(x[1]) ? (int?)x[1] : null;
                        string? tema = !Convert.IsDBNull(x[2]) ? (string?)x[2] : null;

                        Suscripcion susc = new Suscripcion(idClie, idProd, tema, (DateTime)x[3], (int)x[4]);
                        unaLista.Add(susc);
                    }
                }
            }
            return unaLista;
        }//FinFuncion ConsultarSuscriptoresBorrados 


        public List<Suscripcion> ObtenerTodasSuscripciones()
        {
            DataTable DTabla = cmd.ObtenerTodasSuscripciones();
            List<Suscripcion> unaLista = new List<Suscripcion>();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        int? idClie = !Convert.IsDBNull(x[0]) ? (int?)x[0] : null;
                        int? idProd = !Convert.IsDBNull(x[1]) ? (int?)x[1] : null;
                        string? tema = !Convert.IsDBNull(x[2]) ? (string?)x[2] : null;

                        Suscripcion susc = new Suscripcion(idClie, idProd,tema, (DateTime)x[3],(int)x[4]);
                        unaLista.Add(susc);
                    }
                }
            }
            return unaLista;
        }//FinFuncion ObtenerTodasSuscripciones 


        //Le paso los clientes que encontro en ConsultarClientesAlta
        public async Task<Boolean> AltaNuevaSuscripcion(Suscripcion suscripcion)
        {
            try
            {
                Command cmd = new Command();
                bool respuesta = await cmd.RegistrarNuevaSuscripcion(suscripcion);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<Boolean> ActualizarDatosSuscripcion(Suscripcion susc)
        {
            try
            {
                Command cmd = new Command();
                bool respuesta = await cmd.ActualizarDatosSuscripcion(susc);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<Boolean> EliminarSuscripcion(Suscripcion susc)
        {
            try
            {
                Command cmd = new Command();
                bool respuesta = await cmd.EliminarSuscripcion(susc);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarNovedadesSuscripcion(Suscripcion susc, string tipo, string estado, string response)
        {
            try
            {
                bool respuesta = await cmd.ActualizarNovedadesSuscripcion(susc, tipo, estado, response);
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
