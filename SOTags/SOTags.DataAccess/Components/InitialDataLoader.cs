using SOTags.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.DataAccess.Components
{
    public class InitialDataLoader : IInitialDataLoader
    {
        public void LoadData(List<Tag> tagList, DatabaseDbContext context)
        {
            foreach (var tag in tagList)
            {
                context.Tags.Add(tag);
            }
            context.SaveChanges();
        }
    }
}
