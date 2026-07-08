using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
namespace EsemenyMenedzser.BLL.Modul.Esemeny.Validators
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        private readonly ApplicationDbContext _context;

        // Injecting the DB context for verification
        public UpdateEventCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            //Validating the fields in the DTO
            RuleFor(x => x.Data.Id)
                .NotEmpty().WithMessage("Event ID is required.")
                .MustAsync(EventIsExist).WithMessage("The event with the specified ID does not exist.");

            RuleFor(x => x.Data.Name)
                .NotEmpty().WithMessage("This field is required.");

            RuleFor(x => x.Data.Location)
                .NotEmpty().WithMessage("This field is required.")
                .MaximumLength(100).WithMessage("The field value must be 100 characters or less.");

            RuleFor(x => x.Data.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage("The field value must be a non-negative number.");
        }

        // Helper method for database checks
        private async Task<bool> EventIsExist(int? id, CancellationToken cancellationToken)
        {
            if (!id.HasValue) return false;

            return await _context.Events.AnyAsync(e => e.Id == id.Value, cancellationToken);
        }
    }
}
