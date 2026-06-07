# Biblioteket – Full Stack App

En full stack-applikation för att hantera böcker och författare.  
Byggd med **React** (frontend) och **.NET 9 Web API** (backend) i ett monorepo.

## Teknikstack

| Del | Teknik |
|-----|--------|
| Frontend | React 18, TypeScript, Vite |
| Backend | .NET 9, ASP.NET Core Web API, EF Core |
| Databas | SQL Server / LocalDB |
| Tester | xUnit, NSubstitute |
| CI/CD | GitHub Actions |
| Deploy | GitHub Pages (frontend) |

## Projektstruktur

```
library-app/
├── backend/
│   ├── Library.API/          # Controllers, Program.cs
│   ├── Library.Application/  # Services, Interfaces, DTOs
│   ├── Library.Domain/       # Entiteter
│   ├── Library.Infrastructure/ # DbContext, Repositories
│   └── Library.Tests/        # Enhetstester (xUnit + NSubstitute)
└── frontend/
    └── src/
        ├── components/       # Återanvändbara komponenter
        ├── pages/            # AuthorsPage, BooksPage
        ├── services/         # API-anrop
        └── types/            # TypeScript-typer
```

## Domänmodell

- **Author** (1) → **Book** (många)  
  En författare kan ha många böcker. En bok tillhör en författare.

## Kom igång

### Backend

```bash
cd backend

# Kör migrationer (kräver SQL Server / LocalDB)
dotnet ef database update --project Library.Infrastructure --startup-project Library.API

# Starta API:t
dotnet run --project Library.API
```

API:t körs på `https://localhost:7001`. Swagger finns på `/swagger`.

### Frontend

```bash
cd frontend
cp .env.example .env      # Sätt VITE_API_URL om API:t inte körs lokalt
npm install
npm run dev
```

### Tester

```bash
cd backend
dotnet test
```

## CI/CD

- **`ci.yml`** – Bygger och kör alla tester vid varje push/PR.
- **`deploy.yml`** – Deployer frontend till GitHub Pages vid push till `main`.

### Aktivera GitHub Pages

1. Gå till **Settings → Pages** i ditt GitHub-repo.
2. Välj **Source: GitHub Actions**.
3. Lägg till en secret `VITE_API_URL` med din API-URL.
