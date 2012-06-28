using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiled
{
    internal static class SeveralExtensions
    {
        public static string ElementsToString<T>(this List<T> list)
        {
            if (list.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                    sb.Append(list[i].ToString() + ",");
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            else return "";
        }

        public static string ElementsToString<T, String>(this Dictionary<T, String> dictionary)
        {
            if (dictionary.Count() != 0)
            {
                StringBuilder sb = new StringBuilder("[");
                foreach (KeyValuePair<T, String> entry in dictionary)
                    sb.Append(entry.Key + "=" + entry.Value + ",");
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                return sb.ToString();
            }
            else return "";
        }
    }
}
