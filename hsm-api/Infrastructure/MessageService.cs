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

            if (name == GetMessageNameFromMessageClass(typeof(StartProductionMessage)) ||
                name == GetMessageNameFromMessageClass(typeof(FinishProductionMessage)) ||
                name == GetMessageNameFromMessageClass(typeof(ProductionStatusMessage)))
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

        public static string GetMessageNameFromMessageClass(Type messageClass)
        {
            if (!messageClass.IsSubclassOf(typeof(Message)))
                throw new ArgumentException("Type is not derived from Message class");

            string className = messageClass.Name;
            className = className.ToUpper().Replace("MESSAGE", "");
            return FormatMessageName(className);
        }
    }
}
