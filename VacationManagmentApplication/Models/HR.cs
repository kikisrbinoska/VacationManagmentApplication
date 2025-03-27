using System.ComponentModel.DataAnnotations;

namespace VacationManagmentApplication.Models
{
    public class HR
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public List<YearlyVacation> Vacations { get; set; } = new List<YearlyVacation>();
        public List<MedicalCare> MedicalCares { get; set; } = new List<MedicalCare>();
        public List<BonusDays> BonusDays { get; set; } = new List<BonusDays>();

        private const int TOTAL_VACATION_DAYS = 21;
        private const int FIRST_PART_DAYS = 10;
        private const int SECOND_PART_DAYS = 11;


        public int GetUsedFirstPartVacation()
        {
            return Vacations
                .Where(v => v.StartDate.Year == DateTime.Now.Year && v.StartDate.Month <= 12)
                .Sum(v => (v.EndDate - v.StartDate).Days + 1);
        }

        public int GetUsedSecondPartVacation()
        {
            return Vacations
                .Where(v => v.StartDate.Year == DateTime.Now.Year && v.StartDate.Month <= 6)
                .Sum(v => (v.EndDate - v.StartDate).Days + 1);
        }


        public int GetUsedBonusDays()
        {
            return BonusDays
                .Where(b => b.StartDate.Year == DateTime.Now.Year)
                .Sum(b => (b.EndDate - b.StartDate).Days + 1);
        }

        public int GetRemainingFirstPartVacation()
        {
            return Math.Max(FIRST_PART_DAYS - GetUsedFirstPartVacation(), 0);
        }

        public int GetRemainingSecondPartVacation()
        {
            return Math.Max(SECOND_PART_DAYS - GetUsedSecondPartVacation(), 0);
        }

        public int GetRemainingBonusDays()
        {

            int usedBonusDays = BonusDays
                .Where(b => b.StartDate.Year == DateTime.Now.Year)
                .Sum(b => (b.EndDate - b.StartDate).Days + 1);

            int totalBonusDays = 10;
            return totalBonusDays - usedBonusDays;
        }


        public int GetTotalRemainingVacationDays()
        {
            return GetRemainingFirstPartVacation() + GetRemainingSecondPartVacation() + GetRemainingBonusDays();
        }
    }
}
