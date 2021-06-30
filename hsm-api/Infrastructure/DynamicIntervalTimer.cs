using hsm_api.ConfigurationOptions.TimerSettings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace hsm_api.Infrastructure
{
    public class DynamicIntervalTimer<T> : IDynamicIntervalTimer<T>, IDisposable where T : ITimerSettings
    {
        private readonly List<ElapsedEventHandler> _eventHandlers;
        private Timer _timer;
        protected T _timerSettings;

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
        /// Autoreset-Timer with interval from <see cref="T.Interval"/>
        /// Timer is started after construction
        /// </summary>
        public DynamicIntervalTimer(IOptionsMonitor<T> settingsWrapper)
        {
            _eventHandlers = new List<ElapsedEventHandler>();
            UpdateTimerSettings(settingsWrapper.CurrentValue);
            settingsWrapper.OnChange(UpdateTimerSettings);
        }

        public void UpdateTimerSettings(T settings)
        {
            _timerSettings = settings;
            Timer newTimer = RecreateTimer();
            ResubscribeEventHandlers(newTimer);
            _timer = newTimer;
            _timer.Start();
        }

        private Timer RecreateTimer() => new Timer() { AutoReset = true, Interval = _timerSettings.Interval };

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
