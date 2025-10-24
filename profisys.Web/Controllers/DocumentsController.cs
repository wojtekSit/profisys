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
    public IActionResult Import(IFormFile documentsCsv, IFormFile documentItemsCsv)
    {
        if (documentsCsv == null || documentItemsCsv == null)
            return BadRequest("Musisz wybrać oba pliki CSV.");

        _importService.ImportDocuments(documentsCsv, documentItemsCsv);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index(
        string? type, string? city, string? lastName,
        string? firstName, string? fromDate, string? toDate,
        string? sort = "dateDesc")
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

        query = sort switch
        {
            "idAsc" => query.OrderBy(d => d.Id),
            "idDesc" => query.OrderByDescending(d => d.Id),
            "dateAsc" => query.OrderBy(d => d.Date),
            _ => query.OrderByDescending(d => d.Date)
        };

        var docs = await query.ToListAsync();
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
