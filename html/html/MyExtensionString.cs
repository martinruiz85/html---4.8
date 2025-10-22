using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilETWeb
{
    public static class MyExtensionString
    {
        public static bool TryParseGuid(this string s, out Guid guid)
        {
            try
            {
                guid = new Guid(s);
                return true;
            }
            catch
            {
                guid = Guid.Empty;
                return false;
            }
        }
    }
}
