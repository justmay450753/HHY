﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HHY.Models
{
    public partial class HotSaleProducts
    {
        public int ID { get; set; }
        public Guid ProductID { get; set; }
        public string AdvertisingLine { get; set; }
        public string Url { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DownDate { get; set; }
    }
}