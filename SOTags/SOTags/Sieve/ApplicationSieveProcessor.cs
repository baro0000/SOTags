using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using SOTags.ApplicationServices.API.Domain.Models;

namespace SOTags.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Tag>(e => e.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Tag>(e => e.Count)
                .CanSort();

            mapper.Property<Tag>(e=>e.Percentage)
                .CanSort();

            return mapper;
        }
    }
}
