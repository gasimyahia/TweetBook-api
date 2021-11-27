using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Contracts.v1.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Passwod { get; set; }
    }
}
