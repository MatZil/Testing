using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XplicityApp.Dtos.Tags;
using XplicityApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpGet]
        [Produces(typeof(TagDto[]))]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagsService.GetAll();

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags);
        }

        [HttpGet("find/{tagTitle}")]
        [Produces(typeof(TagDto[]))]
        public async Task<IActionResult> Get(string tagTitle)
        {
            var tags = await _tagsService.FindByTitle(tagTitle);

            return Ok(tags);

        }

        [HttpGet("{id}")]
        [Produces(typeof(TagDto))]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await _tagsService.GetById(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [HttpPost]
        [Produces(typeof(int))]
        public async Task<IActionResult> Post(NewTagDto newTagDto)
        {
            try
            {
                var tagId = await _tagsService.Create(newTagDto);

                return Ok(tagId);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ArgumentNullException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
