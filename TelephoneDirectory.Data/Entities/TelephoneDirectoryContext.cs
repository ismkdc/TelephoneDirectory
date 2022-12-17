using Microsoft.EntityFrameworkCore;

namespace TelephoneDirectory.Data.Entities;

public class TelephoneDirectoryContext : DbContext
{
    public TelephoneDirectoryContext(DbContextOptions<TelephoneDirectoryContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactInformation> ContactInformation { get; set; }
    public DbSet<Report> Reports { get; set; }
}