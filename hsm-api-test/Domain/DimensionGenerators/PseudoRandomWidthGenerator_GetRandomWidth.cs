using hsm_api.Domain.DimensionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.DimensionGenerators
{
    public class PseudoRandomWidthGenerator_GetRandomWidth
    {
        [Theory]
        [InlineData(1000, 2000)]
        [InlineData(1000, 1500)]
        [InlineData(1500, 2000)]
        [InlineData(1000, 1002)]
        public void GetRandomWidth_Limits_Considered(float lowLimit, float highLimit)
        {
            var generator = new PseudoRandomWidthGenerator(lowLimit, highLimit);
            float randomWidth = generator.GetRandomWidth();
            Assert.True(randomWidth > lowLimit, "Random width should be higher than low limit");
            Assert.True(randomWidth < lowLimit, "Random width should be smaller than high limit");
        }
    }
}
