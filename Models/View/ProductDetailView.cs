using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.View
{
    public class ProductDetailView
    {
        public string ImageUrl { get; set; }

        public decimal OriganalPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string ProductName { get; set; }

        public string Weight { get; set; }

        public string ProductInfo { get; set; }

        public string BuyLink { get; set; }

        public string SubTitle { get; set; }

        public string Standard { get; set; }

        public List<string> ReportFiles { get; set; }

    }
}
