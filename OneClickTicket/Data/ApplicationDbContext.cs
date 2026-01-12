using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneClickTicket.Models;

namespace OneClickTicket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OneClickTicket.Models.Booking>? Booking { get; set; }
        public DbSet<OneClickTicket.Models.Cinema>? Cinema { get; set; }
        public DbSet<OneClickTicket.Models.Movie>? Movie { get; set; }
    }
}
