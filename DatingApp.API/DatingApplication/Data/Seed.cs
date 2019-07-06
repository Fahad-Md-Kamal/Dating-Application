using System.Collections.Generic;
using System.Linq;
using DatingApplication.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApplication.Data
{
    public class Seed
    {
        // Since UserManager class going to be used we don't need to customize database operation. The class will do all the works for us.

        // private readonly DataContext _context; 
        private readonly RoleManager<Role> _roleManger;
        private readonly UserManager<User> _userManager;
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManger)
        {
            _roleManger = roleManger;
            _userManager = userManager;
            // _context = context;
        }

        public void SeedUsers()
        {

            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"},
                };

                foreach (var role in roles)
                {
                    _roleManger.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.Photos.SingleOrDefault().IsApproved = true; // While seeding data all the photos will be approved
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Member").Wait();

                    #region customPasswrod Hasing
                    // if we are going to create password has and salt by ourselves then we will need to make all the following calls by ourselves.
                    // but we don't need to do that now. As userManger will do the hashing and salting as well as saving the data into the database through out the process.

                    // byte[] passwordHash, passwordSalt;
                    // CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    // user.PasswordHash = passwordHash;
                    // user.PasswordSalt = passwordSalt;
                    // user.UserName = user.UserName.ToLower();
                    // _context.Users.Add(user);
                    #endregion
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
                }


                #region save data
                // _context.SaveChanges();
                #endregion
            }


        }

        #region generate pasword hasing and salt by decoding passowrd
        // private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        // {
        //     using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //     {
        //         passwordSalt = hmac.Key;
        //         passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //     }

        // }
        #endregion
    }
}