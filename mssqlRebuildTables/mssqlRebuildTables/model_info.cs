using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mssqlRebuildTables
{
    public class model_info
    {
        public string dbServerName { get; set; }
        public string dbName { get; set; }
        public string dbUserName { get; set; }
        public string dbUserPass { get; set; }
        public int tryCount { get; set; }
        public int waitTime { get; set; }
        public int timeHour { get; set; }
        public int timeMinute { get; set; }

    }
}
