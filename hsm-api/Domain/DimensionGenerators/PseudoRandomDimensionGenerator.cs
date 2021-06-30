using hsm_api.ConfigurationOptions.DimensionSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Domain.DimensionGenerators
{
    public class PseudoRandomDimensionsGenerator<T> where T : IDimensionGeneratorSettings
    {
        private readonly Random _randomizer;

        private readonly float _lowLimit;
        private readonly float _highLimit;

        /// <summary>
        /// Provide own randomizer
        /// </summary>
        /// <param name="randomizer">Self configured randomizer</param>
        public PseudoRandomDimensionsGenerator(Random randomizer, float lowLimit, float highLimit)
        {
            if (lowLimit < 0)
                throw new ArgumentException("Low limit cannot be a negative value");
            if (highLimit < 0)
                throw new ArgumentException("High limit cannot be a negative value");
            if (highLimit - lowLimit < 2)
                throw new ArgumentException("To create value in range, the limit difference should be greater or equal 2");

            _randomizer = randomizer;
            _lowLimit = lowLimit;
            _highLimit = highLimit;
        }

        /// <summary>
        /// Create with standart <see cref="System.Random()"/>
        /// </summary>
        public PseudoRandomDimensionsGenerator(float lowLimit, float highLimit) : this (new Random(), lowLimit, highLimit) { }

        public float GetValue()
        {
            return (float)(_randomizer.NextDouble() * (_highLimit - _lowLimit) + _lowLimit);
        }
    }
}
