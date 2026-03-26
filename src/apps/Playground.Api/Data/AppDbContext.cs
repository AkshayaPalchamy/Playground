using Microsoft.EntityFrameworkCore;
using Playground.Api.Models;

namespace Playground.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Documents table
        public DbSet<FileUploadModel> Documents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileUploadModel>().Ignore(x=>x.File);
        }
    }
}