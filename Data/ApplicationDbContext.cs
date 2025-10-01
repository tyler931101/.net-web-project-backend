using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TicketModel> Tickets { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Configure relation: Ticket -> ApplicationUser
            builder.Entity<TicketModel>()
                .HasOne(t => t.Performer)
                .WithMany()  // we don’t keep a collection of Tickets on ApplicationUser
                .HasForeignKey(t => t.PerformerId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete
        }
    }
}