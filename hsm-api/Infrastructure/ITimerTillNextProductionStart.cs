using System.Timers;

namespace hsm_api.Infrastructure
{
    public interface ITimerTillNextProductionStart
    {
        event ElapsedEventHandler TimeElapsed;

        void Dispose();
        void Start();
    }
}