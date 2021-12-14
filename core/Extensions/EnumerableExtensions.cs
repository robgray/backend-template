using System.Collections.Generic;
using System.Linq;

namespace core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Sample<T>(this IEnumerable<T> input, int sample)
        {
            return input.Where((point, index) => index == 0 || index == input.Count() - 1 || index % sample == 0);
        }
    }
}