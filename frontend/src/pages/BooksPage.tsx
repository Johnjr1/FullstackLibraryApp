import { useEffect, useState } from "react";
import { Book, Author, CreateBookDto } from "../types";
import { bookApi, authorApi } from "../services/api";
import { ErrorMessage } from "../components/ErrorMessage";

const emptyForm: CreateBookDto = { title: "", isbn: "", publishedYear: new Date().getFullYear(), authorId: 0 };

export function BooksPage() {
  const [books, setBooks] = useState<Book[]>([]);
  const [authors, setAuthors] = useState<Author[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [editing, setEditing] = useState<Book | null>(null);
  const [form, setForm] = useState<CreateBookDto>(emptyForm);
  const [showForm, setShowForm] = useState(false);

  useEffect(() => { load(); }, []);

  async function load() {
    try {
      const [b, a] = await Promise.all([bookApi.getAll(), authorApi.getAll()]);
      setBooks(b);
      setAuthors(a);
      setError(null);
    } catch {
      setError("Kunde inte hämta böcker. Kontrollera att API:t är igång.");
    }
  }

  function startEdit(book: Book) {
    setEditing(book);
    setForm({ title: book.title, isbn: book.isbn, publishedYear: book.publishedYear, authorId: book.authorId });
    setShowForm(true);
  }

  function resetForm() {
    setEditing(null);
    setForm(emptyForm);
    setShowForm(false);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      if (editing) {
        await bookApi.update(editing.id, form);
      } else {
        await bookApi.create(form);
      }
      resetForm();
      load();
    } catch {
      setError("Kunde inte spara boken. Kontrollera att en författare är vald.");
    }
  }

  async function handleDelete(id: number) {
    if (!confirm("Är du säker?")) return;
    try {
      await bookApi.delete(id);
      load();
    } catch {
      setError("Kunde inte ta bort boken.");
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <h1>Böcker</h1>
        <button className="btn btn-primary" onClick={() => setShowForm(!showForm)}>
          {showForm ? "Avbryt" : "+ Ny bok"}
        </button>
      </div>

      {error && <ErrorMessage message={error} />}

      {showForm && (
        <form className="card form-card" onSubmit={handleSubmit}>
          <h2>{editing ? "Redigera" : "Ny bok"}</h2>
          <label>
            Titel
            <input required value={form.title} onChange={e => setForm(f => ({ ...f, title: e.target.value }))} />
          </label>
          <label>
            ISBN
            <input value={form.isbn} onChange={e => setForm(f => ({ ...f, isbn: e.target.value }))} />
          </label>
          <label>
            Utgivningsår
            <input
              type="number" required min={1000} max={2100}
              value={form.publishedYear}
              onChange={e => setForm(f => ({ ...f, publishedYear: Number(e.target.value) }))}
            />
          </label>
          <label>
            Författare
            <select required value={form.authorId} onChange={e => setForm(f => ({ ...f, authorId: Number(e.target.value) }))}>
              <option value={0} disabled>Välj författare</option>
              {authors.map(a => <option key={a.id} value={a.id}>{a.name}</option>)}
            </select>
          </label>
          <div className="form-actions">
            <button type="submit" className="btn btn-primary">Spara</button>
            <button type="button" className="btn btn-secondary" onClick={resetForm}>Avbryt</button>
          </div>
        </form>
      )}

      <div className="grid">
        {books.map(book => (
          <div key={book.id} className="card">
            <h3>{book.title}</h3>
            <p className="muted">av {book.authorName}</p>
            <p className="muted">{book.isbn} · {book.publishedYear}</p>
            <div className="card-actions">
              <button className="btn btn-secondary" onClick={() => startEdit(book)}>Redigera</button>
              <button className="btn btn-danger" onClick={() => handleDelete(book.id)}>Ta bort</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
