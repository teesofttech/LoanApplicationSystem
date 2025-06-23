using LoanApplicationSystem.Data.Context;
using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationSystem.Data.Repositories;

public sealed class LoanApplicationRepository(LoanApplicationContext loanApplicationContext) : ILoanApplicationRepository
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

    public async Task<(IEnumerable<LoanApplication> loans, int count)> GetAllApplicationsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        IQueryable<LoanApplication> apps = _context.LoanApplications;

        int count = await apps.CountAsync(cancellationToken);

        var loans = await apps
            .AsNoTracking()
            .OrderByDescending(a => a.ApplicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (loans, count);

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

        existingApplication.UpdateApplication(
            application.ApplicantName,
            application.LoanAmount,
            application.LoanTerm,
            application.InterestRate);

        _context.LoanApplications.Update(existingApplication);

        if (application.LoanStatus == LoanStatus.Approved)
        {
            existingApplication.Approve();
        }
        else if (application.LoanStatus == LoanStatus.Rejected)
        {
            existingApplication.Reject();
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
