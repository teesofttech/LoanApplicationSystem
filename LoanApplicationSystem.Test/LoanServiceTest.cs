using FluentAssertions;
using LoanApplicationSystem.Data.Interfaces;
using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Services.Dto;
using LoanApplicationSystem.Services.Services;
using Moq;

namespace LoanApplicationSystem.Test;

public class LoanServiceTest
{
    private readonly Mock<ILoanApplicationRepository> _loanApplicationRepositoryMock;
    private readonly LoanService _loanService;
    public LoanServiceTest()
    {
        _loanApplicationRepositoryMock = new Mock<ILoanApplicationRepository>();
        _loanService = new LoanService(_loanApplicationRepositoryMock.Object);
    }

    [Fact]
    public async Task GetLoanByIdAsync_ReturnCorrectLoan()
    {
        int loanId = 1;
        var expectedLoan = new LoanApplication()
        {
            ApplicantName = "Standard Loan",
            Id = loanId,
            ApplicationDate = DateTime.Now,
            InterestRate = 4,
            LoanAmount = 500,
            LoanStatus = Domain.Enums.LoanStatus.Pending,
            LoanTerm = 4
        };
        CancellationToken ct = CancellationToken.None;
        _loanApplicationRepositoryMock.Setup(x => x.GetApplicationByIdAsync(loanId, ct)).ReturnsAsync(expectedLoan);

        var result = await _loanService.GetApplicationByIdAsync(loanId, ct);

        result.Should().NotBeNull();
        result.Status.Should().Be(nameof(Domain.Enums.LoanStatus.Pending));
        result.ApplicantName.Should().Be(expectedLoan.ApplicantName);
    }

    [Fact]
    public async Task AddLoanAsync_ReturnCorrectLoan()
    {
        int loanId = 1;
        var expectedLoan = new LoanApplication()
        {
            ApplicantName = "Standard Loan",
            Id = loanId,
            ApplicationDate = DateTime.Now,
            InterestRate = 4,
            LoanAmount = 500,
            LoanStatus = Domain.Enums.LoanStatus.Pending,
            LoanTerm = 4
        };
        CancellationToken ct = CancellationToken.None;
        _loanApplicationRepositoryMock.Setup(x => x.AddApplicationAsync(expectedLoan, ct)).Returns(Task.CompletedTask);

        var dto = LoanApplicationDto.FromEntity(expectedLoan);
        await _loanService.AddApplicationAsync(dto, ct);

        _loanApplicationRepositoryMock.Verify(x => x.AddApplicationAsync(It.IsAny<LoanApplication>(), ct), Times.Once);
    }

    [Fact]
    public async Task DeleteLoanAsyncById_ReturnSuccessful()
    {
        int loanId = 1;
        var expectedLoan = new LoanApplication()
        {
            ApplicantName = "Standard Loan",
            Id = loanId,
            ApplicationDate = DateTime.Now,
            InterestRate = 4,
            LoanAmount = 500,
            LoanStatus = Domain.Enums.LoanStatus.Pending,
            LoanTerm = 4
        };
        CancellationToken ct = CancellationToken.None;
        _loanApplicationRepositoryMock.Setup(x => x.DeleteApplicationAsync(loanId, ct)).Returns(Task.CompletedTask);

        await _loanService.DeleteApplicationAsync(loanId, ct);
        _loanApplicationRepositoryMock.Verify(x => x.DeleteApplicationAsync(It.IsAny<int>(), ct), Times.Once);
    }
}
