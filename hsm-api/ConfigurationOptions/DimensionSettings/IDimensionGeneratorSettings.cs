using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.ConfigurationOptions.DimensionSettings
{
    public interface IDimensionGeneratorSettings
    {
        public float MinLimit { get; set; }
        public float MaxLimit { get; set; }
    }
}
