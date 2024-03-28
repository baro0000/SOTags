using Microsoft.EntityFrameworkCore;
using SOTags.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.DataAccess.CQRS.Queries
{
    public class GetTagsQuery : QueryBase<List<Entities.Tag>>
    {
        public override Task<List<Tag>> Execute(DatabaseDbContext context)
        {
            return context.Tags.ToListAsync();
        }
    }
}
