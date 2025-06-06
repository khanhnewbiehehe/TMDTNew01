using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favourite> Favorites { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoicesItem { get; set; }
        public DbSet<PriceItem> PriceItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }  
        public DbSet<ProductLaunch> ProductLaunchs { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<FavouriteProduct> FavouriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Favourite)
                .WithOne(p => p.User)
                .HasForeignKey<Favourite>(p => p.UserId);

            modelBuilder.Entity<InvoiceItem>()
                .HasKey(ii => new { ii.InvoiceId, ii.ProductTypeId });

            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId);

            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.ProductType)
                .WithMany(pt => pt.InvoiceItems)
                .HasForeignKey(ii => ii.ProductTypeId);

            modelBuilder.Entity<FavouriteProduct>()
                .HasKey(ii => new { ii.FavouriteId, ii.ProductId });

            modelBuilder.Entity<FavouriteProduct>()
                .HasOne(ii => ii.Favourite)
                .WithMany(i => i.FavouriteProducts)
                .HasForeignKey(ii => ii.FavouriteId);

            modelBuilder.Entity<FavouriteProduct>()
                .HasOne(ii => ii.Product)
                .WithMany(pt => pt.FavouriteProducts)
                .HasForeignKey(ii => ii.ProductId);
        }
    }
}
