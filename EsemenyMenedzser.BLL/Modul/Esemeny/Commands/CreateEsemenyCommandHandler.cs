using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Commands
{
    public record CreateEsemenyCommand(CreateEsemenyDto dto) : ICommand<int>;

    public class CreateEsemenyCommandHandler : ICommandHandler<CreateEsemenyCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateEsemenyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(CreateEsemenyCommand command)
        {
            var ujEsemeny = new DAL.Entities.Esemeny()
            {
                Nev = command.dto.Nev,
                Helyszin = command.dto.Helyszin,
                Orszag = command.dto.Orszag,
                Kapacitas = command.dto.Kapacitas
            };

            _context.Esemenyek.Add(ujEsemeny);
            await _context.SaveChangesAsync();

            return ujEsemeny.Id;
        }
    }
}
