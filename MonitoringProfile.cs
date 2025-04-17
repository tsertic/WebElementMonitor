using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebElementMonitor
{
    public class MonitoringProfile
    {
        public string ProfileName { get; set; }
        public string Url { get; set; }
        public string ElementId { get; set; }
        public int IntervalMinutes { get; set; }

        public MonitoringProfile()
        {
            ProfileName = "New Profile";
            Url = "https://";
            ElementId = "content";
            IntervalMinutes = 5;
        }
        public override string ToString()
        {
            return ProfileName ?? string.Empty;
        }

    }
}
