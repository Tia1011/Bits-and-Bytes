using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tiani.P_Bites_Bytes.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name ="Category Name")]
        public string Name { get; set; }

        public Category() 
        {
            Products = new List<Product>();
        }

        //navigational props
        public virtual ICollection<Product> Products { get; set;}
    }
}