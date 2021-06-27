using System.Timers;

namespace hsm_api.Infrastructure
{
    public interface IDynamicIntervalTimer
    {
        event ElapsedEventHandler TimeElapsed;

        void Dispose();
    }
}