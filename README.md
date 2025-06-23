# Loan Application System

This repository contains a simple loan application system built with .NET 9, Entity Framework Core, and Blazor. It demonstrates layered architecture, pagination, and unit testing and EF Core’s in-memory provider.

---

## 📁 Solution Structure

```
LoanApplicationSystem.sln
│
├── LoanApplicationSystem.Data         # EF Core DbContext & Migrations
│   └── LoanApplicationContext.cs      # ApplicationDbContext definition
	├── ILoanApplicationRepository.cs  # Repository interface
│   └── LoanApplicationRepository.cs   # EF Core implementation
│
├── LoanApplicationSystem.Domain       # Domain entities and enums
│   └── LoanApplication.cs             # Entity model
│   └── LoanStatus.cs                  # Status enum
│
├── LoanApplicationSystem.Services     # Service layer & DTOs
│   ├── ILoanService.cs                # Service interface
│   ├── LoanService.cs                 # Service implementation
│   └── Dto/LoanApplicationDto.cs      # Data transfer object
│
├── LoanApplicationSystem.UI           # Blazor UI project
│   ├── Pages/                         # Razor components (e.g. Loans.razor)
│   └── Program.cs                     # Blazor startup
│
└── LoanApplicationSystem.Tests       # Unit tests 
    └── LoanApplicationRepositoryTests.cs
    └── LoanServiceTests.cs
```

---

## 🚀 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022/2023 or VS Code

---

## 🔧 Setup & Build

1. **Restore dependencies**
   ```bash
   dotnet restore
   ```

2. **Apply EF Core Migrations** (for a real database):
   ```bash
   cd LoanApplicationSystem.Data
   dotnet ef database update
   ```

3. **Run the Blazor UI**
   ```bash
   cd LoanApplicationSystem.UI
   dotnet run
   ```

Navigate to `https://localhost:5001/loans` to view the application.

---

## ✅ Running Unit Tests

All tests use xunit and EF Core’s in-memory provider.

```bash
cd LoanApplicationSystem.Tests
dotnet test --framework net9.0
```

- **Repository tests** ensure paging, counting, add, and delete operations.
- **Service tests** mock repositories to verify business logic.

---

## 🛠️ Adding New Tests

1. Create a new test file under `LoanApplicationSystem.Tests`.
2. Use `[Fact]` attributes.
3. For repository tests, spin up an in-memory `LoanApplicationContext`:
   ```csharp
   var options = new DbContextOptionsBuilder<LoanApplicationContext>()
       .UseInMemoryDatabase(Guid.NewGuid().ToString())
       .Options;

   var context = new LoanApplicationContext(options);
   var repository = new LoanApplicationRepository(context);
   ```
4. For service tests, mock `ILoanApplicationRepository` using Moq or a similar framework.


