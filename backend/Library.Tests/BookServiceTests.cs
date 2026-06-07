using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Entities;
using NSubstitute;

namespace Library.Tests;

public class BookServiceTests
{
    private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
    private readonly IAuthorRepository _authorRepository = Substitute.For<IAuthorRepository>();
    private readonly BookService _sut;

    public BookServiceTests() => _sut = new BookService(_bookRepository, _authorRepository);

    [Fact]
    public async Task CreateAsync_ValidDto_CreatesAndReturnsBook()
    {
        // Arrange
        var author = new Author { Id = 1, Name = "Astrid Lindgren", Bio = "Bio", Books = [] };
        var dto = new CreateBookDto("Pippi Långstrump", "978-0", 1945, 1);
        _authorRepository.GetByIdAsync(1).Returns(author);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        await _bookRepository.Received(1).AddAsync(Arg.Is<Book>(b => b.Title == "Pippi Långstrump"));
        Assert.Equal("Pippi Långstrump", result.Title);
        Assert.Equal("Astrid Lindgren", result.AuthorName);
    }

    [Fact]
    public async Task CreateAsync_NonExistingAuthor_ThrowsKeyNotFoundException()
    {
        // Arrange
        _authorRepository.GetByIdAsync(99).Returns((Author?)null);
        var dto = new CreateBookDto("Book", "000", 2000, 99);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CreateAsync(dto));
    }
}
