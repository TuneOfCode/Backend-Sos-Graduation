using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.ValueObjects;
using Sos.Infrastructure.Cryptography;
using Sos.Persistence.Data;

namespace Sos.Api.Configurations
{
    /// <summary>
    /// Represents the data seeder of the application database context MSSQL.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Runs the data seeder.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns></returns>
        public static async Task Run(AppDbContext dbContext)
        {
            Console.WriteLine("***** Data Seeder Is Running *****");

            if (!dbContext.Set<User>().Any())
            {
                await SeedUsers(dbContext);
            }

            Console.WriteLine("***** Data Seeder Is Done *****");
        }

        public static string RandomEmail(Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static async Task SeedUsers(AppDbContext dbContext, int count = 5)
        {
            var newUsers = new List<User>();

            Random random = new Random();

            string[] firstNames = ["Chương", "Đạt", "Hào", "Hùng", "Nhật", "Ni", "Lộc", "Tuyến", "Trường", "Tú", "Yến"];

            string[] lastNames = ["Trần", "Nguyễn", "Lê", "Hoàng", "Đinh", "Cao"];

            string defaultVerifyCode = "123456";

            DateTime defaultVerifyCodeExpired = DateTime.Now;

            DateTime defaultVerifiedAt = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length - 1)];

                var lastName = lastNames[random.Next(lastNames.Length - 1)];

                var emailRandom = $"{RandomEmail(random, 6)}@gmail.com";
                Result<Email> email = Email.Create(emailRandom);

                Result<Phone> defaultContactPhone = Phone.Create("0123456789");

                IPasswordHasher passwordHasher = new PasswordHasher();
                Result<Password> password = Password.Create("123456");
                string defaultPasswordHashed = passwordHasher.HashPassword(password.Value);

                Result<Avatar> defaultAvatar = Avatar.Create("/images/defaultAvatar.png", 0);

                double latitude = random.NextDouble() * (16.80000 - 16.00000) + 16.00000;
                double longitude = random.NextDouble() * (108.20000 - 107.80000) + 107.80000;
                Result<Location> location = Location.Create(longitude, latitude);

                var newUser = User.Create(firstName, lastName, email.Value, defaultContactPhone.Value, defaultPasswordHashed);
                newUser.Avatar = defaultAvatar.Value;
                newUser.Location = location.Value;
                newUser.VerifyCode = defaultVerifyCode;
                newUser.VerifyCodeExpired = defaultVerifyCodeExpired;
                newUser.VerifiedAt = defaultVerifiedAt;

                newUsers.Add(newUser);
            }

            dbContext.Set<User>().AddRange(newUsers);

            await dbContext.SaveChangesAsync();
        }
    }
}