# DotDocs (.NET 8, ASP.NET Core MVC, EF Core, SQLite)

## Wymagania

- .NET SDK 8.0
- (Opcjonalnie) PowerShell / Bash

## Uruchomienie (szybko)

1. Sklonuj repo i przejdź do katalogu:
   git clone <twoje-repo>; cd DotDocs
2. Umieść pliki CSV w katalogu `data/` (obok solucji).
3. Start:
   - Windows: scripts\setup-win.bat
   - Linux/macOS: bash scripts/setup.sh
4. Wejdź na https://localhost:5001 (lub http://localhost:5000).

## Format CSV

- Documents.csv: Id,Number,Date,CustomerName,Currency,TotalAmount
- DocumentItems.csv: Id,DocumentId,LineNo,ProductCode,ProductName,Qty,Unit,UnitPrice,LineTotal

Daty w formacie `yyyy-MM-dd`. Pola kwotowe jako liczby dziesiętne z kropką.

## Funkcje

- Import CSV → SQLite (idempotentny: update/insert po Id).
- Tabela dokumentów z filtrami: Nr, Kontrahent, Data od–do, Kwota min–max, stronicowanie.
- Podgląd dokumentu z pozycjami.
- Czyszczenie danych przed importem (opcjonalne).

## Dobre praktyki

- EF Core + migracje, indeksy na kluczowych kolumnach.
- Walidacja importu + ostrzeżenia.
- Oddzielenie warstw: Models / Data / Services / MVC.

## Rozszerzenia (opcjonalnie)

- Eksport do CSV/XLSX.
- Upsert pozycji po (DocumentId, LineNo).
- Testy jednostkowe dla CsvImportService.
