
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Cache;
using TweetBook4.Contracts.v1.Posts;
using TweetBook4.Contracts.v1.Requests;
using TweetBook4.Contracts.v1.Requests.Queries;
using TweetBook4.Contracts.v1.Responses;
using TweetBook4.Contracts.v1.Tags.Response;
using TweetBook4.Contracts.vi;
using TweetBook4.Domain;
using TweetBook4.Extensions;
using TweetBook4.Helpers;
using TweetBook4.Service;

namespace TweetBook4.Controllers.v1
{
    // to make class authenitcatable
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PostsController(IPostService postService,IMapper mapper,IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }


        [HttpGet(ApiRoutes.posts.getAll)]
        //[Cached(600)]
        public async Task<IActionResult> GetPosts([FromQuery] PostQuery postQuery, [FromQuery]PaginationQuery paginationQuery)
        {
            try
            {
                var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
                var posts = await _postService.GetPostsAsync(postQuery,paginationFilter);
                var postsResponse = _mapper.Map<List<PostResponse>>(posts);

                if(paginationFilter==null|| paginationFilter.PageNumber<1 || paginationFilter.PageNumber < 1)
                {
                    return Ok(new PagedResponse<PostResponse>(postsResponse));
                }

                var paginationResponse = PaginationHelpers.CreatePaginationResponse(_uriService, paginationFilter, postsResponse);

                return Ok(paginationResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet(ApiRoutes.posts.Get)]
        //[Cached(600)]
        public async Task<IActionResult> Get([FromRoute] Guid postid)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(postid);
                if (post == null)
                    return NotFound();
                return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPut(ApiRoutes.posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postid, UpdatePostRequest request)
        {
            try
            {
                var UserOwnsPost = await _postService.UserOwnsPostAsync(postid, HttpContext.GetUserId());
                if (!UserOwnsPost)
                {
                    return BadRequest(new { error = "You do not own this post" });
                }

                var updatedPost = await _postService.UpdatePostAsync(postid, request);
                if (updatedPost != null)
                    return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(updatedPost)));
                return null;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPost(ApiRoutes.posts.Create)]
        public async Task<IActionResult> Create([FromBody] PostRequest postRequest)
        {
            try
            {
                var newPostId = Guid.NewGuid();
                var useid = HttpContext.GetUserId();
                var tags = postRequest.postTags.Select(x => new Tag { id = Guid.NewGuid(), name = x.Name, createdBy = useid, createdOn = DateTime.Now }).ToList();
                var post = new Post
                {
                    id = newPostId,
                    name = postRequest.Name,
                    UserId = useid,
                    Tags = tags,
                    PostTags = tags.Select(x => new PostTag { PostId = newPostId, TagId = x.id }).ToList()
                };

                var result = await _postService.CreatePostAsync(post);
                // to get url 
                //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                //var locationUri = baseUrl + "/" + ApiRoutes.posts.Get.Replace("{postid}", post.id.ToString());

                var locationUri = _uriService.GetPostUri(post.id.ToString());


                var response = new Response<PostResponse>(_mapper.Map<PostResponse>(result));
                return Created(locationUri, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpDelete(ApiRoutes.posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postid)
        {
            try
            {
                var UserOwnsPost = await _postService.UserOwnsPostAsync(postid, HttpContext.GetUserId());
                if (!UserOwnsPost)
                {
                    return BadRequest(new { error = "You do not own this post" });
                }
                var deleted = await _postService.DeletePostAsync(postid);
                if (deleted)
                    return NoContent();
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet(ApiRoutes.posts.Search)]
        public async Task<IActionResult> Search(string name, Guid? postid, string userId)
        {
            try
            {
                var posts = await _postService.SearchAsync(name, postid, userId);
                if (posts.Any())
                {
                    var postsResponse =new Response<List<PostResponse>>( _mapper.Map<List<PostResponse>>(posts));
                    return Ok(postsResponse);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
