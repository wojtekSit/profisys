using Microsoft.EntityFrameworkCore;
using profisys.Web.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentItem> DocumentItems { get; set; }

}
