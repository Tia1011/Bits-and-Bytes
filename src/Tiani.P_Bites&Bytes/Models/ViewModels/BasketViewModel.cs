using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models.ViewModels
{
    public class BasketViewModel
    {

        public List<OrderLine> OrderLines { get; set; }

        public BasketViewModel()
        {
            OrderLines = new List<OrderLine>();
        }
    }
}