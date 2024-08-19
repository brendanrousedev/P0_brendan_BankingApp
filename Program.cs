using P0_brendan_BankingApp.POCO;
// USERNAME: owner
// PASSWORD: password123

using System;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        // Define your connection string here
        // string connectionString = "YourConnectionStringHere";

        // Create a new Admin instance
        String password = "password123";
        byte[] salt = GenerateSalt();
        var newAdmin = new Admin
        {
            AdminUsername = "owner",
            PasswordHash = HashPassword("password123", salt), // You need to implement password hashing
            Salt = salt // You need to implement salt generation
        };

        // Insert the new Admin into the database
        using (var context = new P0BrendanBankingDbContext())
        {
            context.Admins.Add(newAdmin);
            context.SaveChanges();
        }

        Console.WriteLine("New admin has been added to the database.");
    }

    public static string HashPassword(string password, byte[] salt)
    {
        // implements Hash-based message authentication code using SHA-512 hashing algorithm
        using (var hmac = new HMACSHA512(salt))
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = hmac.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static byte[] GenerateSalt()
    {
        // using - resources are disposed of after being used. RNGCryptoServiceProvider will be dropped after it is used
        // implicity declare the type of variable
        using (var rng = new RNGCryptoServiceProvider())
        {
            // set the variable salt to a byte-array
            var salt = new byte[16]; // 128-bit salt
            // fill the byte array 
            rng.GetBytes(salt);
            return salt;
        }
    }
}
