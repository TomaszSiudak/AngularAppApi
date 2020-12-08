using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers
{
    public class StringHelper
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string numbers = "123456789";
        static Random rand = new Random();

        public static string GenerateRandomString(int length) => new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());

        public static string GenerateRandomNumberString(int length) => new string(Enumerable.Repeat(numbers, length).Select(s => s[rand.Next(s.Length)]).ToArray());
    }
}
