using hsm_api.Models.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Infrastructure
{
    public class CoilIdFormatJsonConverter_Write
    {
        [Fact]
        public void Write_Coil_Id_Contain_HB_Prefix()
        {
            var message = new StartProductionMessage();

            var messageJson = JsonSerializer.Serialize(message);
            var coilId = JObject.Parse(messageJson)["CoilId"].ToString();

            Assert.StartsWith("HB", coilId);
        }

        [Fact]
        public void Write_Coil_Id_Length_12()
        {
            var message = new StartProductionMessage();

            var messageJson = JsonSerializer.Serialize(message);
            var coilId = JObject.Parse(messageJson)["CoilId"].ToString();

            Assert.Equal(12, coilId.Length);
        }

        [Fact]
        public void Write_Coil_Id_Ends_With_1()
        {
            var message = new StartProductionMessage();
            message.CoilId = 1;

            var messageJson = JsonSerializer.Serialize(message);
            var coilId = JObject.Parse(messageJson)["CoilId"].ToString();

            Assert.EndsWith("1", coilId);
        }
    }
}
