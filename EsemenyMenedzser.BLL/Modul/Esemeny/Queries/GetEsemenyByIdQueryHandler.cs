using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.DAL;

namespace EsemenyMenedzser.BLL.Modul.Esemeny.Queries
{
    public record GetEsemenyByIdQuery(int Id) : IQuery<DAL.Entities.Esemeny?>;
    public class GetEsemenyByIdQueryHandler : IQueryHandler<GetEsemenyByIdQuery, DAL.Entities.Esemeny?>
    {
        private readonly ApplicationDbContext _context;

        public GetEsemenyByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DAL.Entities.Esemeny?> HandleAsync(GetEsemenyByIdQuery query)
        {
            return await _context.Esemenyek.FindAsync(query.Id);
        }
    }
}
