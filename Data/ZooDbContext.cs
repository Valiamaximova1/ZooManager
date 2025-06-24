using Models;
using Microsoft.EntityFrameworkCore;
using Data.Models;
namespace Data
{

    public class ZooDbContext : DbContext
    {
        public ZooDbContext(DbContextOptions<ZooDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TicketTemplate> TicketTemplates => Set<TicketTemplate>();
        public DbSet<TicketPurchase> TicketPurchases => Set<TicketPurchase>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Events)
                .WithMany(e => e.Animals);

            modelBuilder.Entity<TicketTemplate>()
                .HasOne(tt => tt.Event)
                .WithMany(e => e.TicketTemplates)
                .HasForeignKey(tt => tt.EventId);

            modelBuilder.Entity<TicketPurchase>()
                .HasOne(tp => tp.User)
                .WithMany(u => u.TicketPurchases)
                .HasForeignKey(tp => tp.UserId);

            modelBuilder.Entity<TicketPurchase>()
                .HasOne(tp => tp.TicketTemplate)
                .WithMany(tt => tt.TicketPurchases)
                .HasForeignKey(tp => tp.TicketTemplateId);
        }
    }


}