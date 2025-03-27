using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VacationManagmentApplication.Controllers;
using VacationManagmentApplication.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VacationManagmentApplication.Data;

namespace ApplicationTests
{
    [TestClass]
    public class BonusDaysControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly BonusDaysController _controller;

        public BonusDaysControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new BonusDaysController(_mockContext.Object);
        }

        [TestMethod]
        public async Task AddNewBonus_InvalidStartDate_ShouldReturnViewWithError()
        {
            var bonusDays = new BonusDays
            {
                StartDate = DateTime.Now.AddDays(1),  
                EndDate = DateTime.Now
            };

           
            var result = await _controller.AddNewBonus(bonusDays);

          
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState[""].Errors[0].ErrorMessage.Contains("The start date cannot be after the end date."));
        }

        [TestMethod]
        public async Task AddNewBonus_ValidBonusDays_ShouldRedirectToViewBonusRequests()
        {
           
            var bonusDays = DataInitializer.GetBonusDaysData().First(); 

            
            var mockBonusDaysList = DataInitializer.GetBonusDaysData();
            _mockContext.Setup(m => m.BonusDays).Returns((Microsoft.EntityFrameworkCore.DbSet<BonusDays>)mockBonusDaysList.AsQueryable());

            
            var result = await _controller.AddNewBonus(bonusDays);

           
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("ViewBonusRequests", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task AddNewBonus_EmployeeData_ShouldReturnValidEmployee()
        {
            
            var employee = DataInitializer.GetEmployeesData().First();

           
            var mockEmployeeList = DataInitializer.GetEmployeesData();
            _mockContext.Setup(m => m.Employees).Returns((Microsoft.EntityFrameworkCore.DbSet<Employee>)mockEmployeeList.AsQueryable());

           
            var result = await _controller.AddNewBonus(new BonusDays { EmployeeId = employee.Id });

            
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public async Task AddNewBonus_MedicalCareData_ShouldReturnValidMedicalCare()
        {
           
            var medicalCare = DataInitializer.GetMedicalCaresData().First();

            
            var mockMedicalCareList = DataInitializer.GetMedicalCaresData();
            _mockContext.Setup(m => m.MedicalCares).Returns((Microsoft.EntityFrameworkCore.DbSet<MedicalCare>)mockMedicalCareList.AsQueryable());

           
            var result = await _controller.AddNewBonus(new BonusDays { Id = medicalCare.Id });

            
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}
