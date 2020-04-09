using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.View
{
    public class DefaultView
    {
        public List<MenuView> Menus { get; set; }

        public List<KVView> KeyVisuals { get; set; }

        public List<SeasonalActivityVIew> SeasonalActivities { get; set; }

        public List<HotSaleView> HotSales { get; set; }

        public List<BlogView> Blogs { get; set; }
    }
}
