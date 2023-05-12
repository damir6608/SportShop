using System.Collections.Generic;
using System.Linq;

namespace SportShop.Services
{
    internal static class OrderStorage
    {
        internal static IEnumerable<Product> products = new List<Product>();

        internal static bool ContainsProducts => products.Any();
    }
}
