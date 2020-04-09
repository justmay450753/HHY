using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY_NETCore.Base
{
    public class Common
    {
        public static class Roles
        {
            public const string DIRECTOR = "DIRECTOR";
            public const string SUPERVISOR = "SUPERVISOR";
            public const string ANALYST = "ANALYST";
        }

        public static Dictionary<string, string> Type = new Dictionary<string, string> {
            {"69396A28-110F-4B9E-BAE2-88BEA734E7E1", "甜蜜蜂知識"},
            {"203EBD71-848D-4FC3-B0B9-5FF9C763CDD1","幸福食譜"},
            {"A813A714-5373-4C92-B550-F1234EE8376C","嗡嗡影音" },
            {"296D55EE-26A8-4646-83E1-29F4B33A40E1","部落客推薦" }
        };
    }
}
