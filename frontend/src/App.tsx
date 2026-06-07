import { useState } from "react";
import { AuthorsPage } from "./pages/AuthorsPage";
import { BooksPage } from "./pages/BooksPage";

type Page = "authors" | "books";

export function App() {
  const [page, setPage] = useState<Page>("books");

  return (
    <div className="app">
      <nav className="navbar">
        <span className="nav-brand">📚 Biblioteket</span>
        <div className="nav-links">
          <button
            className={`nav-link ${page === "books" ? "active" : ""}`}
            onClick={() => setPage("books")}
          >
            Böcker
          </button>
          <button
            className={`nav-link ${page === "authors" ? "active" : ""}`}
            onClick={() => setPage("authors")}
          >
            Författare
          </button>
        </div>
      </nav>

      <main className="main-content">
        {page === "books" ? <BooksPage /> : <AuthorsPage />}
      </main>
    </div>
  );
}
