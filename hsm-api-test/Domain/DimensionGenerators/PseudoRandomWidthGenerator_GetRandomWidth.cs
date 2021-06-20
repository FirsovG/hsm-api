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

        [Theory]
        [InlineData(-5, 10)]
        [InlineData(10, -5)]
        [InlineData(-10, -5)]
        public void GetRandomWidth_Not_Accept_Negative_Limits(float lowLimit, float highLimit)
        {
            var generator = new PseudoRandomWidthGenerator(lowLimit, highLimit);

            var exception = Assert.Throws<ArgumentException>(() => generator.GetRandomWidth());
            Assert.Contains("negative", exception.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("limit", exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetRandomWidth_Randomizer_Depend_On_Seed()
        {
            const int seed = 5;
            const int lowLimit = 500;
            const int highLimit = 1000;
            var randomizer1 = new Random(seed);
            var generator1 = new PseudoRandomWidthGenerator(randomizer1, lowLimit, highLimit);
            var generatedWidth1 = new float[10];
            var randomizer2 = new Random(seed);
            var generator2 = new PseudoRandomWidthGenerator(randomizer2, lowLimit, highLimit);
            var generatedWidth2 = new float[10];

            for (int i = 0; i < generatedWidth1.Length; i++)
                generatedWidth1[i] = generator1.GetRandomWidth();
            for (int i = 0; i < generatedWidth2.Length; i++)
                generatedWidth2[i] = generator2.GetRandomWidth();

            Assert.Equal(generatedWidth1, generatedWidth2);
        }
    }
}
