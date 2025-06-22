using LoanApplicationSystem.Domain.Entities;
using LoanApplicationSystem.Domain.Enums;

namespace LoanApplicationSystem.Services.Dto;
public class LoanApplicationDto
{
    public int Id { get; set; }
    public string ApplicantName { get; set; } = default!;
    public decimal LoanAmount { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = default!;
    public decimal InterestRate { get; set; }
    public int LoanTerm { get; set; }

    public static LoanApplicationDto FromEntity(LoanApplication application)
    {
        if (application == null) throw new ArgumentNullException(nameof(application), "Application cannot be null.");
        return new LoanApplicationDto
        {
            Id = application.Id,
            ApplicantName = application.ApplicantName,
            LoanAmount = application.LoanAmount,
            ApplicationDate = application.ApplicationDate,
            Status = application.LoanStatus.ToString(),
            InterestRate = application.InterestRate,
            LoanTerm = application.LoanTerm
        };
    }

    public LoanApplication ToEntity()
    {
        return new LoanApplication
        {
            Id = this.Id,
            ApplicantName = this.ApplicantName,
            LoanAmount = this.LoanAmount,
            ApplicationDate = this.ApplicationDate,
            LoanStatus = Enum.TryParse<LoanStatus>(this.Status, out var status) ? status : LoanStatus.Pending,
            InterestRate = this.InterestRate,
            LoanTerm = this.LoanTerm
        };
    }
}
