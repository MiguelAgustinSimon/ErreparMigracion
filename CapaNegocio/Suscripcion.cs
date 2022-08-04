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
        public string? product_code;
        public string? product_name;
        public string? product_type_id;
        public string? apply_eol;
        public string? apply_ius;

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
