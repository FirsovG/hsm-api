using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace hsm_api.Infrastructure
{
    public class TimerTillNextProductionStart : IDisposable, ITimerTillNextProductionStart
    {
        private Timer _timer;

        /// <summary>
        /// Sends event when the production of new coil started
        /// </summary>
        public event ElapsedEventHandler TimeElapsed { add => _timer.Elapsed += value; remove => _timer.Elapsed -= value; }

        /// <summary>
        /// Providing basic timer with auto reset and two minutes interval
        /// </summary>
        public TimerTillNextProductionStart() : this(new Timer() { AutoReset = true, Interval = 120000 }) { }

        /// <summary>
        /// Overload with selfconfigured timer
        /// </summary>
        /// <param name="timer">Provided timer with selfconfigured timer values</param>
        public TimerTillNextProductionStart(Timer timer) => _timer = timer;

        /// <summary>
        /// Start countdown for processing of new coil
        /// </summary>
        public void Start() => _timer.Start();

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose() => _timer.Dispose();
    }
}
