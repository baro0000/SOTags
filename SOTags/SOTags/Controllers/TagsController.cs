using MediatR;
using Microsoft.AspNetCore.Mvc;
using SOTags.ApplicationServices.API.Domain;

namespace SOTags.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ApiControllerBase
    {
        private readonly ILogger<TagsController> logger;

        public TagsController(IMediator mediator, ILogger<TagsController> logger) : base(mediator, logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("")]
        public Task<IActionResult> GetAllTags()
        {
            logger.LogInformation("Run GetAllTags method");
            var request = new GetTagsRequest();
            return HandleRequest<GetTagsRequest, GetTagsResponse>(request);
        }

        [HttpGet]
        [Route("Paged")]
        public Task<IActionResult> GetAllTags([FromQuery] int page, int pageSize, string? sortByName = null, string? sortByCount = null)
        {
            logger.LogInformation("Run GetAllTags method");
            var request = new GetPagedTagsRequest()
            {
                Page = page,
                PageSize = pageSize,
                SortByCount = sortByCount != null ? sortByCount.ToUpper() : null,
                SortByName = sortByName != null ? sortByName.ToUpper() : null
            };
            return HandleRequest<GetPagedTagsRequest, GetPagedTagsResponse>(request);
        }

        [HttpPut]
        [Route("")]
        public Task<IActionResult> UpdateDatabase()
        {
            logger.LogInformation("Run UpdateDatabase method");
            var request = new UpdateDatabaseRequest();
            return HandleRequest<UpdateDatabaseRequest, UpdateDatabaseResponse>(request);
        }
    }
}
