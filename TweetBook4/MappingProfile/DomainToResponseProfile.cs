using AutoMapper;
using System.Linq;
using TweetBook4.Contracts.v1.Posts;
using TweetBook4.Contracts.v1.Tags.Response;
using TweetBook4.Domain;

namespace TweetBook4.MappingProfile
{
    public class DomainToResponseProfile: Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.tags, opt => opt.MapFrom(src => src.Tags.Select(
                    tag=> new TagResponse { Id=tag.id,Name=tag.name,createdBy=tag.createdBy,createdOn=tag.createdOn})));
            CreateMap<Tag, TagResponse>();
        }
    }
}
