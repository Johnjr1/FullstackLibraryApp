using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await bookRepository.GetAllWithAuthorAsync();
        return books.Select(ToDto);
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdWithAuthorAsync(id);
        return book is null ? null : ToDto(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        var author = await authorRepository.GetByIdAsync(dto.AuthorId)
            ?? throw new KeyNotFoundException($"Author {dto.AuthorId} not found.");

        var book = new Book
        {
            Title = dto.Title,
            Isbn = dto.Isbn,
            PublishedYear = dto.PublishedYear,
            AuthorId = author.Id
        };

        await bookRepository.AddAsync(book);
        await bookRepository.SaveChangesAsync();

        book.Author = author;
        return ToDto(book);
    }

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await bookRepository.GetByIdWithAuthorAsync(id);
        if (book is null) return null;

        var author = await authorRepository.GetByIdAsync(dto.AuthorId)
            ?? throw new KeyNotFoundException($"Author {dto.AuthorId} not found.");

        book.Title = dto.Title;
        book.Isbn = dto.Isbn;
        book.PublishedYear = dto.PublishedYear;
        book.AuthorId = author.Id;
        book.Author = author;

        bookRepository.Update(book);
        await bookRepository.SaveChangesAsync();
        return ToDto(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book is null) return false;

        bookRepository.Delete(book);
        await bookRepository.SaveChangesAsync();
        return true;
    }

    private static BookDto ToDto(Book b) =>
        new(b.Id, b.Title, b.Isbn, b.PublishedYear, b.AuthorId, b.Author.Name);
}
