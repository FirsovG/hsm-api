using hsm_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Infrastructure
{
    public class MessageService_IsKnownMessageName
    {
        [Theory]
        [InlineData("START$PRODUCTION")]
        [InlineData("STARTproduction")]
        public void IsKnownMessageName_Throw_Exception_On_Bad_Format_With_Message(string messageName)
        {
            var expectedMessage = $"Invalid message name format, use {nameof(MessageService.FormatMessageName)} before";
            Action methodCall = () => MessageService.IsKnownMessageName(messageName);
            
            var exception = Assert.Throws<FormatException>(methodCall);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void IsKnownMessageName_Start_Production_Is_Known()
        {
            var isKnown = MessageService.IsKnownMessageName("STARTPRODUCTION");
            Assert.True(isKnown, "Start production should be known");
        }
    }
}
