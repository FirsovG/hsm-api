using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.ConfigurationOptions
{
    public interface ITimerSettings
    {
        public int Interval { get; set; }
    }
}
