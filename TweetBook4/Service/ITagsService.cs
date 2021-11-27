using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Tags.Request;
using TweetBook4.Domain;

namespace TweetBook4.Service
{
    public interface ITagsService
    {

        Task<Tag> CreateTagAsync(Tag tag);

        Task<Tag> GetTagByIdAsync(Guid tagId);

        Task<Tag> UpdateTagAsync(Guid tagId, UpdateTagsRequest tagsRequest);

        Task<bool> DeleteTagAsync(Guid tagId);
        Task<List<Tag>> GetAllTagsAsync();
        Task<ICollection<Tag>> SearchAsync(string name, Guid? tagId, string userId, string createdOn);
    }
}
