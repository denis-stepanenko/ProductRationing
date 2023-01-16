using FluentValidation;
using ProductRationing.DAL.Models;

namespace ProductRationing.Validators
{
    public class ProductOperationValidator : AbstractValidator<ProductOperation>
    {
        public ProductOperationValidator()
        {
            RuleFor(x => x.ProductCode).NotEmpty().Length(0, 500).WithName("Код продукта");
            RuleFor(x => x.Count).GreaterThan(0).WithName("Количество");
            RuleFor(x => x.OperationId).NotEqual(0).WithName("Операция").WithMessage("'{PropertyName}' не указано значение.");
        }
    }
}