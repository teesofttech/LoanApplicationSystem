using LoanApplicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LoanApplicationSystem.Domain.Entities;
public class LoanApplication
{
    [Key]
    public int Id { get; set; }
    public string ApplicationName { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanTerm { get; set; }
    public decimal InterestRate { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public DateTime ApplicationDate { get; set; }
}
