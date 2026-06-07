export interface Author {
  id: number;
  name: string;
  bio: string;
  bookCount: number;
}

export interface Book {
  id: number;
  title: string;
  isbn: string;
  publishedYear: number;
  authorId: number;
  authorName: string;
}

export interface CreateAuthorDto {
  name: string;
  bio: string;
}

export interface CreateBookDto {
  title: string;
  isbn: string;
  publishedYear: number;
  authorId: number;
}
