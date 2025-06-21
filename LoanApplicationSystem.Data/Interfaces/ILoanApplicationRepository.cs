using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Domain.Enums;
using System.Threading;

namespace LoanApplicationSystem.Data.Interfaces;
public interface ILoanApplicationRepository
{
    Task<IEnumerable<LoanApplication>> GetAllApplicationsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    Task<LoanApplication> GetApplicationByIdAsync(int id, CancellationToken cancellationToken);
    Task AddApplicationAsync(LoanApplication application, CancellationToken cancellationToken);
    Task UpdateApplicationAsync(LoanApplication application, CancellationToken cancellationToken);
    Task DeleteApplicationAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(LoanStatus status, CancellationToken cancellationToken);
}
