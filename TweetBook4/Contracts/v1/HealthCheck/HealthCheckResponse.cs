using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Contracts.v1.HealthCheck
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheck> Checks { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class HealthCheck
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
