using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.Domain.Models;
using SOTags.ApplicationServices.API.ErrorHandling;
using System.Net;

namespace SOTags.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ApiControllerBase
    {
        private readonly ILogger<TagsController> logger;

        public TagsController(IMediator mediator, ILogger<TagsController> logger, ISieveProcessor sieveProcessor) : base(mediator, logger, sieveProcessor)
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
