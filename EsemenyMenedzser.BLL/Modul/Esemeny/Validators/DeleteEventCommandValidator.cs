using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Validators
{
    public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEventCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Event ID is required.")
                .MustAsync(EventIsExist).WithMessage("The event with the specified ID does not exist.");
        }

        private async Task<bool> EventIsExist(int id, CancellationToken cancellationToken)
        {
            return await _context.Events.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }
}
