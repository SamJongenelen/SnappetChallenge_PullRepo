using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Progress
    {
        public Progress()
        {
            Progression = new Random().Next().ToString();
        }
        private DateTime _startDateTime;
        private DateTime _stopDateTime;

        public string Progression { get; set; }
        public string AvgThisWeek { get; set; }
        public string AvgLastWeek { get; set; }
    }
}
