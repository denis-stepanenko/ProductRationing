using FluentValidation;
using ProductRationing.DAL.Models;

namespace ProductRationing.Validators
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        public UnitValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 50).WithName("Наименование");
        }
    }
}