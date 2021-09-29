
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TweetBook4.Contracts.v1.Requests;
using TweetBook4.Contracts.v1.Responses;
using TweetBook4.Contracts.vi;
using TweetBook4.Domain;
using TweetBook4.Service;

namespace TweetBook4.Controllers.v1
{
    public class PostsController : Controller
    {

        private IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }


        [HttpGet(ApiRoutes.posts.getAll)]
        public IActionResult GetPosts()
        {
            return Ok(_postService.GetPosts());
        }

        [HttpGet(ApiRoutes.posts.Get)]
        public IActionResult Get([FromRoute]Guid postid)
        {
            var post = _postService.GetPostById(postid);
            if (post == null)
                return NotFound();
            return Ok(post);
        }


        [HttpPut(ApiRoutes.posts.Update)]
        public IActionResult Update([FromRoute] Guid postid,UpdatePostRequest request)
        {
            var post = new Post
            {
                id = postid,
                name = request.name
            };
            var update = _postService.UpdatePost(post);
            if (update)
                return Ok(post);

            return NotFound();
            
        }


        [HttpPost(ApiRoutes.posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { id = postRequest.id };
            if (post.id != Guid.Empty)
                post.id = Guid.NewGuid();

            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.posts.Get.Replace("{postid}", post.id.ToString());

            var response = new CreatePostResponse { id = post.id };
            return Created(locationUri,response);
        }

        [HttpDelete(ApiRoutes.posts.Delete)]
        public IActionResult Delete([FromRoute]Guid postid)
        {
            var deleted = _postService.DeletePost(postid);
            if (deleted)
                return NoContent();
            return NotFound();
        }
    }
}
