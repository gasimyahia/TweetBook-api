using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Domain
{
    public class AuthenticatonResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string RefreshToken { get; internal set; }
    }
}
