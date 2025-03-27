using Microsoft.AspNetCore.Identity;

namespace VacationManagmentApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public Employee? Employee { get; set; }
        public HR? HR { get; set; }
    }
}
