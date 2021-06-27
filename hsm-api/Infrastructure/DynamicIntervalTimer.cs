using hsm_api.ConfigurationOptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace hsm_api.Infrastructure
{
    public class DynamicIntervalTimer : IDisposable, IDynamicIntervalTimer
    {
        private readonly List<ElapsedEventHandler> _eventHandlers;
        private Timer _timer;

        /// <summary>
        /// Sends event when the production of new coil started
        /// </summary>
        public event ElapsedEventHandler TimeElapsed
        {
            add
            {
                _eventHandlers.Add(value);
                _timer.Elapsed += value;
            }
            remove
            {
                _eventHandlers.Remove(value);
                _timer.Elapsed -= value;
            }
        }

        /// <summary>
        /// Autoreset-Timer with interval of <see cref="TimerSettings.StartProduction"/>
        /// </summary>
        public DynamicIntervalTimer(IOptionsMonitor<TimerSettings> settings)
        {
            _eventHandlers = new List<ElapsedEventHandler>();
            SetTimer(settings.CurrentValue);
            settings.OnChange(SetTimer);
        }

        public void SetTimer(TimerSettings settings)
        {
            var newTimer = new Timer() { AutoReset = true, Interval = settings.StartProduction };
            ResubscribeEventHandlers(newTimer);
            _timer = newTimer;
            _timer.Start();
        }

        private void ResubscribeEventHandlers(Timer newTimer)
        {
            foreach (var handler in _eventHandlers)
            {
                _timer.Elapsed -= handler;
                newTimer.Elapsed += handler;
            }
        }

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose() => _timer.Dispose();
    }
}
