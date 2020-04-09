using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.ProductBanner
{
    public class RequestProductBanner
    {
        [Display(Name = "編號")]
        public int ID { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }

        [Display(Name = "連結")]
        public string Url { get; set; }

        [Display(Name = "圖片連結")]
        public string ImgUrl { get; set; }

        [Display(Name = "上檔日期")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "下檔日期")]
        public DateTime DownDate { get; set; }

        [Display(Name = "上傳檔案")]
        public IFormFile file { get; set; }
    }
}
