using FluentAssertions;
using LoanApplicationSystem.Data.Context;
using LoanApplicationSystem.Data.Repositories;
using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationSystem.Test;

public class LoanApplicationRepositoryTests
{
    LoanApplicationContext _context;
    LoanApplicationRepository _repository;

    /// <summary>
    /// Setup method to initialize the in-memory database and seed data.
    /// </summary>
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LoanApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new LoanApplicationContext(options);
        _repository = new LoanApplicationRepository(_context);

        for (int i = 1; i <= 25; i++)
        {
            _context.LoanApplications.Add(new LoanApplication
            {
                Id = i,
                ApplicantName = $"Test{i}",
                LoanAmount = 100 * i,
                InterestRate = 5.0m,
                LoanStatus = LoanStatus.Pending,
                ApplicationDate = DateTime.UtcNow.AddDays(-i)
            });
        }
        _context.SaveChanges();
    }


    [Fact]
    public async Task GetAllAsync_Returns_Correct_Page()
    {
        // Arrange
        Setup();

        int pageSize = 10;
        int pageNumber = 2;

        // Act
        var result = await _repository.GetAllApplicationsAsync(pageSize, pageNumber, CancellationToken.None);

        // Assert
        pageSize.Should().Be(result.loans.Count());
        result.loans.First().Id.Should().Be(11);
    }

    [Fact]
    public async Task CountAsync_Returns_Total_Count()
    {
        // Arrange
        Setup();

        int pageSize = 10;
        int pageNumber = 2;

        // Act
        var result = await _repository.GetAllApplicationsAsync(pageSize, pageNumber, CancellationToken.None);


        // Assert
        result.count.Should().Be(25);
    }


    [Fact]
    public async Task DeleteAsync_Removes_Loan()
    {
        // Arrange
        Setup();

        int idToDelete = 5;

        // Act
        await _repository.DeleteApplicationAsync(idToDelete, CancellationToken.None);
        var exists = await _context.LoanApplications.FindAsync(idToDelete);

        // Assert
        exists.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_Inserts_Loan()
    {
        // Arrange
        Setup();

        var entity = new LoanApplication
        {
            ApplicantName = "NewUser",
            LoanAmount = 2000,
            InterestRate = 4.5m,
            LoanStatus = LoanStatus.Approved,
            ApplicationDate = DateTime.UtcNow
        };

        // Act
        await _repository.AddApplicationAsync(entity, CancellationToken.None);

        var added = await _context.LoanApplications.SingleOrDefaultAsync(l => l.ApplicantName == "NewUser");

        // Assert
        added.Should().NotBeNull();
    }

    //update
    [Fact]
    public async Task UpdateAsync_Updates_Loan()
    {
        // Arrange
        Setup();
        int idToUpdate = 10;

        var existingLoan = await _context.LoanApplications.FindAsync(idToUpdate);
        existingLoan.ApplicantName = "UpdatedUser";
        existingLoan.LoanAmount = 5000;
        existingLoan.LoanStatus = LoanStatus.Pending;

        // Act
        await _repository.UpdateApplicationAsync(existingLoan, CancellationToken.None);

        // Assert
        existingLoan.ApplicantName.Should().Be("UpdatedUser");
        existingLoan.LoanAmount.Should().Be(5000);
        existingLoan.LoanStatus.Should().Be(LoanStatus.Pending);
    }
}
