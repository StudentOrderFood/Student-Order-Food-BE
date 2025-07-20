using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Domain.Base;
using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        // DbSet properties for your entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopImage> ShopImages { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<HistoryTransaction> HistoryTransactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity properties, relationships, etc. here
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.Avatar).HasMaxLength(250);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Dob).IsRequired();
                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.HasMany(r => r.Users)
                      .WithOne(u => u.Role)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("Shops");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ImageUrl).HasMaxLength(250);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.OpenHours).IsRequired();
                entity.Property(e => e.EndHours).IsRequired();
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
                entity.HasOne(s => s.Owner)
                      .WithMany(u => u.Shops)
                      .HasForeignKey(s => s.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict); // tránh multiple cascade
            });

            modelBuilder.Entity<ShopImage>(entity =>
            {
                entity.ToTable("ShopImages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(250);
                entity.HasOne(e => e.Shop)
                      .WithMany(s => s.ShopImages)
                      .HasForeignKey(e => e.ShopId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.ImageUrl).HasMaxLength(250);
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.MenuItems)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Shop)
                      .WithMany(s => s.MenuItems)
                      .HasForeignKey(e => e.ShopId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Vouchers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VoucherCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.HasOne(e => e.Shop)
                      .WithMany(s => s.Vouchers)
                      .HasForeignKey(e => e.ShopId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OrderTime).IsRequired();
                entity.Property(e => e.PaymentMethod).HasMaxLength(20);
                entity.HasOne(o => o.Customer)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); // Chỉ cái này cascade
                entity.HasOne(o => o.Shop)
                      .WithMany(s => s.Orders)
                      .HasForeignKey(o => o.ShopId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Voucher)
                      .WithMany(v => v.Orders)
                      .HasForeignKey(o => o.VoucherId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Note).HasMaxLength(250);
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Item)
                      .WithMany(m => m.OrderItems)
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedbacks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Content).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(250);
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Feedbacks)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.Feedbacks)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict); // tránh multiple cascade
            });

            modelBuilder.Entity<HistoryTransaction>(entity =>
            {
                entity.ToTable("HistoryTransactions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OrderCode).HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PaymentTime).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.HistoryTransactions)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.HistoryTransactions)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        //Auto update timestamps for created and updated entities
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity<Guid>)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity<Guid>.CreatedAt)).IsModified = false;

                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

    }
}
