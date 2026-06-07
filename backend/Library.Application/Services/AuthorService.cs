using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class AuthorService(IAuthorRepository repository) : IAuthorService
{
    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        var authors = await repository.GetAllAsync();
        return authors.Select(ToDto);
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await repository.GetByIdWithBooksAsync(id);
        return author is null ? null : ToDto(author);
    }

    public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
    {
        var author = new Author { Name = dto.Name, Bio = dto.Bio };
        await repository.AddAsync(author);
        await repository.SaveChangesAsync();
        return ToDto(author);
    }

    public async Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto dto)
    {
        var author = await repository.GetByIdAsync(id);
        if (author is null) return null;

        author.Name = dto.Name;
        author.Bio = dto.Bio;
        repository.Update(author);
        await repository.SaveChangesAsync();
        return ToDto(author);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await repository.GetByIdAsync(id);
        if (author is null) return false;

        repository.Delete(author);
        await repository.SaveChangesAsync();
        return true;
    }

    private static AuthorDto ToDto(Author a) =>
        new(a.Id, a.Name, a.Bio, a.Books.Count);
}
