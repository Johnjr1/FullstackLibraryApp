using Library.Domain.Entities;

namespace Library.Application.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByIdWithBooksAsync(int id);
}
