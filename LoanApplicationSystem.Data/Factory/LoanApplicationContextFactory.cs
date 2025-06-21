using LoanApplicationSystem.Data.Context;
using LoanApplicationSystem.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LoanApplicationSystem.Data.Factory;
internal class LoanApplicationContextFactory : IDesignTimeDbContextFactory<LoanApplicationContext>
{
    public LoanApplicationContext CreateDbContext(string[] args)
    {

        var config = new ConfigurationBuilder()
            .AddBasePath()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LoanApplicationContext>();
        var connectionString = config.GetConnectionString("LoanApplicationContext");

        optionsBuilder.UseSqlServer(connectionString);

        return new LoanApplicationContext(optionsBuilder.Options);
    }

}
