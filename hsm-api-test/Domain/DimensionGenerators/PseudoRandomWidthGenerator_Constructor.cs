using hsm_api.Domain.DimensionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.DimensionGenerators
{
    public class PseudoRandomWidthGenerator_Constructor
    {
        [Theory]
        [InlineData(-5, 10)]
        [InlineData(10, -5)]
        [InlineData(-10, -5)]
        public void GetRandomWidth_Not_Accept_Negative_Limits(float lowLimit, float highLimit)
        {
            Action construction = () => new PseudoRandomWidthGenerator(lowLimit, highLimit);

            var exception = Assert.Throws<ArgumentException>(construction);
            Assert.Contains("negative", exception.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("limit", exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
