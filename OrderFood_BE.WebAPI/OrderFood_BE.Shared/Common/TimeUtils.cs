using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Shared.Common
{
    public static class TimeUtils
    {
        public static List<string> DetermineMeal(TimeSpan currentTime)
        {
            var meals = new List<string>();
            meals.Add("Soft drink");

            if (currentTime >= TimeSpan.FromHours(5) && currentTime < TimeSpan.FromHours(10))
                meals.Add("Breakfast");

            if (currentTime >= TimeSpan.FromHours(10) && currentTime < TimeSpan.FromHours(14))
                meals.Add("Lunch");

            if (currentTime >= TimeSpan.FromHours(17) && currentTime < TimeSpan.FromHours(21))
                meals.Add("Dinner");

            if (currentTime >= TimeSpan.FromHours(9) && currentTime < TimeSpan.FromHours(22))
                meals.Add("Fast food");

            if (meals.Count == 0)
                meals.Add("Other");

            return meals;
        }
    }
}
