using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Basket
    {

        public List<OrderLine> OrderLines { get; set; }

        public Basket()
        {
            OrderLines = new List<OrderLine>();
        }


       
    }
}