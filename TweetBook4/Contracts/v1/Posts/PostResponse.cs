using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Tags.Response;
using TweetBook4.Domain;

namespace TweetBook4.Contracts.v1.Posts
{
    public class PostResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string userid { get; set; }
        public ICollection<TagResponse> tags { get; set; }
    }

}
