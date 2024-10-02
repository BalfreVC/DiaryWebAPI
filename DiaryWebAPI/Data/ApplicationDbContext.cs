using DiaryWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DiaryWebAPI.Data;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<DiaryEntry> DiaryEntries { get; set; }

}
