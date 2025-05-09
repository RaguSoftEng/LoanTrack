# LoanTrack

**LoanTrack** is a lightweight and extensible loan tracking application built for a small finance company in Sri Lanka. The system enables effective tracking and management of loans and installments, and it is fully open source for educational and commercial adaptation.

---

## 🚀 Project Highlights

- Built with **.NET 9**, **Blazor**, and **Entity Framework Core**
- Follows **Clean Architecture** principles
- Uses **PostgreSQL** as the primary database
- Containerized using **Docker** and **Docker Compose**
- Supports orchestration via **.NET Aspire** for development environments
- Dual database contexts: **Identity** and **Application**

---

## 🧱 Project Structure

```
LoanTrack/
├── aspire/
├── src/
│ ├── Core/
| | ├── Application/
│ | ├── Domain/
│ ├── Infrastructure/
│ │ ├── Identity/
│ │ └── Persistence/
│ ├── Presentation/
│ │ └── UI/
│ │     └── Web/
│ |     └── Shared/
├── docker-compose.yml
└── README.md
```

---

## 🐳 Running the Application

You can orchestrate the application in development using one of the following:

### Option 1: Docker Compose

```bash
docker compose up --build
```

### Option 2: .NET Aspire (Preview)

Follow official .NET Aspire documentation to run the solution with service orchestration.

> If you are not using Docker or Aspire, ensure PostgreSQL is running externally and pass the correct connection strings.

---

## 🛠️ Apply Database Migrations

After building the solution and ensuring PostgreSQL is running, apply the migrations for both databases:

### 1. Identity Database

```bash
dotnet ef database update ^
  --project src\Infrastructure\LoanTrack.Identity\LoanTrack.Identity.csproj ^
  --startup-project src\Presentation\UI\Web\LoanTrack.Web\LoanTrack.Web.csproj ^
  --context LoanTrack.Identity.Database.LoanTrackIdentityDbContext ^
  --connection "<your-postgres-connection-string>"
```

### 2. Application Database

```bash
dotnet ef database update ^
  --project src\Infrastructure\LoanTrack.Persistence\LoanTrack.Persistence.csproj ^
  --startup-project src\Presentation\UI\Web\LoanTrack.Web\LoanTrack.Web.csproj ^
  --context LoanTrack.Persistence.Common.Database.ApplicationDbContext ^
  --connection "<your-postgres-connection-string>"
```

---

## 🔐 Default Login

After migrations are complete, you can log in using the default admin credentials:

- **Username:** `ADMIN@EMAIL.COM`
- **Password:** `P@ssw0rd`

---

## 🤝 Contributing & Usage

This project is open source and free to use for:

- Learning and experimentation
- Enhancing and adapting for your own finance system
- Adding features and contributing back

---