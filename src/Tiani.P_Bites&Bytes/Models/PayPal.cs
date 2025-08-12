using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class PayPal : Payment
    {
        public string PaypalEmail { get; set; }
    }
}