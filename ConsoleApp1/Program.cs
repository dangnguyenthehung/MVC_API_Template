using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiCall;
using Constants;
using Constants.Interface;
using Constants.Security;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var demo = new DemoHelper("123456");
            demo.Insert("https://www.google.com.vn/", "demo");

            var emp = new Employee(){IdEmployee = 1, Permissions = new List<int>(){10,2}};
            MenuLayoutVisibilityConstant menuLayoutVisibilityConstant = new MenuLayoutVisibilityConstant();
            menuLayoutVisibilityConstant.CheckMenuVisibilityState(emp);

            Console.WriteLine(menuLayoutVisibilityConstant.Demo.Any.ToString());

            Console.WriteLine(Encryptor.EncryptMD5("abcdef"));
            Console.WriteLine(Encryptor.EncryptSHA1("abcdef"));
            Console.WriteLine(Encryptor.EncryptSHA256("abcdef"));
            Console.WriteLine(Encryptor.EncryptSHA384("abcdef"));
            Console.WriteLine(Encryptor.EncryptSHA512("abcdef"));

            var enc = new Encryptor.AESencrypt();
            Console.WriteLine(enc.EncryptString_Aes("abcdef"));
            Console.ReadLine();


        }

        class Employee : IEmployee
        {
            public int IdEmployee { get; set; }
            public List<int> Permissions { get; set; }
        }
    }
}
