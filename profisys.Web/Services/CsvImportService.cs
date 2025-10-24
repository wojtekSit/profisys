using profisys.Web.Data;
using profisys.Web.Models;
using Microsoft.AspNetCore.Http;

public class CsvImportService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CsvImportService(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public void ImportDocuments(IFormFile documentsCsv, IFormFile documentItemsCsv)
    {
        var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadPath);

        var docsPath = Path.Combine(uploadPath, "Documents.csv");
        var itemsPath = Path.Combine(uploadPath, "DocumentItems.csv");

        using (var stream = new FileStream(docsPath, FileMode.Create))
            documentsCsv.CopyTo(stream);

        using (var stream = new FileStream(itemsPath, FileMode.Create))
            documentItemsCsv.CopyTo(stream);

        var docs = ParseDocuments(docsPath);
        var items = ParseItems(itemsPath);

        _context.DocumentItems.RemoveRange(_context.DocumentItems);
        _context.Documents.RemoveRange(_context.Documents);
        _context.SaveChanges();

        _context.Documents.AddRange(docs);
        _context.SaveChanges();

        _context.DocumentItems.AddRange(items);
        _context.SaveChanges();

        File.Delete(docsPath);
        File.Delete(itemsPath);
    }

    private IEnumerable<Document> ParseDocuments(string path)
    {
        var lines = File.ReadAllLines(path).Skip(1);
        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (parts.Length < 6) continue;
            yield return new Document
            {
                Id = int.Parse(parts[0]),
                Type = parts[1],
                Date = DateTime.Parse(parts[2]),
                FirstName = parts[3],
                LastName = parts[4],
                City = parts[5]
            };
        }
    }

    private IEnumerable<DocumentItem> ParseItems(string path)
    {
        var lines = File.ReadAllLines(path).Skip(1);
        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (parts.Length < 6) continue;
            yield return new DocumentItem
            {
                DocumentId = int.Parse(parts[0]),
                Ordinal = int.Parse(parts[1]),
                Product = parts[2],
                Quantity = int.Parse(parts[3]),
                Price = decimal.Parse(parts[4]),
                TaxRate = decimal.Parse(parts[5])
            };
        }
    }
}
