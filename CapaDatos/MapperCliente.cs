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
    public class MapperCliente
    {
        Command cmd = new Command();

        public List<Cliente> ObtenerTodosClientes()
        {
            DataTable DTabla = new DataTable();

            DTabla = cmd.ObtenerTodosClientes();
            List<Cliente> unaLista = new List<Cliente>();
            if (DTabla != null)
            {
                if (DTabla.Rows.Count > 0)
                {
                    foreach (DataRow x in DTabla.Rows)
                    {
                        string? mail = !Convert.IsDBNull(x[1]) ? (string?)x[1] : null;
                        string? suscriptorActivo = !Convert.IsDBNull(x[1]) ? (string?)x[2] : null;
                        string? razonSocial = !Convert.IsDBNull(x[1]) ? (string?)x[5] : null;
                        string? estaSuspendido = !Convert.IsDBNull(x[1]) ? (string?)x[6] : null;
                        string? unPais = !Convert.IsDBNull(x[1]) ? (string?)x[8] : null;
                        string? unaProvincia = !Convert.IsDBNull(x[1]) ? (string?)x[9] : null;
                        string? unTipoSuscriptor = !Convert.IsDBNull(x[1]) ? (string?)x[10] : null;
                        decimal? unPerIIBB = !Convert.IsDBNull(x[11]) ? (decimal?)x[11] : null;
                        string? unCuit = !Convert.IsDBNull(x[1]) ? (string?)x[12] : null;
                        Cliente clie = new Cliente((int)x[0], mail, suscriptorActivo, (DateTime)x[3], (DateTime)x[4], razonSocial, estaSuspendido,
                            (DateTime)x[7], unPais, unaProvincia, unTipoSuscriptor, unPerIIBB, unCuit);
                        unaLista.Add(clie);
                    }
                }
            }
            return unaLista;
        }

        public List<Cliente> ConsultarIDSClientesAlta()
        {
            DataTable DTabla = cmd.ObtenerIDSClientesAlta();
            List<Cliente> unaLista = new List<Cliente>();

            if (DTabla.Rows.Count > 0)
            {
                foreach (DataRow x in DTabla.Rows)
                {
                    Cliente clie = new Cliente((int)x[0]);
                    unaLista.Add(clie);
                }
            }
            return unaLista;
        }

        public List<Cliente> ConsultarDatosCliente(int unId)
        {
            DataTable DTabla = cmd.ConsultarDatosCliente(unId);
            List<Cliente> unaLista = new List<Cliente>();

            if (DTabla.Rows.Count > 0)
            {
                foreach (DataRow x in DTabla.Rows)
                {
                    string? mail = !Convert.IsDBNull(x[1]) ? (string?)x[1] : null;
                    string? suscriptorActivo = !Convert.IsDBNull(x[1]) ? (string?)x[2] : null;
                    string? razonSocial = !Convert.IsDBNull(x[1]) ? (string?)x[5] : null;
                    string? estaSuspendido = !Convert.IsDBNull(x[1]) ? (string?)x[6] : null;
                    string? unPais = !Convert.IsDBNull(x[1]) ? (string?)x[8] : null;
                    string? unaProvincia = !Convert.IsDBNull(x[1]) ? (string?)x[9] : null;
                    string? unTipoSuscriptor = !Convert.IsDBNull(x[1]) ? (string?)x[10] : null;
                    decimal? unPerIIBB = !Convert.IsDBNull(x[11]) ? (decimal?)x[11] : null;
                    string? unCuit = !Convert.IsDBNull(x[1]) ? (string?)x[12] : null;
                    Cliente clie = new Cliente((int)x[0], mail, suscriptorActivo, (DateTime)x[3], (DateTime)x[4], razonSocial, estaSuspendido,
                        (DateTime)x[7], unPais, unaProvincia, unTipoSuscriptor, unPerIIBB, unCuit);
                    unaLista.Add(clie);
                }
            }
            return unaLista;
        }

        public List<Cliente> ConsultarSuscriptoresBorrados()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresBorrados();
                List<Cliente> unaLista = new List<Cliente>();
                if (DTabla!=null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            int? id = !Convert.IsDBNull(x[0]) ? (int?)x[0] : null;

                            Cliente clie = new Cliente(id);
                            unaLista.Add(clie);
                        }
                    }
                }
                
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        #region Deteccion de Modificaciones
        public List<Cliente> ConsultarSuscriptoresModificados()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla= cmd.ObtenerSuscriptoresModificados();
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla !=null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? mail = !Convert.IsDBNull(x[1]) ? (string?)x[1] : null;
                            string? suscriptorActivo = !Convert.IsDBNull(x[1]) ? (string?)x[2] : null;
                            string? razonSocial = !Convert.IsDBNull(x[1]) ? (string?)x[5] : null;
                            string? estaSuspendido = !Convert.IsDBNull(x[1]) ? (string?)x[6] : null;
                            string? unPais = !Convert.IsDBNull(x[1]) ? (string?)x[8] : null;
                            string? unaProvincia = !Convert.IsDBNull(x[1]) ? (string?)x[9] : null;
                            string? unTipoSuscriptor = !Convert.IsDBNull(x[1]) ? (string?)x[10] : null;
                            decimal? unPerIIBB = !Convert.IsDBNull(x[11]) ? (decimal?)x[11] : null;
                            string? unCuit = !Convert.IsDBNull(x[1]) ? (string?)x[12] : null;
                            Cliente clie = new Cliente((int)x[0], mail, suscriptorActivo, (DateTime)x[3], (DateTime)x[4], razonSocial, estaSuspendido,
                                (DateTime)x[7], unPais, unaProvincia, unTipoSuscriptor, unPerIIBB, unCuit);
                            unaLista.Add(clie);
                        }
                    }
                }
                
                return unaLista;
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        public List<Cliente> ConsultarSuscriptoresModificadosMail()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresModificados("MailComercial");
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla != null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? mail = !Convert.IsDBNull(x[1]) ? (string?)x[1] : null;
                           
                            Cliente clie = new Cliente((int)x[0]);
                            clie.mailComercial = mail;
                            unaLista.Add(clie);
                        }
                    }
                }
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Cliente> ConsultarSuscriptoresModificadosCuit()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresModificados("CUIT");
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla != null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? unCuit = !Convert.IsDBNull(x[1]) ? (string?)x[12] : null;

                            Cliente clie = new Cliente((int)x[0]);
                            clie.cuit = unCuit;
                            unaLista.Add(clie);
                        }
                    }
                }
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Cliente> ConsultarSuscriptoresModificadosRazonSocial()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresModificados("RazonSocial");
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla != null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? razonSocial = !Convert.IsDBNull(x[1]) ? (string?)x[5] : null;

                            Cliente clie = new Cliente((int)x[0]);
                            clie.razonSocial = razonSocial;
                            unaLista.Add(clie);
                        }
                    }
                }
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Cliente> ConsultarSuscriptoresModificadosRazonActivo()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresModificados("SuscriptorActivo");
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla != null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? suscriptorActivo = !Convert.IsDBNull(x[1]) ? (string?)x[2] : null;

                            Cliente clie = new Cliente((int)x[0]);
                            clie.suscriptorActivo = suscriptorActivo;
                            unaLista.Add(clie);
                        }
                    }
                }
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Cliente> ConsultarSuscriptoresModificadosSuspendido()
        {
            try
            {
                DataTable DTabla = new DataTable();
                DTabla = cmd.ObtenerSuscriptoresModificados("Suspendido");
                List<Cliente> unaLista = new List<Cliente>();

                if (DTabla != null)
                {
                    if (DTabla.Rows.Count > 0)
                    {
                        foreach (DataRow x in DTabla.Rows)
                        {
                            string? estaSuspendido = !Convert.IsDBNull(x[1]) ? (string?)x[6] : null;

                            Cliente clie = new Cliente((int)x[0]);
                            clie.suspendido = estaSuspendido;
                            unaLista.Add(clie);
                        }
                    }
                }
                return unaLista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        //Le paso los clientes que encontro en ConsultarClientesAlta
        public async Task<Boolean> AltaNuevoCliente(Cliente cli)
        {
            try
            {
                bool respuesta = await cmd.AltaNuevoCliente(cli);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Le paso los clientes que encontro en ConsultarSuscriptoresModificados
        public async Task<Boolean> ActualizarDatosCliente(Cliente cli)
        {
            try
            {
                bool respuesta = await cmd.ActualizarDatosCliente(cli);
                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarDatosCliente(Cliente cli, string queActualizar)
        {
            try
            {
                bool respuesta = false;
                switch (queActualizar)
                {
                    case "MailComercial":
                         respuesta= await cmd.ActualizarMailCliente(cli);
                        break;
                    case "CUIT":
                        respuesta = await cmd.ActualizarCuitCliente(cli);
                        break;
                    case "RazonSocial":
                        respuesta = await cmd.ActualizarRazonSocialCliente(cli);
                        break;
                    case "SuscriptorActivo":
                        respuesta = await cmd.ActualizarActivoCliente(cli);
                        break;
                    case "Suspendido":
                        respuesta = await cmd.ActualizarSuspendidoCliente(cli);
                        break;
                    default:
                        break;
                }
                
                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Le paso los clientes que encontro en ConsultarSuscriptoresBorrados
        public async Task<Boolean> EliminarCliente(Cliente cli)
        {
            try
            {
                bool respuesta = await cmd.EliminarCliente(cli);
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<Boolean> ActualizarNovedadesSuscriptor(Cliente cli, string tipo,string estado, string response)
        {
            try
            {
                bool respuesta = await cmd.ActualizarNovedadesSuscriptor(cli, tipo, estado,response); 
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
