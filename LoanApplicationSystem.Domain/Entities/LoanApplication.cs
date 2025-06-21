using LoanApplicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LoanApplicationSystem.Domain.Entities;
public class LoanApplication
{
    [Key]
    public int Id { get; set; }
    public string ApplicantName { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanTerm { get; set; }
    public decimal InterestRate { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public DateTime ApplicationDate { get; set; }

    public LoanApplication()
    {

    }

    public LoanApplication(string applicantName, decimal loanAmount, int loanTerm, decimal interestRate)
    {
        ApplicantName = applicantName;
        LoanAmount = loanAmount;
        LoanTerm = loanTerm;
        InterestRate = interestRate;
        LoanStatus = LoanStatus.Pending;
        ApplicationDate = DateTime.UtcNow;
    }

    public void UpdateApplication(string applicantName, decimal loanAmount, int loanTerm, decimal interestRate)
    {
        if (LoanStatus != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending applications can be updated.");

        ApplicantName = applicantName;
        LoanAmount = loanAmount;
        LoanTerm = loanTerm;
        InterestRate = interestRate;
    }

    public void Approve()
    {
        if (LoanStatus != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending applications can be approved.");
        LoanStatus = LoanStatus.Approved;
    }

    public void Reject()
    {
        if (LoanStatus != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending applications can be rejected.");
        LoanStatus = LoanStatus.Rejected;
    }

    public override string ToString()
    {
        return $"Loan Application: {ApplicantName}, Amount: {LoanAmount:C}, Term: {LoanTerm} months, Status: {LoanStatus}, Date: {ApplicationDate}";
    }
}
