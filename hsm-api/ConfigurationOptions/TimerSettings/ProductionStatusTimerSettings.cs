﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.ConfigurationOptions.TimerSettings
{
    public class ProductionStatusTimerSettings : ITimerSettings
    {
        public int Interval { get; set; }
    }
}
