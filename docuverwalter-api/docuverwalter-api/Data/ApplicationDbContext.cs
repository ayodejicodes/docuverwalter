using docuverwalter_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace docuverwalter_api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentShareLink> DocumentShareLinks { get; set; }
    }
}
