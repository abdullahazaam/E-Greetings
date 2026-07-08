using E_Greetings.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Card - Restrict (Cascade Delete nahi)
            modelBuilder.Entity<Card>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Transaction - Cascade
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Card)
                .WithOne(c => c.Transaction)
                .HasForeignKey<Transaction>(t => t.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Subscription - Cascade
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithOne(u => u.Subscription)
                .HasForeignKey<Subscription>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Birthday", Description = "Happy Birthday cards", IconClass = "fa-birthday-cake", CreatedDate = DateTime.Now },
                new Category { CategoryId = 2, Name = "Wedding", Description = "Wedding anniversary cards", IconClass = "fa-ring", CreatedDate = DateTime.Now },
                new Category { CategoryId = 3, Name = "New Year", Description = "New Year celebration cards", IconClass = "fa-calendar-alt", CreatedDate = DateTime.Now },
                new Category { CategoryId = 4, Name = "Festivals", Description = "Festival celebration cards", IconClass = "fa-gift", CreatedDate = DateTime.Now }
            );
        }
    }
}