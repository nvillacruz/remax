using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remax.Web.Server
{
    /// <summary>
    /// Manages the standard web server pages
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]

    public class HomeController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext mContext;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        /// <summary>
        /// 
        /// </summary>
        protected RoleManager<IdentityRole> mRoleManager;
        /// <summary>
        /// 
        /// </summary>
        protected string superAdminId;

        /// <summary>
        /// 
        /// </summary>
        protected string mEmployeeId;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
            mRoleManager = roleManager;
        }
                
        #endregion


        /// <summary>
        /// Basic welcome page
        /// </summary>
        /// <returns></returns>
        ///
        public async Task<IActionResult> Index()
        {
            // Make sure we have the database

            //mContext.Database.EnsureDeleted();
            mContext.Database.EnsureCreated();

            await AddAspNetRolesAsync();
            await AddSuperAdminUserAsync();
            return View();
        }

        private async Task AddAspNetRolesAsync()
        {
            var li = new List<IdentityRole> {
                new IdentityRole{Name = AspNetRolesDefaults.SuperAdmin, NormalizedName = AspNetRolesDefaults.SuperAdmin.NormalizedUpper() },
                new IdentityRole{Name = AspNetRolesDefaults.Admin, NormalizedName = AspNetRolesDefaults.Admin.NormalizedUpper() },
                new IdentityRole{Name = AspNetRolesDefaults.Manager, NormalizedName = AspNetRolesDefaults.Manager.NormalizedUpper() },
                new IdentityRole{Name = AspNetRolesDefaults.Employee, NormalizedName = AspNetRolesDefaults.Employee.NormalizedUpper() },
            };

            await mContext.Roles.AddRangeAsync(li);
            await mContext.SaveChangesAsync();
        }
        private async Task AddSuperAdminUserAsync()
        {
            var employeeId = StringHelper.GenerateNewGuid();
            mEmployeeId = employeeId;
            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "nelson.villacruz@ygroup.ph",
                Id = StringHelper.GenerateNewGuid(),
                EmailConfirmed = true,
                PhoneNumber = "09509595882",
                Employee = new Employee
                {
                    Id = employeeId,
                    FirstName = "Nelson",
                    LastName = "Villacruz",
                    Email = "nelson.villacruz@ygroup.ph",
                    PhoneNumber = "09509595882",
                    Affiliation = "Y Hotel",
                    Gender = "male",
                    BirthDate = DateTimeOffset.UtcNow,
                },

            };

            var success = await mUserManager.CreateAsync(user, "Password1234");
            if (success.Succeeded)
                await mUserManager.AddToRolesAsync(user, new[] { AspNetRolesDefaults.SuperAdmin, AspNetRolesDefaults.Employee });
        }


    }
}
