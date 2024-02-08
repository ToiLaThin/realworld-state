using FluentValidation;

namespace Realworld.Api.Utils.Validation
{
    public class ArticleFeedQueryValidator : AbstractValidator<ArticlesFeedQueryDto>
    {
        public ArticleFeedQueryValidator() {
            RuleFor(x => x.Limit).GreaterThan(0).NotEmpty().WithMessage("Limit must be greater than 0");
            RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).NotEmpty().WithMessage("Offset must be greater than or equal to 0");
        }
    }
}