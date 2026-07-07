using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Validators
{
    public class DeleteEsemenyCommandValidator : AbstractValidator<DeleteEsemenyCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEsemenyCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Event ID is required.")
                .MustAsync(EsemenyIsExist).WithMessage("The event with the specified ID does not exist.");
        }

        private async Task<bool> EsemenyIsExist(int id, CancellationToken cancellationToken)
        {
            return await _context.Esemenyek.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }
}
