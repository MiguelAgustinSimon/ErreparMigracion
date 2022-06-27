using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Suscripcion
    {
        public int? idCliente;
        public int? idProducto;
        public int? idEjecutivo;
        public string? tema { get; set; }
        public DateTime? vencimiento { get; set; }

        public Suscripcion(int? cliente, int? idProd)
        {
            idCliente = cliente;
            idProducto = idProd;
        }

        public Suscripcion(int? idClie, int? idProd, string? unTema, DateTime? unVencimiento, int? idEjec)
        {
            idCliente = idClie;
            idProducto = idProd;
            tema = unTema;
            vencimiento = unVencimiento;
            idEjecutivo = idEjec;
            
        }
    }
}
