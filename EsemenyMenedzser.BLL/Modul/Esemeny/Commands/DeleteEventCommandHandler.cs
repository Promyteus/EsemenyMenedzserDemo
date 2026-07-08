using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record DeleteEventCommand(int Id) : ICommand<bool>;

    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEventCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(DeleteEventCommand command)
        {
            var esemeny = await _context.Events.FirstAsync(x => x.Id == command.Id);

            _context.Events.Remove(esemeny);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
