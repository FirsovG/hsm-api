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
    public class DimensionValueGenerator<T> : ValueGenerator<float> where T : IDimensionGeneratorSettings
    {
        private PseudoRandomDimensionGenerator<T> _generator;

        public override bool GeneratesTemporaryValues => false;

        public override float Next(EntityEntry entry)
        {
            // Workaround, because only parameterless constructor is called
            // https://github.com/dotnet/efcore/issues/10792#issuecomment-560134363
            if (_generator == null)
                _generator = entry.Context.GetService<PseudoRandomDimensionGenerator<T>>();

            return _generator.GetValue();
        }
    }
}
