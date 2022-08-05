namespace CapaNegocio
{
    public class Cliente
    {
        public int? idCliente;
        public string? mailComercial { get; set; }
        public string? suscriptorActivo { get; set; }
        public DateTime? fechaAlta { get; set; }
        public DateTime? fechaActualizacion { get; set; }
        public string? razonSocial { get; set; }
        public string? suspendido { get; set; }
        public DateTime? timeStamp { get; set; }
        public string? pais { get; set; }
        public string? provincia { get; set; }
        public string? tipoSuscriptor { get; set; }
        public decimal? perIIBB { get; set; }
        public string? cuit { get; set; }
        public string? subscriber_id{ get; set; }
        public int? subscriber_max_user_count { get; set; }
        public string? organization_id { get; set; }

        public Cliente(int? cliente)
        {
            idCliente = cliente;
        }

        public Cliente(int? cliente, string? unMailComercial, string? esSuscriptorActivo, DateTime? unaFechaAlta, DateTime? unaFechaActualizacion, 
            string? unaRazonSocial,string? estaSuspendido, DateTime? unTimeStamp, string? unPais, string? unaProvincia, string? unTipoSuscriptor,
            decimal? unPerIIBB, string? unCuit)
        {
            idCliente = cliente;
            mailComercial = unMailComercial;
            suscriptorActivo = esSuscriptorActivo;
            fechaAlta = unaFechaAlta;
            fechaActualizacion = unaFechaActualizacion;
            razonSocial = unaRazonSocial;
            suspendido = estaSuspendido;
            timeStamp = unTimeStamp;
            pais = unPais;
            provincia = unaProvincia;
            tipoSuscriptor = unTipoSuscriptor;
            perIIBB = unPerIIBB;
            cuit = unCuit;
        }
    }
}