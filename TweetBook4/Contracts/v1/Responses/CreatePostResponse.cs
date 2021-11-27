using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Tags.Response;
using TweetBook4.Domain;

namespace TweetBook4.Contracts.v1.Responses
{
    public class CreatePostResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string UserId { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; }
    }
}
