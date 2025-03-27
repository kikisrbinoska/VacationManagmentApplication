using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VacationManagmentApplication.Data;
using VacationManagmentApplication.Models;

namespace VacationManagmentApplication.Areas.Identity.Pages.Account
{
    public class RegisterModel1 : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel1(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string PhoneNumber { get; set; }
            public string Role { get; set; }
            public int Age { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            ViewData["Roles"] = new SelectList(new[]
            {
        new SelectListItem { Text = "HR", Value = "HR" },
        new SelectListItem { Text = "Employee", Value = "Employee" }
    }, "Value", "Text");
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var fullName = $"{Input.Name} {Input.Surname}";
                    var userId = await _userManager.GetUserIdAsync(user);

                    if (Input.Role == "HR")
                    {
                        await _userManager.AddToRoleAsync(user, "HR");

                        var hr = new HR
                        {
                            Name = Input.Name,
                            Surname = Input.Surname,
                            PhoneNumber = Input.PhoneNumber,
                            Age = Input.Age,
                            Email = Input.Email
                        };

                        _context.HR.Add(hr);
                        await _context.SaveChangesAsync();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Details", "HRs", new { id = hr.Id });
                    }
                    else if (Input.Role == "Employee")
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");

                        var employee = new Employee
                        {
                            Name = Input.Name,
                            Surname = Input.Surname,
                            PhoneNumber = Input.PhoneNumber,
                            Age = Input.Age,
                            Email = Input.Email
                        };

                        _context.Employees.Add(employee);
                        await _context.SaveChangesAsync();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Details", "Employees", new { id = employee.Id });
                    }

                    // Skip the email confirmation process entirely
                    // Sign in the user immediately
                    

                    return LocalRedirect(returnUrl); // Redirect user to the returnUrl (home page or desired location)
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }



        private ApplicationUser CreateUser()
        {
            return new ApplicationUser();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
