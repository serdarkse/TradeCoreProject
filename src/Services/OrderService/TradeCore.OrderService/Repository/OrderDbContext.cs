using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TradeCore.OrderService.Domain.BaseEntity;
using TradeCore.OrderService.Domain.CustomerOrderAggregate;
using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Helpers;

namespace TradeCore.OrderService.Repository
{
    public class OrderDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {

        }

        public OrderDbContext() : base()
        {

        }

        #region Domains

        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<Product> Products { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
              .Where(type => !string.IsNullOrEmpty(type.Namespace))
              .Where(type => type.BaseType != null && type.BaseType.IsGenericType
              && type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            modelBuilder.Entity<CustomerOrder>()
               .HasMany(o => o.Products)
               .WithMany(p => p.CustomerOrders)
               .UsingEntity(j => j.ToTable("CustomerOrderProduct")); // Ara tabloyu tanımla


            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureIndex<T>(ModelBuilder modelBuilder, string columnName, bool isUnique, bool isClustered, string indexName) where T : class
        {
            var entityType = modelBuilder.Entity<T>();
            var indexBuilder = entityType.HasIndex(columnName);
            if (isUnique)
                indexBuilder = indexBuilder.IsUnique();
            if (isClustered)
                indexBuilder = indexBuilder.IsClustered();
            if (!string.IsNullOrEmpty(indexName))
                indexBuilder = indexBuilder.HasDatabaseName(indexName);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in added)
            {
                if (entity is Entity track)
                {
                    track.CreatedDate = DateTimeHelper.DateTimeUtcTimeZone();
                }
            }

            var modified = this.ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in modified)
            {
                if (entity is Entity track)
                {
                    track.ModifiedDate = DateTimeHelper.DateTimeUtcTimeZone();
                }
            }
            return base.SaveChangesAsync();
        }


    }
}
