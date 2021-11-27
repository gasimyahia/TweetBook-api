using System;

namespace TweetBook4.Contracts.v1.Tags.Request
{
    public class TagRequest
    {
        public string Name { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
    }
}
