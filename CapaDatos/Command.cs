using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaNegocio;

namespace CapaDatos
{
    public class Command
    {
        MapperClonadoBD mprCBD = new MapperClonadoBD();// al llamar al constructor asigno sus propiedades
        MapperLog mapperLog = new MapperLog();

        private static SqlCommand mCom;
        public static SqlCommand CommandObj(string pConsulta, SqlConnection pCon)
        {
            mCom = new SqlCommand();
            mCom.CommandText = pConsulta;
            mCom.CommandType = CommandType.Text;
            mCom.Connection = pCon;
            return mCom;
        }

        public async Task<Boolean> PreguntarExistencia(string pTabla)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                // using (SqlCommand cmd = new SqlCommand("SELECT * INTO " + nuevaTabla + " FROM " + tablaActual, Connection.ConnectionObj()))
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' " +
                    "AND TABLE_NAME ='" + pTabla + "'", Connection.ConnectionObj()))
                {
                    // Comprobamos si existe-> Devuelve 1:existe o 0:no existe
                    int n = (int)cmd.ExecuteScalar();
                    mConeccion.Close();
                    return n > 0;
                }
            }
            catch (Exception ex)
            {
                //display error message
                await mapperLog.agregarLogSerilog($"No se pudo conectar a la BD: {ex.Message}",false);

                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> InsertarCopiaCompleta(string nuevaTabla, string tablaActual)
        {
            try {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * INTO "+ nuevaTabla + " FROM " + tablaActual + "", Connection.ConnectionObj()))
                {
                    resultado=cmd.ExecuteNonQuery();
                }
                mConeccion.Close();
                
                if (resultado > 0)
                {
                    await this.AgregarCamposTabla(nuevaTabla);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        //Le agregamos el campo Activo a la tabla de Test
        public async Task<Boolean> AgregarCamposTabla(string unaTabla)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "ALTER TABLE "+unaTabla+ " ADD Activo bit NOT NULL DEFAULT(1)";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.ExecuteNonQuery();
                }
                mConeccion.Close();
                return true;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        //---------------------------------------------------------- PARTE DE SUSCRIPTOR--------------------------------------------------------------------------
        public DataTable ObtenerTodosClientes()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();

                DataTable mDT = new DataTable();
                string query = "SELECT TOP (10) * " +
                               "FROM " + this.mprCBD.tablaDestinoDC  +
                               " order by Cliente ASC ";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    SqlDataAdapter mDA = new SqlDataAdapter(cmd);
                    mDA.Fill(mDT);
                    mConeccion.Close();
                }
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        public DataTable ObtenerIDSClientesAlta()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();

                DataTable mDT = new DataTable();
                string query = "SELECT o.Cliente " +
                               "FROM " + this.mprCBD.tablaOrigenDC + " o " +
                                "Except "+
                                "SELECT t.Cliente "+
                                "FROM " + this.mprCBD.tablaDestinoDC + " t";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    SqlDataAdapter mDA = new SqlDataAdapter(cmd);
                    mDA.Fill(mDT);
                    mConeccion.Close();
                }
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ConsultarDatosCliente(int unId)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();

                DataTable mDT = new DataTable();
                string query = @"SELECT Cliente, MailComercial,SuscriptorActivo,FechaAlta,FechaActualizacion,RazonSocial,Suspendido,
                                                        TimeStamp,Pais,Provincia,TipoSuscriptor,PerIIBB,CUIT 
                                                        FROM " + this.mprCBD.tablaOrigenDC +
                                                        " where Cliente=@unId";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@unId", unId);
                    SqlDataAdapter mDA = new SqlDataAdapter(cmd);
                    mDA.Fill(mDT);
                    mConeccion.Close();
                }
                return mDT;
            }
            catch (Exception ex)    
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ObtenerSuscriptoresBorrados()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                mDA.SelectCommand = Command.CommandObj(@"SELECT Cliente
                                                        FROM " + this.mprCBD.tablaDestinoDC + 
                                                        @" where Activo=1
                                                        EXCEPT
                                                        SELECT Cliente
                                                        FROM " + this.mprCBD.tablaOrigenDC + "", mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        #region Deteccion de Modificaciones
        public DataTable ObtenerSuscriptoresModificados()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                mDA.SelectCommand = Command.CommandObj(@"SELECT Cliente, MailComercial,SuscriptorActivo,FechaAlta,FechaActualizacion,RazonSocial,Suspendido,TimeStamp,Pais,Provincia,TipoSuscriptor,PerIIBB,CUIT 
                                                        FROM " + this.mprCBD.tablaOrigenDC + 
                                                        @" Except
                                                        SELECT Cliente, MailComercial, SuscriptorActivo, FechaAlta, FechaActualizacion, RazonSocial, Suspendido, TimeStamp, Pais, Provincia, TipoSuscriptor, PerIIBB, CUIT
                                                        FROM " + this.mprCBD.tablaDestinoDC + 
                                                        " where Activo = 1", mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ObtenerSuscriptoresModificados(string filtro)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                string miSql = "";
                switch (filtro)
                {
                    case "MailComercial":
                        miSql = @"SELECT Cliente, MailComercial
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, MailComercial
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                    case "CUIT":
                        miSql = @"SELECT Cliente, CUIT
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, CUIT
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                    case "RazonSocial":
                        miSql = @"SELECT Cliente, RazonSocial
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, RazonSocial
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                    case "SuscriptorActivo":
                        miSql = @"SELECT Cliente, SuscriptorActivo
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, SuscriptorActivo
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                    case "Suspendido":
                        miSql = @"SELECT Cliente, Suspendido
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, Suspendido
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                    default:
                        miSql = @"SELECT Cliente, MailComercial,SuscriptorActivo,FechaAlta,FechaActualizacion,RazonSocial,Suspendido,TimeStamp,Pais,Provincia,TipoSuscriptor,PerIIBB,CUIT 
                                FROM " + this.mprCBD.tablaOrigenDC +
                                @" Except
                                SELECT Cliente, MailComercial, SuscriptorActivo, FechaAlta, FechaActualizacion, RazonSocial, Suspendido, TimeStamp, Pais, Provincia, TipoSuscriptor, PerIIBB, CUIT
                                FROM " + this.mprCBD.tablaDestinoDC +
                                " where Activo = 1";
                        break;
                }

                mDA.SelectCommand = Command.CommandObj(miSql, mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        #endregion

        public async Task<Boolean> AltaNuevoCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "INSERT INTO " + this.mprCBD.tablaDestinoDC + " (Cliente,MailComercial, SuscriptorActivo, FechaAlta, FechaActualizacion, RazonSocial ,Suspendido,"+
                                        @"TimeStamp, Pais, Provincia, TipoSuscriptor,PerIIBB, CUIT)
                                VALUES(@idCliente,@mailComercial,@suscriptorActivo,@fechaAlta,@fechaActualizacion,@razonSocial,@suspendido,@timeStamp,@pais,
                                        @provincia,@tipoSuscriptor,@perIIBB,@cuit)";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@mailComercial", clie.mailComercial);
                    cmd.Parameters.AddWithValue("@suscriptorActivo", clie.suscriptorActivo);
                    cmd.Parameters.AddWithValue("@fechaAlta", clie.fechaAlta);
                    cmd.Parameters.AddWithValue("@fechaActualizacion", clie.fechaActualizacion);
                    cmd.Parameters.AddWithValue("@razonSocial", clie.razonSocial);
                    cmd.Parameters.AddWithValue("@suspendido", clie.suspendido);
                    cmd.Parameters.AddWithValue("@timeStamp", clie.timeStamp);
                    cmd.Parameters.AddWithValue("@pais", clie.pais);
                    cmd.Parameters.AddWithValue("@provincia", clie.provincia);
                    cmd.Parameters.AddWithValue("@tipoSuscriptor", clie.tipoSuscriptor);
                    cmd.Parameters.AddWithValue("@perIIBB", clie.perIIBB ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cuit", clie.cuit);

                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD

                }
                mConeccion.Close();

                if (resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        #region Modificaciones
        public async Task<Boolean> ActualizarDatosCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC + 
                    @" set MailComercial=@mailComercial, SuscriptorActivo=@suscriptorActivo,
                    FechaAlta=@fechaAlta, FechaActualizacion=@fechaActualizacion, RazonSocial=@razonSocial ,Suspendido=@suspendido, 
                    TimeStamp=@timeStamp, Pais=@pais, Provincia=@provincia, TipoSuscriptor=@tipoSuscriptor,
                    PerIIBB=@perIIBB, CUIT=@cuit where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@mailComercial", clie.mailComercial);
                    cmd.Parameters.AddWithValue("@suscriptorActivo", clie.suscriptorActivo);
                    cmd.Parameters.AddWithValue("@fechaAlta", clie.fechaAlta);
                    cmd.Parameters.AddWithValue("@fechaActualizacion", clie.fechaActualizacion);
                    cmd.Parameters.AddWithValue("@razonSocial", clie.razonSocial);
                    cmd.Parameters.AddWithValue("@suspendido", clie.suspendido);
                    cmd.Parameters.AddWithValue("@timeStamp", clie.timeStamp);
                    cmd.Parameters.AddWithValue("@pais", clie.pais);
                    cmd.Parameters.AddWithValue("@provincia", clie.provincia);
                    cmd.Parameters.AddWithValue("@tipoSuscriptor", clie.tipoSuscriptor);
                    cmd.Parameters.AddWithValue("@perIIBB", clie.perIIBB ?? (object)DBNull.Value);                    
                    cmd.Parameters.AddWithValue("@cuit", clie.cuit);

                    resultado= cmd.ExecuteNonQuery(); //impacto en la BD
                  
                }
                mConeccion.Close();
               
                if (resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
        
        public async Task<Boolean> ActualizarMailCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC +
                    @" set MailComercial=@mailComercial where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@mailComercial", clie.mailComercial);
                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
        public async Task<Boolean> ActualizarCuitCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC +
                    @" set CUIT=@cuit where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@cuit", clie.cuit);
                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                     await this.ActualizarNovedadesSuscriptor(clie, "Modificacion", "Realizado", null);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarRazonSocialCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC +
                    @" set RazonSocial=@rs where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@rs", clie.razonSocial);
                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                     await this.ActualizarNovedadesSuscriptor(clie, "Modificacion", "Realizado", null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarActivoCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC +
                    @" set SuscriptorActivo=@activo where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@activo", clie.suscriptorActivo);
                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                     await this.ActualizarNovedadesSuscriptor(clie, "Modificacion", "Realizado", null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarSuspendidoCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC +
                    @" set Suspendido=@susp where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@susp", clie.suspendido);
                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                     await this.ActualizarNovedadesSuscriptor(clie, "Modificacion", "Realizado", null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        #endregion
        public async Task<Boolean> EliminarCliente(Cliente clie)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "UPDATE " + this.mprCBD.tablaDestinoDC + 
                                @" set Activo=@activo
                                where Cliente=@idCliente";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@activo", 0);

                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                    await this.ActualizarNovedadesSuscriptor(clie, "Eliminacion", "Realizado", null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarNovedadesSuscriptor(Cliente clie, string tipo, string estado, string response)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = "INSERT INTO " + this.mprCBD.tablaNovedadesSuscriptor +
                                @"(IDCliente, FechaHora, Tipo, TablaOrigen, Estado, Response)
                                VALUES (@idCliente, @fechaHora, @tipo, @tablaOrigen, @estado, @response);";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", clie.idCliente);
                    cmd.Parameters.AddWithValue("@fechaHora", DateTime.Now);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@tablaOrigen", this.mprCBD.tablaOrigenDC);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@response", response);
                   cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                return true;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }




        //----------------------------------------------------------- SECCION DE SUSCRIPCIONES ----------------------------------------------------------------------

        public DataTable ObtenerTodasSuscripciones()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                if (mConeccion.State != ConnectionState.Open && mConeccion.State != ConnectionState.Connecting)
                {
                    mConeccion.Open();
                }

                DataTable mDT = new DataTable();
                string query = "SELECT TOP (10) * " +
                               "FROM " + this.mprCBD.tablaDestinoSA +
                               " order by Cliente ASC ";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    SqlDataAdapter mDA = new SqlDataAdapter(cmd);
                    mDA.Fill(mDT);
                }
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ObtenerIDSSuscripcionesAlta()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                mDA.SelectCommand = Command.CommandObj(@"SELECT o.Cliente,o.Producto
                                                        FROM " + this.mprCBD.tablaOrigenSA + " o "+
                                                        @" Except
                                                        SELECT t.Cliente,t.Producto
                                                         FROM " + this.mprCBD.tablaDestinoSA + " t " +
                                                        @" where t.Activo = 1", mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message" 
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ConsultarDatosSuscripcion(int idClie, int idProd)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();

                DataTable mDT = new DataTable();
                string query = @"SELECT Cliente,Producto,Tema,Vencimiento,Ejecutivo 
                                FROM " + this.mprCBD.tablaOrigenSA + 
                                @" where Cliente=@idClie and Producto=@idProd";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idClie", idClie);
                    cmd.Parameters.AddWithValue("@idProd", idProd);
                    SqlDataAdapter mDA = new SqlDataAdapter(cmd);
                    mDA.Fill(mDT);
                    mConeccion.Close();
                }
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        public DataTable ObtenerSuscripcionesBorradas()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                mDA.SelectCommand = Command.CommandObj(@"SELECT Cliente,Producto
                                                        FROM " + this.mprCBD.tablaDestinoSA + 
                                                        @" where Activo=1
                                                        EXCEPT
                                                        SELECT Cliente,Producto
                                                        FROM " + this.mprCBD.tablaOrigenSA +" ", mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public DataTable ObtenerSuscripcionesModificadas()
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                SqlDataAdapter mDA = new SqlDataAdapter();
                DataTable mDT = new DataTable();
                mDA.SelectCommand = Command.CommandObj(@"SELECT Cliente,Producto,Tema,Vencimiento,Ejecutivo
                                                        FROM " + this.mprCBD.tablaOrigenSA + 
                                                        @" Except
                                                        SELECT Cliente,Producto,Tema,Vencimiento,Ejecutivo
                                                        FROM " + this.mprCBD.tablaDestinoSA +  
                                                        @" where Activo = 1", mConeccion);
                mDA.Fill(mDT);
                mConeccion.Close();
                return mDT;
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        public async Task<Boolean> RegistrarNuevaSuscripcion(Suscripcion unaSuscripcion)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = @"INSERT INTO " + this.mprCBD.tablaDestinoSA + 
                                @" (Cliente,Producto,Tema, Vencimiento, Ejecutivo) 
                                Values(@idCliente,@idProducto,@tema,@unVencimiento,@ejecutivo)";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", unaSuscripcion.idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", unaSuscripcion.idProducto);
                    cmd.Parameters.AddWithValue("@tema", unaSuscripcion.tema);
                    cmd.Parameters.AddWithValue("@unVencimiento", unaSuscripcion.vencimiento);
                    cmd.Parameters.AddWithValue("@ejecutivo", unaSuscripcion.idEjecutivo);

                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD

                }
                mConeccion.Close();

                if (resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarDatosSuscripcion(Suscripcion unaSuscripcion)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = @"UPDATE " + this.mprCBD.tablaDestinoSA +  
                                @" set Tema=@tema, Vencimiento=@unVencimiento, Ejecutivo=@ejecutivo 
                                where Cliente=@idCliente and Producto=@idProducto";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", unaSuscripcion.idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", unaSuscripcion.idProducto);
                    cmd.Parameters.AddWithValue("@tema", unaSuscripcion.tema);
                    cmd.Parameters.AddWithValue("@unVencimiento", unaSuscripcion.vencimiento);
                    cmd.Parameters.AddWithValue("@ejecutivo", unaSuscripcion.idEjecutivo);

                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD

                }
                mConeccion.Close();

                if (resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> EliminarSuscripcion(Suscripcion susc)
        {
            try
            {
                int resultado = 0;
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = @"UPDATE " + this.mprCBD.tablaDestinoSA +  
                                @" set Activo=@activo
                                where Cliente=@idCliente and Producto=@idProducto";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", susc.idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", susc.idProducto);
                    cmd.Parameters.AddWithValue("@activo", 0);

                    resultado = cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                if (resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ActualizarNovedadesSuscripcion(Suscripcion susc, string tipo, string estado, string response)
        {
            try
            {
                SqlConnection mConeccion = Connection.ConnectionObj();
                mConeccion.Open();
                string query = @"INSERT INTO " + this.mprCBD.tablaNovedadesSuscripcion +
                                @"(IDCliente, IDProducto, FechaHora, Tipo, TablaOrigen, Estado, Response)
                                VALUES (@idCliente, @idProducto, @fechaHora, @tipo, @tablaOrigen, @estado, @response);";

                using (SqlCommand cmd = new SqlCommand(query, Connection.ConnectionObj()))
                {
                    cmd.Parameters.AddWithValue("@idCliente", susc.idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", susc.idProducto);
                    cmd.Parameters.AddWithValue("@fechaHora", DateTime.Now);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@tablaOrigen", this.mprCBD.tablaOrigenSA);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@response", response);
                    cmd.ExecuteNonQuery(); //impacto en la BD
                }
                mConeccion.Close();

                return true;
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
