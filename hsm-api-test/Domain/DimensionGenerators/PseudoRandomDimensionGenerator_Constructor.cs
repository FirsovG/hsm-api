using hsm_api.Domain.DimensionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.DimensionGenerators
{
    public class PseudoRandomDimensionGenerator_Constructor
    {
        [Theory]
        [InlineData(-5, 10)]
        [InlineData(10, -5)]
        [InlineData(-10, -5)]
        public void Constructor_Not_Accept_Negative_Limits(float lowLimit, float highLimit)
        {
            Action construction = () => new PseudoRandomDimensionGenerator(lowLimit, highLimit);

            var exception = Assert.Throws<ArgumentException>(construction);
            Assert.Contains("negative", exception.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("limit", exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Theory]
        [InlineData(1000, 1001)]
        [InlineData(1000, 1000)]
        [InlineData(1000, 500)]
        public void Constructor_Not_Accept_Limit_Difference_Lower_2(float lowLimit, float highLimit)
        {
            Action construction = () => new PseudoRandomDimensionGenerator(lowLimit, highLimit);

            var exception = Assert.Throws<ArgumentException>(construction);
            Assert.Contains("To create value in range, the limit difference should be greater or equal 2", 
                            exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
