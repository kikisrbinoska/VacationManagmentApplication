using System;
using System.Collections.Generic;
using System.Linq;
using VacationManagmentApplication.Models;

namespace ApplicationTests
{
    public static class DataInitializer
    {
        public static List<BonusDays> GetBonusDaysData()
        {
            return new List<BonusDays>
            {
                new BonusDays
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(-5),
                    
                },
                new BonusDays
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddDays(-20),
                    EndDate = DateTime.Now.AddDays(-15),
                  
                }
            };
        }

        public static List<Employee> GetEmployeesData()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "John Doe",
                   
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Smith",
                  
                }
            };
        }

        public static List<MedicalCare> GetMedicalCaresData()
        {
            return new List<MedicalCare>
            {
                new MedicalCare
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = Guid.NewGuid(),
                    HRId = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now,
                    Reason = "Medical care for illness"
                }
            };
        }
    }
}
