

namespace WatchCenter.Infrasturcture.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Genre> _Genre;
        private IRepository<Content> _Content;
        private IRepository<Movie> _Movie;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        //----
        public IRepository<Genre> Genres => _Genre ??= new GenericRepository<Genre>(_context);
        public IRepository<Content> Contents => _Content ??= new GenericRepository<Content>(_context);
        public IRepository<Movie> Movies => _Movie ??= new GenericRepository<Movie>(_context);
        //----
        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            
            return result > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
