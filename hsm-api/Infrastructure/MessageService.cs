using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure
{
    public static class MessageService
    {
        public static void IsKnownMessageName(string name)
        {
            throw new NotImplementedException();
        }

        public static string FormatMessageName(string name)
        {
            var formatedName = name.ToUpper();
            var rgx = new Regex("[^A-Z]");
            formatedName = rgx.Replace(formatedName, "");
            return formatedName;
        }
    }
}
