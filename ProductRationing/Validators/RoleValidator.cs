using FluentValidation;
using ProductRationing.DAL.Models;

namespace ProductRationing.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 100).WithName("Наименование");
        }
    }
}