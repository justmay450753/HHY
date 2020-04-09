using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models
{
    public class CommonModel
    {
        [Display(Name = "編號")]
        public Guid ID { get; set; }

        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "商品名稱必填")]
        public string ProductName { get; set; }

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

        [Display(Name = "上傳圖片")]
        public virtual IFormFile file { get; set; }

        [Display(Name = "上傳檢驗報告")]
        public virtual List<IFormFile> Reportfile { get; set; }

        [Display(Name = "購買連結")]
        public string BuyUrl { get; set; }

        [Display(Name = "副標")]
        public string SubTitle { get; set; }

        [Display(Name = "標題")]
        [Required(ErrorMessage = "標題必填")]
        public string Title { get; set; }

        [Display(Name = "內容")]
        public string Content { get; set; }

        [Display(Name = "熱銷商品編號")]
        [Required(ErrorMessage = "熱銷商品編號必填")]
        public int ProductID { get; set; }

        [Display(Name = "廣告SLOGN")]
        public string AdvertisingLine { get; set; }

        [Display(Name = "熱銷商品資訊")]
        public string ProductInfo { get; set; }

        [Display(Name = "帳號")]
        [EmailAddress(ErrorMessage = "帳號格式不正確")]
        [Required(ErrorMessage = "帳號必填")]
        public string LoginEmail { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼必填")]
        public string LoginPassword { get; set; }

        [Display(Name = "帳號")]
        [EmailAddress]
        [Required(ErrorMessage = "帳號必填")]

        public string Email { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼必填")]

        public string Password { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }

        [Display(Name = "帳號是否啟用")]
        public bool IsVerify { get; set; }

        [Display(Name = "確認密碼")]
        public string CheckPassword { get; set; }

        [Display(Name = "是否為優惠商品連結")]
        public bool IsProductPromotion { get; set; }

        [Display(Name = "連結")]
        public string Url { get; set; }

        [Display(Name = "規格說明")]
        public string Standard { get; set; }

        [Display(Name = "影片連結ID")]
        public string VideoUrl { get; set; }

        [Display(Name = "副文")]
        public string SubContent { get; set; }

        [Display(Name = "類型")]
        public string Type { get; set; }
    }
}
