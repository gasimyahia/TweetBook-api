using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Contracts.v1.Responses
{
    public class AuthFaildResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
