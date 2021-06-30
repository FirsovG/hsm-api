using hsm_api.Domain.DimensionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.DimensionGenerators
{
    public class PseudoRandomDimensionsGenerator_GetValue
    {
        [Theory]
        [InlineData(1000, 2000)]
        [InlineData(1000, 1500)]
        [InlineData(1500, 2000)]
        [InlineData(1000, 1002)]
        public void GetRandomWidth_Limits_Considered(float lowLimit, float highLimit)
        {
            var generator = new PseudoRandomDimensionGenerator(lowLimit, highLimit);
            float randomValue = generator.GetValue();
            Assert.True(randomValue > lowLimit, "Random value should be higher than low limit");
            Assert.True(randomValue < highLimit, "Random value should be smaller than high limit");
        } 

        [Fact]
        public void GetRandomWidth_Randomizer_Depend_On_Seed()
        {
            const int seed = 5;
            const int lowLimit = 500;
            const int highLimit = 1000;
            var randomizer1 = new Random(seed);
            var generator1 = new PseudoRandomDimensionGenerator(randomizer1, lowLimit, highLimit);
            var generatedValue1 = new float[10];
            var randomizer2 = new Random(seed);
            var generator2 = new PseudoRandomDimensionGenerator(randomizer2, lowLimit, highLimit);
            var generatedValue2 = new float[10];

            for (int i = 0; i < generatedValue1.Length; i++)
                generatedValue1[i] = generator1.GetValue();
            for (int i = 0; i < generatedValue2.Length; i++)
                generatedValue2[i] = generator2.GetValue();

            Assert.Equal(generatedValue1, generatedValue2);
        }
    }
}
