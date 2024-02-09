using FluentValidation;

namespace Realworld.Api.Utils.Validation
{
    public class ArticleFeedQueryValidator : AbstractValidator<ArticlesFeedQueryDto>
    {
        public ArticleFeedQueryValidator() {
            RuleFor(x => x.Limit).GreaterThan(0).When(x => x.Limit != default(int)).WithMessage("Limit must be greater than 0"); //for the newman testing is succeed
            RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).When(x => x.Offset != default).WithMessage("Offset must be greater than or equal to 0");
        }
    }
}