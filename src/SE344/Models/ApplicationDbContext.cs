﻿using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace SE344.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CalendarEvent> Events { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockNote> StockNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>(b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd();
                b.Key(c => c.Id);
                b.ToTable("Chat");
                b.Reference(c => c.Sender).InverseCollection().ForeignKey(c => c.SenderId);
            });

            builder.Entity<CalendarEvent>(b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd();
                b.Key(e => e.Id);
                b.ToTable("Events");
                b.Reference(e => e.User).InverseCollection().ForeignKey(e => e.UserId);
            });

            builder.Entity<StockTransaction>(b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd();
                b.Key(e => e.Id);
                b.Ignore(s => s.TotalPrice);
                b.ToTable("StockTransactions");
                b.Reference(e => e.User).InverseCollection().ForeignKey(e => e.UserId);
            });

            builder.Entity<StockNote>(b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd();
                b.Key(e => e.Id);
                b.ToTable("StockNotes");
                b.Reference(e => e.User).InverseCollection().ForeignKey(e => e.UserId);
            });

            // Other entities config here
        }
    }
}
