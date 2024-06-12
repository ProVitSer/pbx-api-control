using System.Security.Cryptography;

namespace PbxApiControl.Services.Utils;
public class UtilService
{
    public static string GeneratePassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+";

        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[bytes[i] % validChars.Length];
            }

            return new string(chars);
        };
    }
}