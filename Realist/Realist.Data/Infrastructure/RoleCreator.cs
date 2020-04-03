using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
  public  class RoleCreator
    {


    

     
        public  async Task Create(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager,
            IConfiguration _configuration)
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new User
                {
                    UserName = _configuration.GetSection("SuperAdminUserName").Value,
                    Email = _configuration.GetSection("SuperAdminEmail").Value
                };

                string userPWD = _configuration.GetSection("SuperAdminPassowrd").Value ;

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("User");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await _roleManager.CreateAsync(role);
            }

        }
    }
}
