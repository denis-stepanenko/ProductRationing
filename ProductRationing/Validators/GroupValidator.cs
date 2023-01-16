using FluentValidation;
using ProductRationing.DAL.Models;

namespace ProductRationing.Validators
{
    public class GroupValidator : AbstractValidator<Group>
    {
        public GroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 500).WithName("Наименование");
        }
    }
}