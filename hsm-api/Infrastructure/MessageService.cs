using hsm_api.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure
{
    public static class MessageService
    {
        // TODO: Fix open closed principle violation
        public static bool IsKnownMessageName(string name)
        {
            if (FormatMessageName(name) != name)
                throw new FormatException($"Invalid message name format, use {nameof(MessageService.FormatMessageName)} before");

            var nameWithSuffix = name + "MESSAGE";
            if (nameWithSuffix == FormatMessageName(nameof(StartProductionMessage)))
                return true;
            else
                return false;
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
