import { Author, Book, CreateAuthorDto, CreateBookDto } from "../types";

const BASE_URL = import.meta.env.VITE_API_URL ?? "https://localhost:7001/api";

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...init,
  });
  if (!res.ok) throw new Error(`API error: ${res.status} ${res.statusText}`);
  if (res.status === 204) return undefined as T;
  return res.json();
}

export const authorApi = {
  getAll: () => request<Author[]>("/authors"),
  getById: (id: number) => request<Author>(`/authors/${id}`),
  create: (dto: CreateAuthorDto) =>
    request<Author>("/authors", { method: "POST", body: JSON.stringify(dto) }),
  update: (id: number, dto: CreateAuthorDto) =>
    request<Author>(`/authors/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  delete: (id: number) => request<void>(`/authors/${id}`, { method: "DELETE" }),
};

export const bookApi = {
  getAll: () => request<Book[]>("/books"),
  getById: (id: number) => request<Book>(`/books/${id}`),
  create: (dto: CreateBookDto) =>
    request<Book>("/books", { method: "POST", body: JSON.stringify(dto) }),
  update: (id: number, dto: CreateBookDto) =>
    request<Book>(`/books/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  delete: (id: number) => request<void>(`/books/${id}`, { method: "DELETE" }),
};
