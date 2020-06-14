using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Evento[]> GetEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);

            }

            query = query
                .AsNoTracking()
                .OrderByDescending(evento => evento.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosAsyncById(int id, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);

            }

            query = query
                .AsNoTracking()
                .Where(evento => evento.Id == id)
                .OrderByDescending(evento => evento.DataEvento);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetEventosAsyncByTema(string tema, bool includePalestrantes)
        {

            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query
                .AsNoTracking()
                .Where(evento => evento.Tema.ToLower().Contains(tema.ToLower()))
                .OrderByDescending(evento => evento.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsync(int id, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.OrderBy(palestrante => palestrante.Nome).Where(palestrante=> palestrante.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetPalestrantesAsyncByNome(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.Where(palestrante => palestrante.Nome.ToLower().Contains(nome.ToLower()));
            
            return await query.ToArrayAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
