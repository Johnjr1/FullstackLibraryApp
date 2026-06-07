using Library.Domain.Entities;

namespace Library.Application.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> GetAllWithAuthorAsync();
    Task<Book?> GetByIdWithAuthorAsync(int id);
}
