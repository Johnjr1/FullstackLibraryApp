using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class BookRepository(LibraryDbContext context) : GenericRepository<Book>(context), IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllWithAuthorAsync() =>
        await DbSet.Include(b => b.Author).ToListAsync();

    public async Task<Book?> GetByIdWithAuthorAsync(int id) =>
        await DbSet.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
}
