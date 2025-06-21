using LoanApplicationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationSystem.Data.Context;
public class LoanApplicationContext(DbContextOptions<LoanApplicationContext> options) : DbContext(options)
{
    public DbSet<LoanApplication> LoanApplications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
