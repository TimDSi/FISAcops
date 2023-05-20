using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    static class NextCallDate
    {
        static readonly DateTime startDate = DateTime.Now;
        private static readonly List<DateTime> holidays = GetHolidaysForYear(startDate.Year); 

        private static List<DateTime> GetHolidaysForYear(int year)
        {
            List<DateTime> holidays = new()
            {
                // Jours fériés fixes
                new DateTime(year, 1, 1), // 1er janvier
                new DateTime(year, 5, 1), // 1er mai
                new DateTime(year, 5, 8), // 8 mai
                new DateTime(year, 7, 14), // 14 juillet
                new DateTime(year, 8, 15), // 15 août
                new DateTime(year, 11, 1), // 1er novembre
                new DateTime(year, 11, 11), // 11 novembre
                new DateTime(year, 12, 25) // 25 décembre
            };

            // Vacances d'été
            DateTime firstJune = new(year, 6, 1);
            DateTime LastSeptember = new(year, 9, 30);

            for (DateTime date = firstJune; date <= LastSeptember; date = date.AddDays(1))
            {
                holidays.Add(date);
            }

            return holidays;
        }

        public static DateTime GetNextValidDay(DateTime startDate)
        {
            DateTime nextDate = startDate.AddDays(1);

            while (!IsValidDate(nextDate))
            {
                nextDate = nextDate.AddDays(1);
            }

            return nextDate;
        }

        public static DateTime GetNextValidWeek(DateTime startDate)
        {
            DateTime nextDate = startDate.AddDays(7);

            //test différent pour les weekends
            while (holidays.Contains(nextDate))
            {
                nextDate = nextDate.AddDays(7);
            }
            return nextDate;
        }

        private static bool IsValidDate(DateTime date)
        {
            // Vérifier si la date est un samedi, un dimanche ou un jour férié
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(date))
            {
                return false;
            }

            return true;
        }
    }
}
