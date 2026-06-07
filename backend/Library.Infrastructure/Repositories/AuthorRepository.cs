using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class AuthorRepository(LibraryDbContext context) : GenericRepository<Author>(context), IAuthorRepository
{
    public async Task<Author?> GetByIdWithBooksAsync(int id) =>
        await DbSet.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
}
