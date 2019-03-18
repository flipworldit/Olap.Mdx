using System.Linq;

namespace SystemExt
{
    public static class ArrayExt
    {
        public static string[] ToStringArray(this int[] intArray)
        {
            return intArray
                .Select(el => el.ToString())
                .ToArray();
        }
    }
}