using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.Product
{
    public class RequestPD
    {
        [Display(Name = "編號")]
        public string ID { get; set; }

        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "商品名稱必填")]
        public string Name { get; set; }

        [Display(Name = "重量規格")]
        [Required(ErrorMessage = "重量規格必填")]
        public string Weight { get; set; }

        [Display(Name = "價格")]
        [Required(ErrorMessage = "價格必填")]
        public decimal Price { get; set; }

        [Display(Name = "特價價格")]
        public decimal? SalePrice { get; set; }

        [Display(Name = "商品資訊")]
        public string Information { get; set; }

        [Display(Name = "檢驗報告")]
        public string InpectionReport { get; set; }

        [Display(Name = "上架日期")]
        [Required(ErrorMessage = "連結必填")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "下架日期")]
        public DateTime DownDate { get; set; }

        [Display(Name = "圖片路徑")]
        public string ImgUrl { get; set; }

        [Display(Name = "是否為優惠組合")]
        public bool IsPromotion { get; set; }

        [Display(Name = "上傳商品圖片")]
        public virtual IFormFile file { get; set; }

        [Display(Name = "上傳檢驗報告")]
        public virtual List<IFormFile> Reportfile { get; set; }

        [Display(Name = "購買連結")]
        public string BuyUrl { get; set; }

        [Display(Name = "副標")]
        public string SubTitle { get; set; }

        public string Standard { get; set; }
    }
}
