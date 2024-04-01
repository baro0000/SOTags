using MediatR;
using Microsoft.AspNetCore.Mvc;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.Validators;

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
        public async Task<IActionResult> GetAllTags()
        {
            logger.LogInformation("Run GetAllTags method");
            var request = new GetTagsRequest();
            return await HandleRequest<GetTagsRequest, GetTagsResponse>(request);
        }

      
        /// <param name="page">Integer</param>
        /// <param name="pageSize">Integer value: 10 / 30 / 50</param>
        /// <param name="sortByName">ASC or DESC</param>
        /// <param name="sortByCount">ASC or DESC</param>
        [HttpGet]
        [Route("Paged")]
        public async Task<IActionResult> GetPagedTags([FromQuery] int page, int pageSize, string? sortByName = null, string? sortByCount = null)
        {
            logger.LogInformation("Run GetAllTags method");
            
            var request = new GetPagedTagsRequest()
            {
                Page = page,
                PageSize = pageSize,
                SortByCount = sortByCount != null ? sortByCount.ToUpper() : null,
                SortByName = sortByName != null ? sortByName.ToUpper() : null
            };

            var validator = new GetPagedTagsRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await HandleRequest<GetPagedTagsRequest, GetPagedTagsResponse>(request);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateDatabase()
        {
            logger.LogInformation("Run UpdateDatabase method");
            var request = new UpdateDatabaseRequest();
            return await HandleRequest<UpdateDatabaseRequest, UpdateDatabaseResponse>(request);
        }
    }
}
