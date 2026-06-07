namespace Library.Application.DTOs;

public record AuthorDto(int Id, string Name, string Bio, int BookCount);

public record CreateAuthorDto(string Name, string Bio);

public record UpdateAuthorDto(string Name, string Bio);
