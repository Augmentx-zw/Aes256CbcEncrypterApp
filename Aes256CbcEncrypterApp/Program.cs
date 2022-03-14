using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aes256CbcEncrypterApp
{
    internal class Program
    {
        //replace the _key value with your key
        private static readonly string _key = "1123y13y132y13y132y132y13y123y1u";
        private static readonly string _initVector = _key[..16];

        public static string Encrypt(string text)
        {
            var aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = Encoding.UTF8.GetBytes(_initVector);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs))
            {
                sw.Write(text);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string text)
        {
            var aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = Encoding.UTF8.GetBytes(_initVector);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using MemoryStream ms = new(Convert.FromBase64String(text));
            aes.Padding = PaddingMode.Zeros;
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new(cs);
            return reader.ReadToEnd();
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter String");
            Console.WriteLine();
            string originalString = Console.ReadLine();
            Console.WriteLine("---------------------------------------------");
            var encryptedString = Encrypt(originalString);
            Console.WriteLine();
            Console.WriteLine("Encrypeted Text");
            Console.WriteLine(encryptedString);
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Enter Encrypeted String");
            Console.WriteLine();
            string newecryptedString = Console.ReadLine();
            Console.WriteLine();
            var decryptedString = Decrypt(newecryptedString);
            Console.WriteLine();
            Console.WriteLine("Decrypted");
            Console.WriteLine();
            Console.WriteLine(decryptedString);
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
        }
    }
}