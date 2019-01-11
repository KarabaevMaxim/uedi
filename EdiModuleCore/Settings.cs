using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiModuleCore
{
    public class Settings
    {
        public string StartWaybillFolder { get; set; }
        public string DestinationWaybillFolder { get; set; }

        public static string[] DefaultValues
        {
            get
            {
                return new string[]
                {
                    "Waybills",
                    "Archieve"
                };
            }
        }
    }
}
