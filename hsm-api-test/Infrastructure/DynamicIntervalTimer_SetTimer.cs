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
    public class DynamicIntervalTimer_SetTimer
    {
        [Fact]
        public void SetTimer_Resubscribes()
        {
            var longTime = new TimerSettings { StartProduction = 50000 };
            var shortTime = new TimerSettings { StartProduction = 1 };
            var timer = new DynamicIntervalTimer(Mock.Of<IOptionsMonitor<TimerSettings>>(_ => _.CurrentValue == longTime));
            int timeElapsedCount = 0;

            timer.TimeElapsed += (_, _) => timeElapsedCount++;
            timer.SetTimer(shortTime);
            Task.Delay(10).Wait();

            Assert.True(timeElapsedCount > 0, "Timer fired at least once");
        }
    }
}
