using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Requests.Queries;
using TweetBook4.Contracts.vi;

namespace TweetBook4.Service
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetAllPostsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);
            if (pagination == null)
            {
                return uri;
            }
            var modifiedUri = QueryHelpers.AddQueryString(_baseUri+ApiRoutes.posts.getAll,"pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            return new Uri(modifiedUri);
        }

        public Uri GetPostUri(string postId)
        {
            return new Uri(_baseUri + ApiRoutes.posts.Get.Replace("postId",postId));
        }
    }
}
