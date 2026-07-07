using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using FluentValidation;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Validators
{
    public class CreateEsemenyCommandValidator : AbstractValidator<CreateEsemenyCommand>
    {
        public CreateEsemenyCommandValidator()
        {
            RuleFor(x => x.dto.Nev)
                .NotEmpty().WithMessage("This field is required.");
            RuleFor(x => x.dto.Helyszin)
                .NotEmpty().WithMessage("This field is required.")
                .MaximumLength(100).WithMessage("The field value must be 100 characters or less.");
            RuleFor(x => x.dto.Kapacitas)
                .GreaterThanOrEqualTo(0).WithMessage("The field value must be a non-negative number.");
        }
    }
}
