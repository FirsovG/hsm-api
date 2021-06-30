using hsm_api.ConfigurationOptions.DimensionSettings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Domain.DimensionGenerators
{
    public class PseudoRandomDimensionGenerator<T> where T : IDimensionGeneratorSettings
    {
        private readonly Random _randomizer;
        private T _settings;

        /// <summary>
        /// Provide own randomizer
        /// </summary>
        /// <param name="randomizer">Self configured randomizer</param>
        public PseudoRandomDimensionGenerator(Random randomizer, IOptionsMonitor<T> settingsWrapper)
        {
            UpdateSettings(settingsWrapper.CurrentValue);
            settingsWrapper.OnChange(UpdateSettings);
            _randomizer = randomizer;
        }

        public void UpdateSettings(T settings)
        {
            if (settings.MinLimit < 0)
                throw new ArgumentException("Min limit cannot be a negative value");
            if (settings.MaxLimit < 0)
                throw new ArgumentException("Max limit cannot be a negative value");
            if (settings.MaxLimit - settings.MinLimit < 2)
                throw new ArgumentException("To create value in range, the limit difference should be greater or equal 2");

            _settings = settings;
        }

        /// <summary>
        /// Create with standart <see cref="System.Random()"/>
        /// </summary>
        public PseudoRandomDimensionGenerator(IOptionsMonitor<T> settingsWrapper) : this (new Random(), settingsWrapper) { }

        public float GetValue()
        {
            return (float)(_randomizer.NextDouble() * (_settings.MaxLimit - _settings.MinLimit) + _settings.MinLimit);
        }
    }
}
