using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TradeCore.AuthService.Domain.AppOperationClaimAggregate;
using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Domain.BaseEntity;
using TradeCore.AuthService.Helpers;

namespace TradeCore.AuthService.Repository
{
    public class AuthDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {

        }

        public AuthDbContext() : base()
        {

        }

        #region Domains
        public DbSet<AppCustomer> AppCustomers { get; set; }
        public DbSet<AppOperationClaim> AppOperationClaims { get; set; }
        public DbSet<AppCustomerClaim> AppCustomerClaims { get; set; }
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
            base.OnModelCreating(modelBuilder);
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
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
