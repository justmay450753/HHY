using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.View
{
    public class ProductListView
    {
        public string ProductID { get; set; }

        public string Link { get; set; }

        public string ImageUrl { get; set; }

        public decimal OriganalPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string ProductName { get; set; }

        public string Weight { get; set; }
    }
}
