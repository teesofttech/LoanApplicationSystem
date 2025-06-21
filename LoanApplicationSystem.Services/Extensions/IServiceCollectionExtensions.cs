using LoanApplicationSystem.Services.Interfaces;
using LoanApplicationSystem.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplicationSystem.Services.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServicesLayer(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped<ILoanService, LoanService>();

        return services;
    }
}
