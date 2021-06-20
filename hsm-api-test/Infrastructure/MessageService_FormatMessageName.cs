using hsm_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Infrastructure
{
    public class MessageService_FormatMessageName
    {
        [Fact]
        public void FormatMessageName_Use_Uppercase()
        {
            var formatedName = MessageService.FormatMessageName("STARTproduction");
            Assert.Equal("STARTPRODUCTION", formatedName);
        }

        [Theory]
        [InlineData("START_PRODUCTION")]
        [InlineData("START__PRODUCTION")]
        [InlineData("START$PRODUCTION")]
        public void FormatMessageName_Remove_Non_Alphabetic(string unformatedName)
        {
            var formatedName = MessageService.FormatMessageName(unformatedName);
            Assert.Equal("STARTPRODUCTION", formatedName);
        }
    }
}
