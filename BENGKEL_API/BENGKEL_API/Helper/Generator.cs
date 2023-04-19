using Microsoft.IdentityModel.Abstractions;
using System.Diagnostics;
using System.Text;

namespace BENGKEL_API.Helper
{
    enum Master { Customer, Transaction }
    public static class Generator
    {
        public static string GenerateID(string master, int length)
        {
            var sb = new StringBuilder();

            if (master == Master.Customer.ToString()) sb.Insert(0, "C");

            if (master == Master.Transaction.ToString()) sb.AppendFormat($"T{DateTime.Now.ToString("yyyy")}");

            var character = "1234567890";
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(character[random.Next(character.Length)]);
            }
            return sb.ToString();
        }
    }
}
