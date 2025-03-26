using System.ComponentModel.DataAnnotations;

namespace VacationManagmentApplication.Models
{
    public class HR
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public List<YearlyVacation> Vacations { get; set; }
        public List<MedicalCare> MedicalCares { get; set; }
        public List<BonusDays> BonusDays { get; set; }
    }
}
