using FluentValidation;
using ProductRationing.DAL.Models;

namespace ProductRationing.Validators
{
    public class OperationValidator : AbstractValidator<Operation>
    {
        public OperationValidator()
        {
            RuleFor(x => x.Department).NotEqual(0).WithName("Цех").WithMessage("'{PropertyName}' не указано значение.");
            RuleFor(x => x.Name).NotEmpty().Length(0, 500).WithName("Наименование");
            RuleFor(x => x.Labor).GreaterThanOrEqualTo(0).WithName("Трудоемкость");
            RuleFor(x => x.Description).Length(0, 3000).WithName("Описание");
            RuleFor(x => x.UnitId).NotEqual(0).WithName("Единица измерения").WithMessage("'{PropertyName}' не указано значение.");
            RuleFor(x => x.GroupId).NotEqual(0).WithName("Группа").WithMessage("'{PropertyName}' не указано значение.");
            RuleFor(x => x.BigOperationId).NotEqual(0).WithName("Укрупненная операция").WithMessage("'{PropertyName}' не указано значение.");
        }
    }
}