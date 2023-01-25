
using Microsoft.EntityFrameworkCore;

namespace bookParser.Models{
    public class ApplicationDbContext : DbContext
        {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Author>? author { get; set; }
        public DbSet<Book>? book { get; set; }
        public DbSet<BookToAuthor>? bookToAuthor { get; set; }
        public DbSet<BookToTag>? bookToTag { get; set; }
        public DbSet<Tag>? Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity properties and relationships here
        }
    }
}