using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models
{
    public class SeasonalActivitesView
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public int ProductID { get; set; }

        public string ProductInfo { get; set; }

        public string PDImg { get; set; }

        public string Url { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime DownDate { get; set; }
    }
}
