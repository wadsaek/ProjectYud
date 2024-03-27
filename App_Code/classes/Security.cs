using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for Security
/// </summary>
public class Security
{
    /// <summary>
    /// salts
    /// </summary>
    /// <param name="length"></param>
    /// <returns>a salt,however can also be used for other things
    /// that need random strings so it's up to your imagination</returns>
    public static string RandomString(int length)
    {
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(Consts.VALID_SYMBOLS[(int)(num % (uint)Consts.VALID_SYMBOLS.Length)]);
            }
        }

        return res.ToString();
    }

    /// <summary>
    /// hashes using a salt
    /// </summary>
    /// <param name="password"> the inputed password</param>
    /// <param name="salt"> the salt</param>
    /// <returns>the password to be stored in the database</returns>
    public static string EncryptPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = string.Format("{0}{1}", salt, password);
            byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
            return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
        }
    }


}