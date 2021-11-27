using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Requests;
using TweetBook4.Contracts.v1.Requests.Queries;
using TweetBook4.Data;
using TweetBook4.Domain;

namespace TweetBook4.Service 
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _dataContext;

        public PostService(AppDbContext dbContext)
        {
            _dataContext = dbContext;
        }


        public async Task<Post> CreatePostAsync(Post post)
        {
            var result = await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var post = await GetPostByIdAsync(id);
            if (post != null)
            {
                _dataContext.Posts.Remove(post);
                var deleted = await _dataContext.SaveChangesAsync();
                return deleted > 0;
            }

            return false;

        }

        public async Task<Post> GetPostByIdAsync(Guid postid)
        {
            return await _dataContext.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.id == postid);
        }

        public async Task<List<Post>> GetPostsAsync(PostQuery postQuery, PaginationFilter paginationFilter=null)
        {
            var queryable = _dataContext.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(postQuery.userId))
            {
                queryable = queryable.Where(p => p.UserId == postQuery.userId);
            }
            if (paginationFilter==null)
            {
                return await queryable.Include(x => x.Tags).ToListAsync(); 
            }
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(p => p.Tags).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<ICollection<Post>> SearchAsync(string name, Guid? postId, string userId)
        {
            IQueryable<Post> query = _dataContext.Posts;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.name.Contains(name));
            }
            if (postId != null)
            {
                query = query.Where(e => e.id == postId);
            }
            if (userId != null)
            {
                query = query.Where(e => e.UserId == userId);
            }
            return await query.Include(e => e.Tags).ToListAsync();
        }

        public async Task<Post> UpdatePostAsync(Guid postid, UpdatePostRequest request)
        {
            var post = await _dataContext.Posts.Include(e => e.Tags).FirstOrDefaultAsync(x => x.id == postid);
            if (post != null)
            {
                post.name = request.name;
                var result = _dataContext.Posts.Update(post);
                var updated = await _dataContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;

        }

        public async Task<bool> UserOwnsPostAsync(Guid postid, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.id == postid);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
