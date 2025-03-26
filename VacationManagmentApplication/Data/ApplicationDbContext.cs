using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManagmentApplication.Models;

namespace VacationManagmentApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<HR> HR { get; set; }
        public virtual DbSet<BonusDays> BonusDays { get; set; }
        public virtual DbSet<MedicalCare> MedicalCares { get; set; }
        public virtual DbSet<YearlyVacation> YearlyVacations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
