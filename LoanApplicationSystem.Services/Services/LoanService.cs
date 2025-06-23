using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Domain.Enums;
using LoanApplicationSystem.Services.Dto;
using LoanApplicationSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LoanApplicationSystem.Services.Services;

public sealed class LoanService(ILoanApplicationRepository loanApplicationRepository, ILogger<LoanService> logger) : ILoanService
{
    private readonly ILoanApplicationRepository _loanApplicationRepository = loanApplicationRepository;
    private readonly ILogger<LoanService> _logger = logger;

    public async Task AddApplicationAsync(LoanApplicationDto application, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start processing {JsonConvert.SerializeObject(application)}");
        await _loanApplicationRepository.AddApplicationAsync(application.ToEntity(), cancellationToken);
        _logger.LogInformation($"End processing");
    }

    public async Task DeleteApplicationAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start delete processing {id}");
        await _loanApplicationRepository.DeleteApplicationAsync(id, cancellationToken);
        _logger.LogInformation($"End delete processing {id}");
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
        _logger.LogInformation($"Start update processing {JsonConvert.SerializeObject(application)}");

        var entity = application.ToEntity();
        return _loanApplicationRepository.UpdateApplicationAsync(entity, cancellationToken);
    }
}
