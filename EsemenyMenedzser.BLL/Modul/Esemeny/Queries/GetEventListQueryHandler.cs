using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Queries
{
    public record GetEventListQuery : IQuery<List<DAL.Entities.Event>>;
    public class GetEventListQueryHandler : IQueryHandler<GetEventListQuery, List<DAL.Entities.Event>>
    {
        private readonly ApplicationDbContext _context;

        public GetEventListQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DAL.Entities.Event>> HandleAsync(GetEventListQuery query)
        {
            return await _context.Events.ToListAsync();
        }
    }
}
    