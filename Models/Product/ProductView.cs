using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.Product
{
    public class ProductView
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string Information { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DownDate { get; set; }
        public string ImgUrl { get; set; }
        public bool IsPromotion { get; set; }
        public string Weight { get; set; }
        public string BuyUrl { get; set; }
        public string SubTitle { get; set; }

        public List<ReportFile> ReportFiles { get; set; }
    }
}
