using SOTags.DataAccess.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.DataAccess.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly DatabaseDbContext context;

        public QueryExecutor(DatabaseDbContext context)
        {
            this.context = context;
        }

        public Task<TResult> Execute<TResult>(QueryBase<TResult> query)
        {
            return query.Execute(context);
        }
    }
}
