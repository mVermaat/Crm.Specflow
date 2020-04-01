using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Expressions
{
    public class FormulaFunctions
    {
        public static DateTime Today()
        {
            return DateTime.Today;
        }

        public static TimeSpan Days(double amount)
        {
            return TimeSpan.FromDays(amount);
        }

        public static TimeSpan Hours(double amount)
        {
            return TimeSpan.FromHours(amount);
        }

        public static TimeSpan Minutes(double amount)
        {
            return TimeSpan.FromMinutes(amount);
        }
    }
}
