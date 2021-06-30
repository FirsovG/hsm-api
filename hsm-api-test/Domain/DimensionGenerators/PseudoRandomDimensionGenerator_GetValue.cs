using hsm_api.ConfigurationOptions.DimensionSettings;
using hsm_api.Domain.DimensionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.DimensionGenerators
{
    public class PseudoRandomDimensionGenerator_GetValue
    {
        [Theory]
        [InlineData(1000, 2000)]
        [InlineData(1000, 1500)]
        [InlineData(1500, 2000)]
        [InlineData(1000, 1002)]
        public void GetValue_Limits_Considered(float minLimit, float maxLimit)
        {
            var settings = new WidthGeneratorSettings { MinLimit = minLimit, MaxLimit = maxLimit };
            var generator = new PseudoRandomDimensionGenerator<WidthGeneratorSettings>(settings);
            float randomValue = generator.GetValue();
            Assert.True(randomValue > settings.MinLimit, "Random value should be higher than min limit");
            Assert.True(randomValue < settings.MaxLimit, "Random value should be smaller than max limit");
        } 

        [Fact]
        public void GetValue_Randomizer_Depend_On_Seed()
        {
            const int seed = 5;
            var settings = new WidthGeneratorSettings { MinLimit = 500, MaxLimit = 1000 };
            var randomizer1 = new Random(seed);
            var generator1 = new PseudoRandomDimensionGenerator<WidthGeneratorSettings>(randomizer1, settings);
            var generatedValue1 = new float[10];
            var randomizer2 = new Random(seed);
            var generator2 = new PseudoRandomDimensionGenerator<WidthGeneratorSettings>(randomizer2, settings);
            var generatedValue2 = new float[10];

            for (int i = 0; i < generatedValue1.Length; i++)
                generatedValue1[i] = generator1.GetValue();
            for (int i = 0; i < generatedValue2.Length; i++)
                generatedValue2[i] = generator2.GetValue();

            Assert.Equal(generatedValue1, generatedValue2);
        }
    }
}
