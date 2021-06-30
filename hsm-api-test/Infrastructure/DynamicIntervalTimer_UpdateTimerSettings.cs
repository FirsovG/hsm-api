using hsm_api.ConfigurationOptions;
using hsm_api.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Infrastructure
{
    public class DynamicIntervalTimer_UpdateTimerSettings
    {
        [Fact]
        public void UpdateTimerSettings_Resubscribes()
        {
            // StartProductionTimerSettings was selected rendomly
            var longInterval = new StartProductionTimerSettings { Interval = 50000 };
            var shortInterval = new StartProductionTimerSettings { Interval = 1 };
            var timer = new DynamicIntervalTimer<StartProductionTimerSettings>(Mock.Of<IOptionsMonitor<StartProductionTimerSettings>>(_ => _.CurrentValue == longInterval));
            int timeElapsedCount = 0;

            timer.TimeElapsed += (_, _) => timeElapsedCount++;
            timer.UpdateTimerSettings(shortInterval);
            Task.Delay(50).Wait();

            Assert.True(timeElapsedCount > 0, "Timer fired at least once");
        }
    }
}
