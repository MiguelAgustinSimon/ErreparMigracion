using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SuscriptorEndpoint
    {
        public string subscriber_id { get; set; }
        public string product_id { get; set; }
        public string organization_id { get; set; }
        public string subscriber_name { get; set; }
        public string subscriber_ref_id { get; set; }
        public int subscriber_max_user_count { get; set; }
        public string? creation_date { get; set; }
        public string creation_user { get; set; }
        public string? modification_date { get; set; }
        public string modification_user { get; set; }
        public string idLoginAccount { get; set; }


    }
}
