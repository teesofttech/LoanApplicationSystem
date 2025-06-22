using LoanApplicationSystem.Domain.Enums;
using LoanApplicationSystem.Services.Dto;

namespace LoanApplicationSystem.Services.Interfaces;
public interface ILoanService
{
    Task<(IEnumerable<LoanApplicationDto> loans, int totalCount)> GetAllApplicationsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<LoanApplicationDto> GetApplicationByIdAsync(int id, CancellationToken cancellationToken);
    Task AddApplicationAsync(LoanApplicationDto application, CancellationToken cancellationToken);
    Task UpdateApplicationAsync(LoanApplicationDto application, CancellationToken cancellationToken);
    Task DeleteApplicationAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<LoanApplicationDto>> GetApplicationsByStatusAsync(LoanStatus status, CancellationToken cancellationToken);
}
