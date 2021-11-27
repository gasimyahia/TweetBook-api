using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Requests.Queries;

namespace TweetBook4.Service
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostsUri(PaginationQuery pagination = null);
    }
}
