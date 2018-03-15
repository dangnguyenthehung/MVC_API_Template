using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApiCall;
using Constants;
using Model.Interfaces;
using Constants.Security;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var demo = new DemoHelper("123456");
            demo.Insert("https://www.google.com.vn/", "demo");

            var emp = new Employee(){Id = 1, Permissions = new List<int>(){10,2}};
            MenuLayoutVisibilityConstant menuLayoutVisibilityConstant = new MenuLayoutVisibilityConstant();
            menuLayoutVisibilityConstant.CheckMenuVisibilityState(emp);

            //Console.WriteLine(menuLayoutVisibilityConstant.Demo.Any.ToString());

            //Console.WriteLine(Encryptor.EncryptMD5("abcdef"));
            //Console.WriteLine(Encryptor.EncryptSHA1("abcdef"));
            //Console.WriteLine(Encryptor.EncryptSHA256("abcdef"));
            //Console.WriteLine(Encryptor.EncryptSHA384("abcdef"));
            //Console.WriteLine(Encryptor.EncryptSHA512("abcdef"));

            //var enc = new Encryptor.AESencrypt();
            //Console.WriteLine(enc.EncryptString_Aes("abcdef"));
            Console.WriteLine(GenerateKey());
            Console.ReadLine();


        }

        class Employee : IAccount
        {
            public int Id { get; set; }
            public List<int> Permissions { get; set; }
        }

        private static string GenerateKey()
        {
            AesManaged aesEncryption = new AesManaged();
            aesEncryption.KeySize = 192;
            aesEncryption.GenerateIV();
            string ivStr = Convert.ToBase64String(aesEncryption.IV);

            aesEncryption.GenerateKey();
            string keyStr = Convert.ToBase64String(aesEncryption.Key);
            //Console.WriteLine(ivStr);
            Console.WriteLine(keyStr);

            var hmac = new HMACSHA512();
            var key = Convert.ToBase64String(hmac.Key);

            Console.WriteLine("---------");
            Console.WriteLine(key);

            return "ok";
        }

    }
}
