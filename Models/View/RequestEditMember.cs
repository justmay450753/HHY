using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HHY.Models.View
{
    public class RequestEditMember
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

        public string chkPassword { get; set; }

        public bool IsVerify { get; set; }
    }
}
