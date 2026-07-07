using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record UpdateEsemenyCommand(UpdateEsemenyDto Data) : ICommand<bool>;

    public class UpdateEsemenyCommandHandler : ICommandHandler<UpdateEsemenyCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateEsemenyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(UpdateEsemenyCommand command)
        {
            // 1. Finding the existing event from the DAL.
            var letezoEsemeny = await _context.Esemenyek.FirstAsync(x => x.Id == command.Data.Id);

            // 2. Mapping the updated values.
            letezoEsemeny.Nev = command.Data.Nev;
            letezoEsemeny.Helyszin = command.Data.Helyszin;
            letezoEsemeny.Orszag = command.Data.Orszag;
            letezoEsemeny.Kapacitas = command.Data.Kapacitas;

            // 3. Saving the changes.
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
