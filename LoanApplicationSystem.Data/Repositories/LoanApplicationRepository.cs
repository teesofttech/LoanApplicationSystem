using LoanApplicationSystem.Data.Context;
using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationSystem.Data.Repositories;

internal sealed class LoanApplicationRepository(LoanApplicationContext loanApplicationContext) : ILoanApplicationRepository
{
    private readonly LoanApplicationContext _context = loanApplicationContext;

    public Task AddApplicationAsync(LoanApplication application, CancellationToken cancellationToken)
    {
        _context.LoanApplications.Add(application);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteApplicationAsync(int id, CancellationToken cancellationToken)
    {
        var application = _context.LoanApplications.Find(id);
        if (application == null)
        {
            throw new KeyNotFoundException($"Loan application with ID {id} not found.");
        }
        _context.LoanApplications.Remove(application);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanApplication>> GetAllApplicationsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        return await _context.LoanApplications
            .OrderBy(a => a.ApplicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<LoanApplication> GetApplicationByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.LoanApplications
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Loan application with ID {id} not found.");
    }

    public async Task<IEnumerable<LoanApplication>> GetApplicationsByStatusAsync(LoanStatus status, CancellationToken cancellationToken)
    {
        return await _context.LoanApplications
            .AsNoTracking()
            .Where(a => a.LoanStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateApplicationAsync(LoanApplication application, CancellationToken cancellationToken)
    {
        if (application == null)
        {
            throw new ArgumentNullException(nameof(application), "Application cannot be null.");
        }

        var existingApplication = await _context.LoanApplications.FindAsync(new object[] { application.Id }, cancellationToken);
        if (existingApplication == null)
        {
            throw new KeyNotFoundException($"Loan application with ID {application.Id} not found.");
        }

        if (application == null)
        {
            throw new ArgumentNullException(nameof(application), "Application cannot be null.");
        }

        existingApplication.UpdateApplication(application.ApplicantName, application.LoanAmount, application.LoanTerm, application.InterestRate);

        _context.LoanApplications.Update(existingApplication);

        if (application.LoanStatus == LoanStatus.Approved)
        {
            existingApplication.Approve();
        }
        else if (application.LoanStatus == LoanStatus.Rejected)
        {
            existingApplication.Reject();
        }
        else
        {
            throw new InvalidOperationException("Invalid loan status for update.");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
