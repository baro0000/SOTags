using AutoMapper;
using MediatR;
using Sieve.Services;
using SOTags.ApplicationServices.API.Domain;
using SOTags.ApplicationServices.API.ErrorHandling;
using SOTags.DataAccess.CQRS;
using SOTags.DataAccess.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.API.Handlers
{
    public class GetTagsHandler : IRequestHandler<GetTagsRequest, GetTagsResponse>
    {
        private readonly IMapper mapper;
        private readonly IQueryExecutor queryExecutor;

        public GetTagsHandler(IMapper mapper, IQueryExecutor queryExecutor)
        {
            this.mapper = mapper;
            this.queryExecutor = queryExecutor;
        }
        public async Task<GetTagsResponse> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTagsQuery();
            var tags = await queryExecutor.Execute(query);

            var mappedTags = mapper.Map<List<Domain.Models.Tag>>(tags);

            var sumOfAllTagsCount = mappedTags.Sum(tag => tag.Count);
            foreach (var tag in mappedTags)
            {
                var result = (tag.Count * 100.0) / sumOfAllTagsCount;
                tag.Percentage = Math.Round(result, 2);
            }
            //// Try error casting
            //if (mappedTags.Count > 10)
            //{
            //    return new GetTagsResponse
            //    {
            //        Error = new ErrorModel(ErrorType.InternalServerError)
            //    };
            //}

            var response = new GetTagsResponse()
            {
                Data = mappedTags
            };

            return response;
        }
    }
}
