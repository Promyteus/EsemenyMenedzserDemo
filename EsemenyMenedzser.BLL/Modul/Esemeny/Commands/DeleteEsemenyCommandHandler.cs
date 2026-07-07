using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record DeleteEsemenyCommand(int Id) : ICommand<bool>;

    public class DeleteEsemenyCommandHandler : ICommandHandler<DeleteEsemenyCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEsemenyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(DeleteEsemenyCommand command)
        {
            var esemeny = await _context.Esemenyek.FirstAsync(x => x.Id == command.Id);

            _context.Esemenyek.Remove(esemeny);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
