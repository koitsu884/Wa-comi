using System;
using System.Linq;

namespace Wacomi.API.Helper
{
    public class Common
    {
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}