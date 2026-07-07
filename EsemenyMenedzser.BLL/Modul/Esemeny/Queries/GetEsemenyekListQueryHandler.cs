using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;
using Microsoft.EntityFrameworkCore;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Queries
{
    public record GetEsemenyekListQuery : IQuery<List<DAL.Entities.Esemeny>>;
    public class GetEsemenyekListQueryHandler : IQueryHandler<GetEsemenyekListQuery, List<DAL.Entities.Esemeny>>
    {
        private readonly ApplicationDbContext _context;

        public GetEsemenyekListQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DAL.Entities.Esemeny>> HandleAsync(GetEsemenyekListQuery query)
        {
            return await _context.Esemenyek.ToListAsync();
        }
    }
}
    