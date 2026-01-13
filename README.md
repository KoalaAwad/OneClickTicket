# OneClickTicket

OneClickTicket is a small ASP.NET Core (Razor + MVC) web app for managing cinemas, movies and bookings. It was created as a university/course project and uses ASP.NET Core Identity for authentication and Entity Framework Core (SQL Server) for data storage.

## Features

- CRUD for Movies, Cinemas and Bookings
- Simple validation on models (ranges, required fields)
- Per-entity relationships: Bookings reference Movies and Cinemas
- ASP.NET Core Identity for user registration/login
- Razor views and server-side rendering

## Tech

- .NET 6 / ASP.NET Core
- Entity Framework Core (SQL Server / LocalDB by default)
- ASP.NET Core Identity
- Razor views (MVC)

## Local setup (Windows / PowerShell)

Prerequisites:
- .NET 6 SDK (or compatible runtime)
- SQL Server or LocalDB (LocalDB is used by default in appsettings.json)
- (optional) dotnet-ef global tool to run migrations from the command line

Quick start:

1. Open a PowerShell prompt in the project folder (where the .sln is).

2. Restore and build:

```powershell
dotnet restore
dotnet build
```

3. (Optional) Install EF Core CLI if you need to run migrations from the command line:

```powershell
dotnet tool install --global dotnet-ef
```

4. Apply migrations (the repo already contains a Migrations folder):

```powershell
# From the project folder that contains the .csproj
dotnet ef database update
```

If you prefer Visual Studio, open the solution and use the Package Manager Console (Default project: OneClickTicket) and run `Update-Database`.

5. Run the app:

```powershell
dotnet run --project OneClickTicket\OneClickTicket.csproj
```

6. Open a browser to https://localhost:5001 (or the URL shown in the console).

Notes about Identity and account confirmation:
- The project enables `RequireConfirmedAccount = true` for Identity (in `Program.cs`). That means user accounts normally require email confirmation before sign-in. In development you can either:
  - disable this requirement in `Program.cs` (set `options.SignIn.RequireConfirmedAccount = false`), or
  - manually confirm a seeded user in the database.
 

Special thanks and all credit to the course professor, Dr. Mahmoud Bashayreh for being an amazing educator, and all around cool guy
