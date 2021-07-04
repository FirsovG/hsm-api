using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure.ValueGenerators
{
    public class CoilIdValueGenerator : ConditionFulfillmentValueGenerator<string>
    {
        private int _current;

        protected override string PropertyName => 
            nameof(CoilIdValueGenerator).Replace("ValueGenerator", "");

        protected override bool IsFulfilledCondition(string propertyValue) => propertyValue == null;

        protected override string GetNewValue(EntityEntry entry)
        {
            Interlocked.Increment(ref _current);
            return "HB" + _current.ToString().PadLeft(10, '0');
        }
    }
}
