using hsm_api.ConfigurationOptions.DimensionSettings;
using hsm_api.Domain.DimensionGenerators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure.ValueGenerators
{
    public class DimensionValueGenerator<T> : ConditionFulfillmentValueGenerator<float> where T : IDimensionGeneratorSettings
    {
        private PseudoRandomDimensionGenerator<T> _generator;

        protected override string PropertyName => typeof(T).Name.Replace("GeneratorSettings", "");

        protected override bool IsFulfilledCondition(float propertyValue) => propertyValue == 0;

        protected override float GetNewValue(EntityEntry entry)
        {
            // Workaround, because only parameterless constructor is called
            // https://github.com/dotnet/efcore/issues/10792#issuecomment-560134363
            if (_generator == null)
                _generator = entry.Context.GetService<PseudoRandomDimensionGenerator<T>>();

            return _generator.GetValue();
        }
    }
}
