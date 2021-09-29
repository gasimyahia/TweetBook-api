using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Domain;

namespace TweetBook4.Service
{
   public interface IPostService
    {
        List<Post> GetPosts();

        Post GetPostById(Guid postid);

        bool UpdatePost(Post PostToUpdate);

        bool DeletePost(Guid id);
    }
}
