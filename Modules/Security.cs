using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Modules
{

    public class Security
    {
        public string Code { get; set; }
        public string Arabic_Name { get; set; }
        public string English_Name { get; set; }
        public string Symbol { get; set; }
        public int Group { get; set; }
        public string Sector { get; set; }
        public double Close_Price { get; set; }
        public int Index_Sample { get; set; }
        public string Isin_Code { get; set; }
        public int Total_Shares { get; set; }
        public double Factor_100 { get; set; }
        public double General_Base_Index { get; set; }
    }
}
