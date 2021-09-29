using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Domain;

namespace TweetBook4.Service 
{
    public class PostService : IPostService
    {
        public readonly List<Post> _posts;

        public PostService()
        {
            _posts = new List<Post>();
            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    id = System.Guid.NewGuid(),
                    name = $"Post Name {i}"
                });
            }
        }

        public bool DeletePost(Guid id)
        {
            var post = GetPostById(id);
            if (post == null)
                return false;
            _posts.Remove(post);
            return true;
        }

        public Post GetPostById(Guid postid)
        {
            return _posts.SingleOrDefault(x => x.id == postid);
        }

        public List<Post> GetPosts()
        {
            return _posts;
        }

        public bool UpdatePost(Post PostToUpdate)
        {
            var exists = GetPostById(PostToUpdate.id) != null;
            if (!exists)
                return false;
            var index = _posts.FindIndex(x => x.id == PostToUpdate.id);
            _posts[index] = PostToUpdate;
            return true;
        }
    }
}
