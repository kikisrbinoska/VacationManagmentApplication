using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VacationManagmentApplication.Controllers;
using VacationManagmentApplication.Data;
using VacationManagmentApplication.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTests
{
    [TestClass]
    public class MedicalCaresControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly MedicalCaresController _controller;

        public MedicalCaresControllerTests()
        {
           
            _mockContext = new Mock<ApplicationDbContext>();

           
            _controller = new MedicalCaresController(_mockContext.Object);
        }

        // Test for Index Action
        [TestMethod]
        public async Task Index_ShouldReturnViewWithMedicalCares()
        {
           
            var mockDbSet = new Mock<DbSet<MedicalCare>>();
            _mockContext.Setup(m => m.MedicalCares).Returns(mockDbSet.Object);

           
            var result = await _controller.Index();

          
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as IQueryable<MedicalCare>;
            Assert.IsNotNull(model);
        }

        // Test for Details Action
        [TestMethod]
        public async Task Details_WithValidId_ShouldReturnView()
        {
            
            var medicalCareId = Guid.NewGuid();
            var mockDbSet = new Mock<DbSet<MedicalCare>>();
            _mockContext.Setup(m => m.MedicalCares.FindAsync(medicalCareId))
                .ReturnsAsync(new MedicalCare { Id = medicalCareId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) });

            
            var result = await _controller.Details(medicalCareId);

            
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as MedicalCare;
            Assert.IsNotNull(model);
        }

        // Test for Create Action (GET)
        [TestMethod]
        public void Create_ShouldReturnView()
        {
            
            var result = _controller.Create();

           
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

       

        // Test for Edit Action (GET)
        [TestMethod]
        public async Task Edit_WithValidId_ShouldReturnView()
        {
           
            var medicalCareId = Guid.NewGuid();
            var mockDbSet = new Mock<DbSet<MedicalCare>>();
            _mockContext.Setup(m => m.MedicalCares.FindAsync(medicalCareId))
                .ReturnsAsync(new MedicalCare { Id = medicalCareId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) });

            
            var result = await _controller.Edit(medicalCareId);

            
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as MedicalCare;
            Assert.IsNotNull(model);
        }

        // Test for Delete Action (GET)
        [TestMethod]
        public async Task Delete_WithValidId_ShouldReturnView()
        {
            
            var medicalCareId = Guid.NewGuid();
            var mockDbSet = new Mock<DbSet<MedicalCare>>();
            _mockContext.Setup(m => m.MedicalCares.FindAsync(medicalCareId))
                .ReturnsAsync(new MedicalCare { Id = medicalCareId });

            
            var result = await _controller.Delete(medicalCareId);

           
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as MedicalCare;
            Assert.IsNotNull(model);
        }

       
       
    }
}
