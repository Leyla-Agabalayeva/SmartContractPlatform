# EsignPlatform(SmartContractPlatform)

**A web platform for creating, sharing, and electronically signing contracts — built for Azerbaijan, from individuals to companies.**

EsignPlatform lets a person or a business pick a contract template, fill it in, invite the other party by their FIN or VÖEN, and have everyone sign it electronically with a one-time SMS code. When every party has signed, the contract is sealed and can be exported to a PDF that carries an audit hash. No printing, no scanning, no couriers — just a clean, green, friendly interface that walks you from *draft* to *fully signed*.

This project was built as a full-stack ASP.NET Core 8 MVC application with a classic three-layer (N-tier) architecture, so it's as much a study in clean structure as it is a working product.

---

## Table of contents

- [What it does](#what-it-does)
- [Tech stack](#tech-stack)
- [Architecture](#architecture)
- [Project structure](#project-structure)
- [Getting started](#getting-started)
- [Demo account](#demo-account)
- [How signing works](#how-signing-works)
- [Design](#design)
- [Good to know](#good-to-know)
- [Roadmap](#roadmap)

---

## What it does

- **Two kinds of users.** Individuals register with a FIN; companies register with a VÖEN. The platform knows the difference and treats each accordingly.
- **Ready-made templates.** Four contract types ship out of the box — Rental (*Kirayə*), Service (*Xidmət*), Sale (*Satış*), and Debt (*Borc*) — each with its own set of dynamic fields defined by a JSON schema. Adding a new template is a matter of describing its fields, not writing new code.
- **A guided flow.** Choose a template, fill in the fields, add the counterparty by their FIN or VÖEN, and share it. If that person already has an account, they're linked automatically.
- **A clear status lifecycle.** Every contract moves through *Draft → Pending → Partially Signed → Fully Signed*, or lands in *Rejected* if someone declines.
- **OTP electronic signing.** Signing happens in two steps: request a one-time code, then confirm. Once all parties sign, the contract is sealed.
- **PDF export with an audit trail.** Any contract can be exported to a branded PDF that includes the parties, the field values, and a SHA-256 hash for integrity.
- **An admin panel.** Administrators can activate or deactivate templates so only the ones you want appear to users.

---

## Tech stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 (MVC) |
| Language | C# |
| Data access | Entity Framework Core 8 |
| Database | SQL Server (LocalDB by default; easily switchable to PostgreSQL) |
| Authentication | ASP.NET Core Identity (roles: Admin / User) |
| Object mapping | AutoMapper |
| PDF generation | QuestPDF |
| One-time codes | In-memory cache (MVP — no real SMS gateway yet) |
| Styling | Custom CSS — light theme, emerald-to-teal gradient |

---

## Architecture

The solution is split into three projects, each with a single responsibility. Dependencies only ever point *inward*: the UI knows about the business layer, the business layer knows about the data layer, and the data layer depends on no one.

```
EsignPlatform.UI    →   Controllers, Razor views, wwwroot (the face of the app)
        │
        ▼
EsignPlatform.BLL   →   DTOs, AutoMapper profiles, services (the brains)
        │
        ▼
EsignPlatform.DAL   →   Entities, DbContext, repositories, migrations (the memory)
```

- **DAL (Data Access Layer)** holds the entities, the `AppDbContext`, a generic repository pattern, and the database seeding logic. It's the only layer that talks to the database.
- **BLL (Business Logic Layer)** contains the DTOs the UI works with, the AutoMapper mappings, and all the real logic — creating contracts, generating and validating OTP codes, producing PDFs, and moving contracts through their lifecycle.
- **UI (Presentation Layer)** is the MVC front end: controllers, Razor views, and the styling. It never touches the database directly — it always goes through the business layer.

This separation means you can change the database, swap the PDF library, or redesign the interface without the other layers noticing.

---

## Project structure

```
EsignPlatform/
├── EsignPlatform.DAL/
│   ├── Entities/           # AppUser, Contract, ContractParty, Signature, Template, Document
│   ├── Enums/              # UserType, ContractStatus, PartyRole, SignatureType, TemplateCategory
│   ├── Data/               # AppDbContext, DbInitializer (seeding), DesignTimeDbContextFactory
│   └── Repositories/       # Generic + specific repositories
│
├── EsignPlatform.BLL/
│   ├── DTOs/               # Account, Contract, Template, Signature DTOs
│   ├── Mapper/             # AutoMapper profile
│   └── Services/           # Template, Contract, Signature, OTP, PDF services
│
├── EsignPlatform.UI/
│   ├── Controllers/        # Home, Account, Templates, Contracts, Signing, Admin
│   ├── Views/              # Razor views per controller + shared layout
│   ├── Helpers/            # Azerbaijani label helpers for enums
│   ├── wwwroot/css/        # site.css — the green light theme
│   └── Program.cs          # Dependency injection + middleware pipeline
│
└── EsignPlatform.sln
```

---

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server LocalDB (comes with Visual Studio) — or any SQL Server instance
- Visual Studio 2022 (recommended) or the .NET CLI

### 1. Clone the repository

```bash
git clone https://github.com/Leyla-Agabalayeva/EsignPlatform.git
cd EsignPlatform
```

### 2. Set your connection string

Open `EsignPlatform.UI/appsettings.json`. By default it points to LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EsignPlatformDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

If you're using a named SQL Server instance, change the `Server` value to match (you can copy the exact server name from SQL Server Management Studio).

> **Switching to PostgreSQL?** Add the `Npgsql.EntityFrameworkCore.PostgreSQL` package and replace `UseSqlServer` with `UseNpgsql` in `Program.cs`. The rest of the code stays the same.

### 3. Create the database

The app applies migrations and seeds data automatically on startup, but you need at least one migration to exist. From the Package Manager Console:

```
Default project:  EsignPlatform.DAL
Startup project:  EsignPlatform.UI

PM> Add-Migration InitialCreate
PM> Update-Database
```

Or from the CLI:

```bash
dotnet ef migrations add InitialCreate --project EsignPlatform.DAL --startup-project EsignPlatform.UI
dotnet ef database update --project EsignPlatform.DAL --startup-project EsignPlatform.UI
```

On first run, the seeder creates the roles, the admin account, and the four default templates for you.

### 4. Run it

Set `EsignPlatform.UI` as the startup project and press **F5**, or:

```bash
dotnet run --project EsignPlatform.UI
```

Then open the URL shown in the console (something like `https://localhost:7185`).

---

## Demo account

An administrator is seeded automatically so you can explore the admin panel right away:

```
Email:     admin@esignplatform.az
Password:  Admin123!
```

For everything else, just register a fresh account from the sign-up page.

---

## How signing works

The most satisfying part to try is a full two-party signing. Here's how:

1. Register two separate accounts with **different** FIN/VÖEN values and different emails (open a second account in a private browser window to keep both logged in at once).
2. From the first account, create a contract and enter the second account's FIN/VÖEN as the counterparty.
3. Both accounts now see the contract under **My Contracts**. Each one opens it and signs with their own one-time code.
4. When both have signed, the status flips to **Fully Signed** — and you can download the sealed PDF.

Because the OTP is simulated in this MVP, the code is shown on screen instead of being texted to you, so you can complete the flow entirely on your own machine.

---

## Design

The interface uses a light theme with an emerald-to-teal gradient, soft mint surfaces, and rounded cards — calm and easy on the eyes rather than corporate and cold. Status pills, a signing-progress bar, and consistent spacing are meant to make the state of a contract obvious at a glance.

All styling lives in a single file, `EsignPlatform.UI/wwwroot/css/site.css`, with the entire palette exposed as CSS variables at the top — so re-theming the whole app is a matter of changing a handful of values.

---

## Good to know

- **The OTP is simulated.** There's no SMS gateway wired up yet, so signing codes appear on screen. Swapping in a real provider is a self-contained change in the OTP service.
- **Keep secrets out of source control.** The default setup uses `Trusted_Connection=True` (Windows authentication, no password), which is safe to commit. If you ever put a SQL login and password in `appsettings.json`, move it to [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) or an environment variable instead of committing it.
- **The design-time connection string** in `DesignTimeDbContextFactory` is only used by EF migration tooling. Make sure it points at the same database as `appsettings.json` so migrations and the running app stay in sync.

---

## Roadmap

Ideas that would take this from a solid MVP to a production-ready product:

- **ASAN İmza integration** as a second, legally recognized signature type alongside OTP.
- **A real SMS gateway** so codes are actually delivered to phones.
- **Persisted PDFs** — save each sealed contract's PDF and its SHA-256 hash in the database as a permanent record.
- **An "Incoming for signature" inbox** so users can see everything waiting on them in one place.
- **Email notifications** when a contract is shared, signed, or rejected.

---

*Built with ASP.NET Core 8, a clean three-layer architecture, and a fondness for the color green.*
