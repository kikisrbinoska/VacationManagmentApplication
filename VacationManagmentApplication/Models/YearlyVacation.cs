using System.ComponentModel.DataAnnotations;

namespace VacationManagmentApplication.Models
{
    public class YearlyVacation
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Employee? Employee { get; set; }
        public Guid? EmployeeId { get; set; }
        public HR? HR { get; set; }
        public Guid? HRId { get; set; }
    }
}
