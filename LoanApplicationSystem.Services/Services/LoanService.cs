using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Domain.Enums;
using LoanApplicationSystem.Services.Dto;
using LoanApplicationSystem.Services.Interfaces;

namespace LoanApplicationSystem.Services.Services;

public sealed class LoanService(ILoanApplicationRepository loanApplicationRepository) : ILoanService
{
    private readonly ILoanApplicationRepository _loanApplicationRepository = loanApplicationRepository;

    public async Task AddApplicationAsync(LoanApplicationDto application, CancellationToken cancellationToken)
    {
        await _loanApplicationRepository.AddApplicationAsync(application.ToEntity(), cancellationToken);
    }

    public async Task DeleteApplicationAsync(int id, CancellationToken cancellationToken)
    {
        await _loanApplicationRepository.DeleteApplicationAsync(id, cancellationToken);
    }

    public async Task<(IEnumerable<LoanApplicationDto> loans, int totalCount)> GetAllApplicationsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _loanApplicationRepository.GetAllApplicationsAsync(pageSize, pageNumber, cancellationToken);
        var loanApplications = result.loans.Select(app => new LoanApplicationDto()
        {
            Id = app.Id,
            ApplicantName = app.ApplicantName,
            LoanAmount = app.LoanAmount,
            ApplicationDate = app.ApplicationDate,
            Status = app.LoanStatus.ToString(),
            InterestRate = app.InterestRate,
            LoanTerm = app.LoanTerm
        }).ToList();

        return (loanApplications, result.count);
    }

    public async Task<LoanApplicationDto> GetApplicationByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _loanApplicationRepository.GetApplicationByIdAsync(id, cancellationToken);
        return new LoanApplicationDto()
        {
            Id = result.Id,
            ApplicantName = result.ApplicantName,
            LoanAmount = result.LoanAmount,
            ApplicationDate = result.ApplicationDate,
            Status = result.LoanStatus.ToString(),
            InterestRate = result.InterestRate,
            LoanTerm = result.LoanTerm
        };
    }

    public async Task<IEnumerable<LoanApplicationDto>> GetApplicationsByStatusAsync(LoanStatus status, CancellationToken cancellationToken)
    {
        var result = await _loanApplicationRepository.GetApplicationsByStatusAsync(status, cancellationToken);
        return result.Select(app => new LoanApplicationDto()
        {
            Id = app.Id,
            ApplicantName = app.ApplicantName,
            LoanAmount = app.LoanAmount,
            ApplicationDate = app.ApplicationDate,
            Status = app.LoanStatus.ToString(),
            InterestRate = app.InterestRate,
            LoanTerm = app.LoanTerm
        }).ToList();
    }

    public Task UpdateApplicationAsync(LoanApplicationDto application, CancellationToken cancellationToken)
    {
        var entity = application.ToEntity();
        return _loanApplicationRepository.UpdateApplicationAsync(entity, cancellationToken);
    }
}
