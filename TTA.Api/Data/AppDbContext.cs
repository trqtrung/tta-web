using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTA.Api.Models;

namespace TTA.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)

        {
        }

        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseLoggerFactory(MyLoggerFactory); // Warning: Do not create a new ILoggerFactory instance each time

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OrderTracking> OrderTrackings { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OptionList> OptionLists { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<ProductDescription> ProductDescriptions { get; set; }

        public DbSet<BuyingPrice> BuyingPrices { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<SellingPrice> SellingPrices { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasKey(m => m.Id);
            builder.Entity<OrderTracking>().HasKey(m => m.Id);
            builder.Entity<Product>().HasKey(m => m.Id);
            builder.Entity<OptionList>().HasKey(m => m.Id);
            builder.Entity<Supplier>().HasKey(m => m.Id);
            builder.Entity<Brand>().HasKey(m => m.Id);
            builder.Entity<ProductDescription>().HasKey(m => m.Id);
            builder.Entity<BuyingPrice>().HasKey(m => m.Id);
            builder.Entity<Customer>().HasKey(m => m.Id);
            builder.Entity<OrderItem>().HasKey(m => m.Id);
            builder.Entity<SellingPrice>().HasKey(m => m.Id);
            builder.Entity<User>().HasKey(m => m.Id);

            // shadow properties - log date time record been updated
            builder.Entity<Order>().Property<DateTime>("updated_timestamp");
            builder.Entity<OrderItem>().Property<DateTime>("updated_timestamp");
            builder.Entity<OrderTracking>().Property<DateTime>("updated_timestamp");
            builder.Entity<Product>().Property<DateTime>("updated_timestamp");
            builder.Entity<OptionList>().Property<DateTime>("updated_timestamp");
            builder.Entity<Supplier>().Property<DateTime>("updated_timestamp");
            builder.Entity<User>().Property<DateTime>("updated_timestamp");

            builder.Entity<Product>().Property(b => b.Created).ValueGeneratedOnAdd();
            builder.Entity<Order>().Property(b => b.Created).ValueGeneratedOnAdd();          
            builder.Entity<Supplier>().Property(b => b.Created).ValueGeneratedOnAdd();
            builder.Entity<User>().Property(b => b.Created).ValueGeneratedOnAdd();

            //relationship
            builder.Entity<OrderItem>().HasOne(i => i.Order).WithMany(o => o.OrderItems);//1 order has many order items

            builder.Entity<ProductDescription>().HasOne(i => i.Product).WithMany(p => p.ProductDescriptions);

            builder.Entity<SellingPrice>().HasOne(i => i.Product).WithMany(p => p.SellingPrices);

            builder.Entity<BuyingPrice>().HasOne(i => i.Product).WithMany(p => p.BuyingPrices);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            //remember to update UpdatedTimestamp when call savechanges
            updateUpdatedProperty<Order>();
            updateUpdatedProperty<OrderTracking>();
            updateUpdatedProperty<OrderItem>();
            updateUpdatedProperty<Product>();
            updateUpdatedProperty<OptionList>();
            updateUpdatedProperty<Supplier>();
            updateUpdatedProperty<User>();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();

            //remember to update UpdatedTimestamp when call savechanges
            updateUpdatedProperty<Order>();
            updateUpdatedProperty<OrderTracking>();
            updateUpdatedProperty<OrderItem>();
            updateUpdatedProperty<Product>();
            updateUpdatedProperty<OptionList>();
            updateUpdatedProperty<Supplier>();
            updateUpdatedProperty<User>();

            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("updated_timestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
    
}
