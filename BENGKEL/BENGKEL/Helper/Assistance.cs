using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BENGKEL.Helper
{
    enum Master { Employee, Vehicle, Transaction, Customer}

    public static class Assistance
    {
        public static string GenerateID(int length, string master)
        {
            StringBuilder id = new StringBuilder();

            if (master == Master.Employee.ToString()) id.Insert(0, 'E');
            if (master == Master.Customer.ToString()) id.Insert(0, 'C');
            if (master == Master.Vehicle.ToString()) id.Insert(0, 'V');
            if (master == Master.Transaction.ToString()) id.AppendFormat($"T{DateTime.Now.ToString("yyyy")}");

            var character = "1234567890";
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                id.Append(character[random.Next(character.Length)]);
            }
            return id.ToString();
        }
    }
}
