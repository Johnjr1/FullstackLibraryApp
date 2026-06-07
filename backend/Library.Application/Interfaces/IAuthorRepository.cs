using Library.Domain.Entities;

namespace Library.Application.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<IEnumerable<Author>> GetAllWithBooksAsync();
    Task<Author?> GetByIdWithBooksAsync(int id);
}
