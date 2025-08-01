
namespace WatchCenter.Application.Interface.Repositores
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Genre> Genres { get; }
        IRepository<Content> Contents { get; }
        IRepository<Movie > Movies { get; }
        IRepository<Season> seasons { get; }
        Task<bool> SaveAsync();
    }
}
