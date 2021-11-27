using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Responses;
using TweetBook4.Contracts.v1.Tags.Request;
using TweetBook4.Contracts.v1.Tags.Response;
using TweetBook4.Contracts.vi;
using TweetBook4.Domain;
using TweetBook4.Extensions;
using TweetBook4.Service;

namespace TweetBook4.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly ITagsService _tagsService;
        private readonly IMapper _mapper;

        public TagsController(ITagsService tagsService,IMapper mapper)
        {
            _tagsService = tagsService;
            _mapper = mapper;
        }

        /// <summary>
        /// returns all tags in the system
        /// </summary>
        /// <response code="200">return all tags in the system</response>
        [HttpGet(ApiRoutes.tags.getAll)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tags = await _tagsService.GetAllTagsAsync();
                if (tags != null)
                {
                    var tagResponse = _mapper.Map<List<TagResponse>>(tags);
                    return Ok(tagResponse); 
                }
                return null;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// returns all tags in the system
        /// </summary>
        /// <response code="200">create tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        [HttpPost(ApiRoutes.tags.Create)]
        public async Task<IActionResult> create([FromBody] TagRequest request)
        {
            try
            {
                Tag newTag = new Tag
                {
                    id = Guid.NewGuid(),
                    name = request.Name,
                    createdOn = DateTime.Now,
                    createdBy = HttpContext.GetUserId()
                };

                var result = await _tagsService.CreateTagAsync(newTag);
                if (result == null)
                {
                    return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create tag" } } }) ;
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUri = baseUrl + "/" + ApiRoutes.tags.Get.Replace("{TagName}", newTag.name);

                var tagResponse = _mapper.Map<TagResponse>(result);

                return Created(locationUri, tagResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet(ApiRoutes.tags.Get)]
        public async Task<IActionResult> get([FromRoute] Guid tagId)
        {
            try
            {
                var result = await _tagsService.GetTagByIdAsync(tagId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<TagResponse>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut(ApiRoutes.tags.Update)]
        public async Task<IActionResult> update([FromRoute] Guid tagId, UpdateTagsRequest tagsRequest)
        {
            try
            {
                var result = await _tagsService.UpdateTagAsync(tagId, tagsRequest);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<TagResponse>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpDelete(ApiRoutes.tags.Delete)]
        public async Task<IActionResult> delete([FromRoute] Guid tagId)
        {
            try
            {
                var deleted = await _tagsService.DeleteTagAsync(tagId);
                if (deleted)
                    return NoContent();
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet(ApiRoutes.tags.Search)]
        public async Task<IActionResult> Search(string name, Guid? tagId, string userId, string createdOn)
        {
            try
            {
                var tags = await _tagsService.SearchAsync(name, tagId, userId,createdOn);
                if (tags.Any())
                {
                    var tagResponse = _mapper.Map<List<TagResponse>>(tags);
                    return Ok(tagResponse);
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
