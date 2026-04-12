# KnowledgeBase — Vulnerable Demo App

> ⚠️ **WARNING: This application is intentionally vulnerable. Do NOT deploy to a production environment or expose it to the internet. It is designed solely for educational purposes to demonstrate common web security vulnerabilities.**

## Overview

KnowledgeBase is an ASP.NET Core 8 web application that intentionally demonstrates two classic OWASP Top 10 vulnerabilities:

1. **SQL Injection** — unsanitised user input is concatenated directly into SQL queries
2. **Stored Cross-Site Scripting (XSS)** — user-supplied HTML is persisted to the database and rendered without encoding

The app simulates a simple internal knowledge-base where users can create notes and search them.

---

## Vulnerabilities

### 1. SQL Injection

**Location:** `KnowledgeBase.Data/NoteRepository.cs`

Both database operations build SQL strings using raw user input:

```csharp
// SearchNotes — string concatenation
string sql = "SELECT Id, Header, Body FROM RepositoryNotes WHERE Header LIKE '%" + query + "%'";

// SaveNote — string interpolation
string sql = $"INSERT INTO RepositoryNotes (Header, Body) VALUES ('{header}', '{body}')";
```

**Search exploit example:**
```
%' OR '1'='1
```
This breaks out of the `LIKE` clause and returns all rows, bypassing any intended filtering.

**Insert exploit example (via Body field):**
```
'); DROP TABLE RepositoryNotes; --
```

### 2. Stored Cross-Site Scripting (XSS)

**Location:** `KnowledgeBase.Web/Views/Notes/Index.cshtml`

The note body is rendered using `@Html.Raw()`, which bypasses Razor's built-in HTML encoding:

```cshtml
<div class="card-text">@Html.Raw(item.Body)</div>
```

**Exploit example — enter this as the Body of a new note:**
```html
<script>alert('XSS: ' + document.cookie)</script>
```

Because the payload is saved to the database and rendered for every user who views the page, this is a **stored (persistent) XSS** vulnerability — any visitor will execute the injected script.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server LocalDB

---

## Setup

### 1. Create the database

Run the script against your SQL Server instance:

```sql
-- Setup_Database.sql
CREATE DATABASE InternalStorage;
GO
USE InternalStorage;
GO
CREATE TABLE RepositoryNotes (
    Id INT PRIMARY KEY IDENTITY,
    Header NVARCHAR(100),
    Body NVARCHAR(MAX)
);
GO
INSERT INTO RepositoryNotes (Header, Body) VALUES ('Welcome', 'This is a demo repository.');
```

### 2. Configure the connection string

Edit `KnowledgeBase.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InternalStorage;Trusted_Connection=True;"
  }
}
```

### 3. Build and run

```powershell
cd KnowledgeBase.Web
dotnet run
```

Navigate to `https://localhost:33297` (or `http://localhost:33298`).

---

## Project Structure

```
KnowledgeBase/
├── KnowledgeBase.Data/
│   ├── NoteEntity.cs          # Data model
│   └── NoteRepository.cs      # ⚠️ Vulnerable SQL queries
├── KnowledgeBase.Web/
│   ├── Controllers/
│   │   └── NotesController.cs # MVC controller
│   ├── Views/
│   │   └── Notes/
│   │       └── Index.cshtml   # ⚠️ @Html.Raw() — unencoded output
│   ├── appsettings.json
│   └── Program.cs
└── Setup_Database.sql
```

---

## How to Fix (Remediation Notes)

| Vulnerability | Fix |
|---|---|
| SQL Injection | Use parameterised queries (`SqlParameter`) or an ORM such as Entity Framework Core |
| Stored XSS | Replace `@Html.Raw(item.Body)` with `@item.Body` to let Razor HTML-encode the output |

---

## Disclaimer

This project is provided for **educational and security training purposes only**. The authors are not responsible for any misuse of the techniques demonstrated here.
