using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BENGKEL.Helper
{
    public static class Validation
    {
        public static bool StringIsEmpty(List<string> strgs)
        {
            foreach (var i in strgs)
            {
                if (String.IsNullOrEmpty(i))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool StringIsNumber(string strg)
        {
            var allNumber = true;
            var meetLength = false;
            foreach (var c in strg)
            {
                if (char.IsLetter(c)) allNumber = false;
            }
            if (strg.Length > 10 && strg.Length < 12) meetLength = true; 

            return allNumber && meetLength;
        }

        public static bool StringIsPassword(string strg)
        {
            var hasUpper = false;
            var hasLower = false;
            var hasNumber = false;
            var meetLength = false;

            foreach (var c in strg)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasNumber = true;
            }

            if (strg.Length > 8 && strg.Length < 12) meetLength = true;

            return hasUpper && hasLower && hasNumber && meetLength;
        }

        public static bool StringIsEmail(string strg)
        {
            if (strg.Contains('@') && strg.Contains('.')) return true;
            return false;
        }


    }
}
