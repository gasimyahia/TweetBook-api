using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Requests;
using TweetBook4.Contracts.v1.Requests.Queries;
using TweetBook4.Domain;

namespace TweetBook4.Service
{
   public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(PostQuery postQuery,PaginationFilter paginationFilter=null);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> GetPostByIdAsync(Guid postid);
        Task<Post> UpdatePostAsync(Guid postid, UpdatePostRequest request);
        Task<bool> DeletePostAsync(Guid id);
        Task<bool> UserOwnsPostAsync(Guid postid, string userId);
        Task<ICollection<Post>> SearchAsync(string name, Guid? postId, string userId);
    }
}
