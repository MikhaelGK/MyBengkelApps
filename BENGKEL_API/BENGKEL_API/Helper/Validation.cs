using System.Reflection.Metadata.Ecma335;

namespace BENGKEL_API.Helper
{
    public static class Validation
    {
        public static bool ValidationString(List<string> strgs)
        {
            foreach (var s in strgs)
            {
                if (String.IsNullOrEmpty(s)) return false;
            }
            return true;
        }

        public static bool ValidationInPassword(string strg)
        {
            var hasUpper = false;
            var hasLower = false;
            var hasDigit = false;
            foreach (var c in strg)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            var meetLength = false;
            if (strg.Length >= 8 && strg.Length <= 12)
            {
                meetLength = true;
            }

            return hasUpper && hasLower && hasDigit && meetLength;
        }

        public static bool ValidationInEmail(string strg)
        {
            if (strg.Contains('@') && strg.Contains('.')) return true;
            return false;
        }
    }
}
