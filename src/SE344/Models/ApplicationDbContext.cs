using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace SE344.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>(b =>
            {
                b.Key(c => c.Id);
                b.ToTable("Chat");
                b.Reference(c => c.Sender).InverseCollection().ForeignKey(c => c.SenderId);
            });
        }
    }
}
