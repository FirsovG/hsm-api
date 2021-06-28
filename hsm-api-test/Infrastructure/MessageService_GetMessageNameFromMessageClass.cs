using hsm_api.Infrastructure;
using hsm_api.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Infrastructure
{
    public class MessageService_GetMessageNameFromMessageClass
    {
        [Fact]
        public void GetMessageNameFromMessageClass_StartProduction_Formated_As_Expected()
        {
            var messageName = MessageService.GetMessageNameFromMessageClass(typeof(StartProductionMessage));
            Assert.Equal("STARTPRODUCTION", messageName);
        }

        [Fact]
        public void GetMessageNameFromMessageClass_Throw_When_Not_Message()
        {
            var expection = Assert.Throws<ArgumentException>(() => MessageService.GetMessageNameFromMessageClass(typeof(int)));
            Assert.Equal("Type is not derived from Message class", expection.Message);
        }
    }
}
