using AccessControl.Api.Models.Entity;
using AccessControl.Api.Models.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Api.Data
{
    public class ClayDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClayDbContext(DbContextOptions<ClayDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Lock> Locks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditableProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private async void SetAuditableProperties()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.Entity as IAuditable != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy =
                            _httpContextAccessor.HttpContext != null
                            && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                            ? _httpContextAccessor.HttpContext.User.Identity.Name
                            : "System";
                        entry.Entity.CreatedDate = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastModifiedBy =
                            _httpContextAccessor.HttpContext != null
                            && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                            ? _httpContextAccessor.HttpContext.User.Identity.Name
                            : "System";
                        entry.Entity.LastModifiedDate = DateTime.Now;
                    }
                }
            }
        }

    }
}
