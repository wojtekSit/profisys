using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using profisys.Web.Data;
using profisys.Web.Models;

public class DocumentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CsvImportService _importService;

    public DocumentsController(ApplicationDbContext context, CsvImportService importService)
    {
        _context = context;
        _importService = importService;
    }
    [HttpPost]
    public IActionResult Import(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            return BadRequest("Nieprawidłowa ścieżka do katalogu CSV.");

        _importService.ImportDocuments(path);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Index(
    string? type,
    string? city,
    string? lastName,
    string? firstName,
    string? fromDate,
    string? toDate)
    {
        var query = _context.Documents.AsQueryable();

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(d => d.Type.Contains(type));

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(d => d.City.Contains(city));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(d => d.LastName.Contains(lastName));

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(d => d.FirstName.Contains(firstName));

        if (DateTime.TryParse(fromDate, out var from))
            query = query.Where(d => d.Date >= from);

        if (DateTime.TryParse(toDate, out var to))
            query = query.Where(d => d.Date <= to);

        var docs = await query
            .OrderByDescending(d => d.Date)
            .ToListAsync();

        return View(docs);
    }


    public async Task<IActionResult> Details(int id)
    {
        var doc = await _context.Documents
            .Include(d => d.Items)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doc == null) return NotFound();
        return View(doc);
    }
}
