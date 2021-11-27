using System;
using System.Collections.Generic;

namespace TweetBook4.Contracts.v1.Posts
{
    public class PostRequest
    {
        public string Name { get; set; }
        public ICollection<PostTags> postTags { get; set; }
    }
    public class PostTags 
    {
        public string Name { get; set; }
    }
}
