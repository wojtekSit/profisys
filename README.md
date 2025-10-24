# PROFISYS – Aplikacja rekrutacyjna (.NET 8, ASP.NET Core MVC, EF Core, SQLite)

Aplikacja demonstracyjna przygotowana w ramach zadania rekrutacyjnego dla **PROFISYS Sp. z o.o.**  
Projekt prezentuje pełny cykl pracy z danymi CSV: import, zapis do relacyjnej bazy danych, oraz wizualizację w interfejsie webowym z filtrami i podglądem szczegółów.

---

## Technologie i architektura

- **.NET 8.0 SDK**
- **ASP.NET Core MVC** – warstwa prezentacji i logiki kontrolerów
- **Entity Framework Core (Code First)** – ORM + migracje
- **SQLite** – lekka relacyjna baza danych
- **Bootstrap 5** – stylizacja i układ interfejsu
- **Dependency Injection** – wstrzykiwanie serwisów (`CsvImportService`)
- **IFormFile API** – bezpieczny upload plików CSV

Architektura opiera się o standardowy wzorzec **MVC + warstwa usług (Service Layer)**:

Controllers/
└── DocumentsController.cs
Services/
└── CsvImportService.cs
Models/
├── Document.cs
└── DocumentItem.cs
Data/
└── ApplicationDbContext.cs
Views/
└── Documents/
├── Index.cshtml
└── Details.cshtml
wwwroot/
└── css/site.css

---

## Wymagania środowiskowe

- .NET SDK **8.0 lub nowszy**
- System: Windows / Linux / macOS
- (Opcjonalnie) PowerShell lub Bash

---

## Uruchomienie projektu

1. **Sklonuj repozytorium:**

   ```bash
   git clone <adres-repozytorium>
   cd profisys.Web

   ```

2. **Przywróć zależności:**
   dotnet restore

3. **Utwórz lokalną bazę danych z migracji:**

dotnet ef database update

4. **Uruchom aplikację:**

dotnet run

## Sposób działania

Użytkownik wchodzi na stronę główną aplikacji (/Documents/Index).

W sekcji Import CSV wybiera dwa pliki:
Documents.csv i DocumentItems.csv.

Po przesłaniu pliki są zapisywane tymczasowo w katalogu wwwroot/uploads/.

CsvImportService:

czyści dotychczasowe dane w tabelach,

parsuje oba pliki CSV,

wstawia rekordy do tabel Documents i DocumentItems,

usuwa pliki tymczasowe po imporcie.

Dane są następnie prezentowane w tabeli z filtrami (typ, miasto, nazwisko, data) oraz sortowaniem po (id, data).

Po kliknięciu Szczegóły można zobaczyć pozycje danego dokumentu.

Autor: Wojciech Sitko
