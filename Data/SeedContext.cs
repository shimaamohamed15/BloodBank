using BloodBank.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Data
{
    public class SeedContext
    {

        public static async Task SeedData(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await CreateRoles(roleManager, userManager);
          
        }
        private static async Task CreateRoles(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
           


            // creating Creating User role     
             bool roleExist = await roleManager.RoleExistsAsync("User");
            if (!roleExist)
            {
                var role = new ApplicationRole();
                role.Name = "User";
                role.Description = "this Role for any user";
                await roleManager.CreateAsync(role);
            }


           

            //  create Admin and Admin role 
             roleExist = await roleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {

                var role = new ApplicationRole();
                role.Name = "Admin";
                role.Description = "this is Role for only Admins";
                await roleManager.CreateAsync(role);

            }

            var user = userManager.FindByEmailAsync("shimaamohamedpro@gmail.com").Result;
            userManager.AddToRoleAsync(user, "Admin").Wait();








        }
    }
}
