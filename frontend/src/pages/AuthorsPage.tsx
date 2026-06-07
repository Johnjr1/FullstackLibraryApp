import { useEffect, useState } from "react";
import { Author, CreateAuthorDto } from "../types";
import { authorApi } from "../services/api";
import { ErrorMessage } from "../components/ErrorMessage";

export function AuthorsPage() {
  const [authors, setAuthors] = useState<Author[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [editing, setEditing] = useState<Author | null>(null);
  const [form, setForm] = useState<CreateAuthorDto>({ name: "", bio: "" });
  const [showForm, setShowForm] = useState(false);

  useEffect(() => { load(); }, []);

  async function load() {
    try {
      setAuthors(await authorApi.getAll());
      setError(null);
    } catch {
      setError("Kunde inte hämta författare. Kontrollera att API:t är igång.");
    }
  }

  function startEdit(author: Author) {
    setEditing(author);
    setForm({ name: author.name, bio: author.bio });
    setShowForm(true);
  }

  function resetForm() {
    setEditing(null);
    setForm({ name: "", bio: "" });
    setShowForm(false);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      if (editing) {
        await authorApi.update(editing.id, form);
      } else {
        await authorApi.create(form);
      }
      resetForm();
      load();
    } catch {
      setError("Kunde inte spara författaren.");
    }
  }

  async function handleDelete(id: number) {
    if (!confirm("Är du säker?")) return;
    try {
      await authorApi.delete(id);
      load();
    } catch {
      setError("Kunde inte ta bort författaren.");
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <h1>Författare</h1>
        <button className="btn btn-primary" onClick={() => setShowForm(!showForm)}>
          {showForm ? "Avbryt" : "+ Ny författare"}
        </button>
      </div>

      {error && <ErrorMessage message={error} />}

      {showForm && (
        <form className="card form-card" onSubmit={handleSubmit}>
          <h2>{editing ? "Redigera" : "Ny författare"}</h2>
          <label>
            Namn
            <input
              required
              value={form.name}
              onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
            />
          </label>
          <label>
            Bio
            <textarea
              value={form.bio}
              onChange={e => setForm(f => ({ ...f, bio: e.target.value }))}
            />
          </label>
          <div className="form-actions">
            <button type="submit" className="btn btn-primary">Spara</button>
            <button type="button" className="btn btn-secondary" onClick={resetForm}>Avbryt</button>
          </div>
        </form>
      )}

      <div className="grid">
        {authors.map(author => (
          <div key={author.id} className="card">
            <h3>{author.name}</h3>
            <p className="muted">{author.bio || "Ingen bio."}</p>
            <span className="badge">{author.bookCount} böcker</span>
            <div className="card-actions">
              <button className="btn btn-secondary" onClick={() => startEdit(author)}>Redigera</button>
              <button className="btn btn-danger" onClick={() => handleDelete(author.id)}>Ta bort</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
