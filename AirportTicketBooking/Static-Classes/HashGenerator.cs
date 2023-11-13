using System.Security.Cryptography;

namespace AirportTicketBooking;

public static class HashGenerator
{
    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static int HashPassword(string password, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        return BitConverter.ToInt32(pbkdf2.GetBytes(4), 0);
    }
}