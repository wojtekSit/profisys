using profisys.Web.Data;
using profisys.Web.Models;

public class CsvImportService
{
    private readonly ApplicationDbContext _context;
    public CsvImportService(ApplicationDbContext context) => _context = context;

    public void ImportDocuments(string folderPath)
    {
        var docPath = Path.Combine(folderPath, "Documents.csv");
        var lines = File.ReadAllLines(docPath);
        var docs = new List<Document>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(';');
            if (parts.Length < 6) continue;

            docs.Add(new Document
            {
                Id = int.Parse(parts[0]),
                Type = parts[1],
                Date = DateTime.Parse(parts[2]),
                FirstName = parts[3],
                LastName = parts[4],
                City = parts[5]
            });
        }
        _context.DocumentItems.RemoveRange(_context.DocumentItems);
        _context.Documents.RemoveRange(_context.Documents);
        _context.SaveChanges();

        _context.Documents.AddRange(docs);
        _context.SaveChanges();

        var itemsPath = Path.Combine(folderPath, "DocumentItems.csv");
        var itemLines = File.ReadAllLines(itemsPath);
        var items = new List<DocumentItem>();

        foreach (var line in itemLines.Skip(1))
        {
            var parts = line.Split(';');
            if (parts.Length < 6) continue;

            items.Add(new DocumentItem
            {
                DocumentId = int.Parse(parts[0]),
                Ordinal = int.Parse(parts[1]),
                Product = parts[2],
                Quantity = int.Parse(parts[3]),
                Price = decimal.Parse(parts[4]),
                TaxRate = decimal.Parse(parts[5])
            });
        }

        _context.DocumentItems.AddRange(items);
        _context.SaveChanges();
    }
}
