using Microsoft.EntityFrameworkCore;
using SOTags.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SOTags.DataAccess.CQRS.Commands
{
    public class UpdateDatabaseCommand : CommandBase<List<Tag>, string>
    {
        public async override Task<string> Execute(DatabaseDbContext context)
        {
            foreach (var tag in Parameter)
            {
                var tagFromDb = await context.Tags.SingleOrDefaultAsync(t => t.Name == tag.Name);
                if (tagFromDb != null) 
                {
                    tagFromDb.Count = tag.Count;
                }
                else
                {
                    await context.Tags.AddAsync(tag);
                }
            }
            await context.SaveChangesAsync();

            return "Database has been succesfully updated";
        }
    }
}
