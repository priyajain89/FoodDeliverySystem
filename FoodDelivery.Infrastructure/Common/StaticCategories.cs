using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Common
{
    public static class StaticCategories
    {
        public static readonly List<string> Categories = new()
    {
        "Beverages",
        "Main Course",
        "Desserts",
        "Snacks",
        "Salads"
    };
    }

}
