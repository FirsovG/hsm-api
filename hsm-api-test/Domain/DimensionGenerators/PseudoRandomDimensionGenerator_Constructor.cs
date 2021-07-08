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
    public class PseudoRandomDimensionGenerator_Constructor
    {
        [Theory]
        [InlineData(-5, 10)]
        [InlineData(10, -5)]
        [InlineData(-10, -5)]
        public void Constructor_Not_Accept_Negative_Limits(float minLimit, float maxLimit)
        {
            var settings = Mock.Of<IDimensionGeneratorSettings>(_ => _.MinLimit == minLimit && _.MaxLimit == maxLimit);
            var settingsWrapper = Mock.Of<IOptionsMonitor<IDimensionGeneratorSettings>>(_ => _.CurrentValue == settings);
            Action construction = () => new PseudoRandomDimensionGenerator<IDimensionGeneratorSettings>(settingsWrapper);

            var exception = Assert.Throws<ArgumentException>(construction);
            Assert.Contains("negative", exception.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("limit", exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
