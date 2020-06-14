using ProAgil.Domain;
using System.Threading.Tasks;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //GERAL
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAsync();

        //EVENTOS
        Task<Evento[]> GetEventosAsync(bool includePalestrantes);
        Task<Evento> GetEventosAsyncById(int id, bool includePalestrantes);
        Task<Evento[]> GetEventosAsyncByTema(string tema, bool includePalestrantes);

        //PALESTRANTES
        Task<Palestrante> GetPalestranteAsync(int id, bool includeEventos);      
        Task<Palestrante[]> GetPalestrantesAsyncByNome(string nome, bool includeEventos);
    }
}