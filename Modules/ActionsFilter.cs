using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Modules
{
    public class ActionsFilter
    {
        public string status { get; set; }
        public string Name { get; set; }
        public string entry_date_from_FilterText { get; set; }
        public string entry_date_to_FilterText { get; set; }



        public string source_file_date_from_FilterText { get; set; }
        public string source_file_date_to_FilterText { get; set; }

        public bool isHistory { get; set; }

        public bool isToday { get; set; }
    }
}
