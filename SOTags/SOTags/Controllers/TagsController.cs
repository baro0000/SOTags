using MediatR;
using Microsoft.AspNetCore.Mvc;
using SOTags.ApplicationServices.API.Domain;

namespace SOTags.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TagsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTags()
        {
            var request = new GetTagsRequest();
            var response = await mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateDatabase()
        {
            var request = new UpdateDatabaseRequest();
            var response = await mediator.Send(request);
            return Ok(response);
        }
    }
}
