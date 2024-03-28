using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.Mappings
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            this.CreateMap<SOTags.DataAccess.Entities.Tag, API.Domain.Models.Tag>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Count, y => y.MapFrom(z => z.Count));
        }
    }
}
