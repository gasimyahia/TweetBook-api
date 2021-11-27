using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Contracts.v1.Tags.Response
{
    public class TagResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
    }
}
