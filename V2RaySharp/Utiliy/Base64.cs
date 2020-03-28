using System;
using System.Text;

namespace V2RaySharp.Utiliy
{
    class Base64
    {
        public static string Decode(string text)
        {
            var s = text.Replace("-", "+").Replace("_", "/");
            var i = s.Length % 4;
            if (i == 2)
            {
                s = s.PadRight(s.Length + 2, '=');
            }
            else if (i == 1 || i == 3)
            {
                s = s.PadRight(s.Length + 1, '=');
            }
            var b = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(b);
        }
    }
}
