using AccessControl.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Factories
{
    public class AccessControlApiApplicationFactory : WebApplicationFactory<AccessControlApiProgram>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(async services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ClayDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ClayDbContext>(options =>
                            options.UseInMemoryDatabase(databaseName: "ClayDbTest"));
            });
            base.ConfigureWebHost(builder);
        }
    }
}
