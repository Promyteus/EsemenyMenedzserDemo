using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.DAL;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record CreateEventCommand(CreateEventDto dto) : ICommand<int>;

    public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateEventCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(CreateEventCommand command)
        {
            var ujEsemeny = new DAL.Entities.Event()
            {
                Name = command.dto.Name!,
                Location = command.dto.Location!,
                Country = command.dto.Country,
                Capacity = command.dto.Capacity
            };

            _context.Events.Add(ujEsemeny);
            await _context.SaveChangesAsync();

            return ujEsemeny.Id;
        }
    }
}
