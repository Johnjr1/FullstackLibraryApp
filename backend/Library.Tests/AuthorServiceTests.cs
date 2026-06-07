using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Entities;
using NSubstitute;

namespace Library.Tests;

public class AuthorServiceTests
{
    private readonly IAuthorRepository _repository = Substitute.For<IAuthorRepository>();
    private readonly AuthorService _sut;

    public AuthorServiceTests() => _sut = new AuthorService(_repository);

    [Fact]
    public async Task GetAllAsync_ReturnsAllAuthors()
    {
        // Arrange
        var authors = new List<Author>
        {
            new() { Id = 1, Name = "Astrid Lindgren", Bio = "Swedish author", Books = [] },
            new() { Id = 2, Name = "Stieg Larsson", Bio = "Swedish crime writer", Books = [] }
        };
        _repository.GetAllAsync().Returns(authors);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsAuthor()
    {
        // Arrange
        var author = new Author { Id = 1, Name = "Astrid Lindgren", Bio = "Bio", Books = [] };
        _repository.GetByIdWithBooksAsync(1).Returns(author);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Astrid Lindgren", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdWithBooksAsync(99).Returns((Author?)null);

        // Act
        var result = await _sut.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_CreatesAndReturnsAuthor()
    {
        // Arrange
        var dto = new CreateAuthorDto("New Author", "A new bio");

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Is<Author>(a => a.Name == "New Author"));
        await _repository.Received(1).SaveChangesAsync();
        Assert.Equal("New Author", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Author?)null);

        // Act
        var result = await _sut.UpdateAsync(99, new UpdateAuthorDto("Name", "Bio"));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesAndReturnsTrue()
    {
        // Arrange
        var author = new Author { Id = 1, Name = "Author", Bio = "Bio", Books = [] };
        _repository.GetByIdAsync(1).Returns(author);

        // Act
        var result = await _sut.DeleteAsync(1);

        // Assert
        _repository.Received(1).Delete(author);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Author?)null);

        // Act
        var result = await _sut.DeleteAsync(99);

        // Assert
        Assert.False(result);
    }
}
