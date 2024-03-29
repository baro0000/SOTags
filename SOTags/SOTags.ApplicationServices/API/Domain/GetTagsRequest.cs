﻿using MediatR;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.API.Domain
{
    public class GetTagsRequest : IRequest<GetTagsResponse>
    {
    }
}
