using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using FluentValidation;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.dto.Name)
                .NotEmpty().WithMessage("This field is required.");
            RuleFor(x => x.dto.Location)
                .NotEmpty().WithMessage("This field is required.")
                .MaximumLength(100).WithMessage("The field value must be 100 characters or less.");
            RuleFor(x => x.dto.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage("The field value must be a non-negative number.");
        }
    }
}
