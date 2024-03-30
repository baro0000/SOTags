using FluentValidation;
using SOTags.ApplicationServices.API.Domain;

namespace SOTags.ApplicationServices.API.Validators
{
    internal class GetPagedTagsRequestValidator : AbstractValidator<GetPagedTagsRequest>
    {
        public GetPagedTagsRequestValidator()
        {
            RuleFor(x => x.SortByName)
                .Must(BeValidSortOrder);

            RuleFor(x => x.SortByCount)
                .Must(BeValidSortOrder);
        }

        private bool BeValidSortOrder(string sortOrder)
        {
            return sortOrder == "ASC" || sortOrder == "DESC" || sortOrder == null;
        }
    }
}
