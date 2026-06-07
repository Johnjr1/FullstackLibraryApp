namespace Library.Application.DTOs;

public record BookDto(int Id, string Title, string Isbn, int PublishedYear, int AuthorId, string AuthorName);

public record CreateBookDto(string Title, string Isbn, int PublishedYear, int AuthorId);

public record UpdateBookDto(string Title, string Isbn, int PublishedYear, int AuthorId);
