using FluentValidation;

namespace Club_System_API.Dtos.QA
{
    public class QARequestValidator:AbstractValidator<QARequest>
    {
        public QARequestValidator()
        {
            RuleFor(x => x.Question).NotEmpty();
            RuleFor(x => x.Answer).NotEmpty();
            RuleFor(x => x.SortNum).NotEmpty();
        }
    }
}
