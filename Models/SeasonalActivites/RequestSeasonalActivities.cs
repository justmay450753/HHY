using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.SeasonalActivites
{
    public class RequestSeasonalActivities
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DownDate { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile file { get; set; }
    }
}
