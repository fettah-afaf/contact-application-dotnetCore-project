using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContactApplication.Models;
namespace ContactApplication.Data;

public class ApplicationDbContext :  IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public  DbSet<Contact> Contacts { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ContactGroup> ContactGroups { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure ContactGroup entity
        modelBuilder.Entity<ContactGroup>()
            .HasKey(cg => cg.ContactGroupId);

        // Other configurations can be added here if needed
    }
}


