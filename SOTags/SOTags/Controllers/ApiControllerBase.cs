using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.Domain.Models;
using SOTags.ApplicationServices.API.ErrorHandling;
using SOTags.Sieve;
using System.Net;

namespace SOTags.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<ApiControllerBase> logger;
        private readonly ISieveProcessor sieveProcessor;

        public ApiControllerBase(IMediator mediator, ILogger<ApiControllerBase> logger, ISieveProcessor sieveProcessor)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.sieveProcessor = sieveProcessor;
        }

        protected async Task<IActionResult> HandleRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : ErrorResponseBase
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new { property = x.Key, errors = x.Value.Errors }));
            }

            var response = await mediator.Send(request);
            if (response.Error != null)
            {
                logger.LogError(response.Error.Error);
                return ErrorResponse(response.Error);
            }

            return Ok(response);
        }

        protected async Task<IActionResult> ApplySieve<TRequest, TResponse>(TRequest request, SieveModel query)
            where TRequest : IRequest<TResponse>
            where TResponse : ErrorResponseBase
        {
            throw new NotImplementedException();
        }

        private IActionResult ErrorResponse(ErrorModel errorModel)
        {
            var httpCode = GetHttpStatusCode(errorModel.Error);
            return StatusCode((int)httpCode, errorModel);
        }

        private static HttpStatusCode GetHttpStatusCode(string errorType)
        {
            switch (errorType)
            {
                case ErrorType.NotFound:
                    return HttpStatusCode.NotFound;
                case ErrorType.InternalServerError:
                    return HttpStatusCode.InternalServerError;
                case ErrorType.Unauthorized:
                    return HttpStatusCode.Forbidden;
                case ErrorType.RequestTooLarge:
                    return HttpStatusCode.RequestEntityTooLarge;
                case ErrorType.UnsupportedMediaType:
                    return HttpStatusCode.UnsupportedMediaType;
                case ErrorType.UnsuppoertedMethod:
                    return HttpStatusCode.MethodNotAllowed;
                case ErrorType.TooManyRequests:
                    return (HttpStatusCode)429;
                default:
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}
