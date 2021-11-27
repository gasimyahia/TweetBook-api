
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook4.Data;
using TweetBook4.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TweetBook4.Contracts.v1.Tags.Request;
using System.Linq;

namespace TweetBook4.Service
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsService : ITagsService
    {
        private readonly AppDbContext _dataContext;

        public TagsService(AppDbContext appDbContext)
        {
            _dataContext = appDbContext;
        }
        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            var result = await _dataContext.Tags.AddAsync(tag);
            var newTag = await _dataContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteTagAsync(Guid tagId)
        {
            var tag = await _dataContext.Tags.SingleOrDefaultAsync(x => x.id == tagId);
            if (tag == null)
            {
                return false;
            }

            _dataContext.Tags.Remove(tag);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(Guid tagId)
        {
            return  await _dataContext.Tags.SingleOrDefaultAsync(x => x.id == tagId);
        }

        public async Task<ICollection<Tag>> SearchAsync(string name, Guid? tagId, string userId, string createdOn)
        {
            IQueryable<Tag> query = _dataContext.Tags;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.name.Contains(name));
            }
            if (tagId != null)
            {
                query = query.Where(e => e.id == tagId);
            }
            if (userId != null)
            {
                query = query.Where(e => e.createdBy == userId);
            }
            return await query.ToListAsync();
        }

        public async Task<Tag> UpdateTagAsync(Guid tagId, UpdateTagsRequest tagsRequest)
        {
            var tag = await _dataContext.Tags.FirstOrDefaultAsync(e => e.id == tagId);
            if (tag != null)
            {
                tag.name = tagsRequest.name;
                var result = _dataContext.Tags.Update(tag);
                var updated = await _dataContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

    }
}
