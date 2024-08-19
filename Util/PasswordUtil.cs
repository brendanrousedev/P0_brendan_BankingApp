using System;
using System.Linq;
using System.Security.Cryptography;

public static class PasswordUtils
{
    // static - the method GenerateSalt() belongs to the class itself so we don't need to instantiate an object to use the method
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

    public static bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
    {
        var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
        return hashOfEnteredPassword == storedHash;
    }
}
     