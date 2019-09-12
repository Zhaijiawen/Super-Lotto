using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Lotto.DataSources.Entities
{
    public class HistoryData
    {
        public string Date
        {
            get;
            set;
        }

        public string Issue
        {
            get;
            set;
        }

        public int[] Numbers
        {
            get;
            set;
        }

        public string NumbersString
        {
            get
            {
                return string.Join(",", Numbers);
            }
        }
    }
}
