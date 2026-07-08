using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record UpdateEventCommand(UpdateEventDto Data) : ICommand<bool>;

    public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateEventCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(UpdateEventCommand command)
        {
            // 1. Finding the existing event from the DAL.
            var letezoEsemeny = await _context.Events.FirstAsync(x => x.Id == command.Data.Id);

            // 2. Mapping the updated values.
            letezoEsemeny.Name = command.Data.Name!;
            letezoEsemeny.Location = command.Data.Location!;
            letezoEsemeny.Country = command.Data.Country;
            letezoEsemeny.Capacity = command.Data.Capacity;

            // 3. Saving the changes.
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
