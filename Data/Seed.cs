using EmployeeCRUD.API.Models;
using Newtonsoft.Json;

namespace EmployeeCRUD.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passworHash, passwordSalt;
                    CreatePassworHash("password", out passworHash, out passwordSalt);

                    user.PasswordHash = passworHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }

                // called on startup when no user accessing our app so no worry for async execution
                context.SaveChanges();
            }
        }

        private static void CreatePassworHash(string password, out byte[] passworHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passworHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}