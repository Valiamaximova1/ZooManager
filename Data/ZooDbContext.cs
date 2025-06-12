using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация за many-to-many между Animal и Event
            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Events)
                .WithMany(e => e.Animals);

            // Един Event има много Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId);

            // Един User има много Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId);
        }
    }
}