using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Queries
{
    public record GetEventByIdQuery(int Id) : IQuery<DAL.Entities.Event?>;
    public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, DAL.Entities.Event?>
    {
        private readonly ApplicationDbContext _context;

        public GetEventByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DAL.Entities.Event?> HandleAsync(GetEventByIdQuery query)
        {
            return await _context.Events.FindAsync(query.Id);
        }
    }
}
