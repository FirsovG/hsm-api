﻿using hsm_api.ConfigurationOptions;
using System.Timers;

namespace hsm_api.Infrastructure
{
    public interface IDynamicIntervalTimer<T> where T : ITimerSettings
    {
        event ElapsedEventHandler TimeElapsed;
        void UpdateTimerSettings(T settings);
        void Dispose();
    }
}