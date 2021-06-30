using hsm_api.ConfigurationOptions.DimensionSettings;
using hsm_api.Domain.DimensionGenerators;
using Microsoft.Extensions.Options;
using Moq;
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
            var settings = Mock.Of<IDimensionGeneratorSettings>(_ => _.MinLimit == minLimit && _.MaxLimit == maxLimit );
            var settingsWrapper = Mock.Of<IOptionsMonitor<IDimensionGeneratorSettings>>(_ => _.CurrentValue == settings);
            var generator = new PseudoRandomDimensionGenerator<IDimensionGeneratorSettings>(settingsWrapper);
            float randomValue = generator.GetValue();
            Assert.True(randomValue > settings.MinLimit, "Random value should be higher than min limit");
            Assert.True(randomValue < settings.MaxLimit, "Random value should be smaller than max limit");
        } 

        [Fact]
        public void GetValue_Randomizer_Depend_On_Seed()
        {
            const int seed = 5;
            var settings = Mock.Of<IDimensionGeneratorSettings>(_ => _.MinLimit == 500 && _.MaxLimit == 1000);
            var settingsWrapper = Mock.Of<IOptionsMonitor<IDimensionGeneratorSettings>>(_ => _.CurrentValue == settings);
            var randomizer1 = new Random(seed);
            var generator1 = new PseudoRandomDimensionGenerator<IDimensionGeneratorSettings>(randomizer1, settingsWrapper);
            var generatedValue1 = new float[10];
            var randomizer2 = new Random(seed);
            var generator2 = new PseudoRandomDimensionGenerator<IDimensionGeneratorSettings>(randomizer2, settingsWrapper);
            var generatedValue2 = new float[10];

            for (int i = 0; i < generatedValue1.Length; i++)
                generatedValue1[i] = generator1.GetValue();
            for (int i = 0; i < generatedValue2.Length; i++)
                generatedValue2[i] = generator2.GetValue();

            Assert.Equal(generatedValue1, generatedValue2);
        }
    }
}
