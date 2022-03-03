using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aes256CbcEncrypterApp
{
    internal class Program
    {
        private static readonly string key = "728378c43ebb41df9495bc090ecf54af";
        private static readonly string initVector = key[..16];

        private static Aes CreateAes()
        {
            var aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(initVector);
            return aes;
        }

        public static string Encrypt(string text)
        {
            using var aes = CreateAes();
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
            using var aes = CreateAes();
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using MemoryStream ms = new(Convert.FromBase64String(text));
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