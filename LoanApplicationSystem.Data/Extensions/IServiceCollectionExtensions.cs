using LoanApplicationSystem.Data.Context;
using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplicationSystem.Data.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<LoanApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LoanApplicationContext")));

        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

        return services;

    }
}
