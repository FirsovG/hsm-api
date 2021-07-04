using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure.ValueGenerators
{
    /// <summary>
    /// Property with this value generator attached will check if <see cref="ConditionFulfillmentValueGenerator.IsFulfilledCondition(T)"/> is true. <para />
    /// In case it is <see cref="ConditionFulfillmentValueGenerator.GetNewValue(EntityEntry)"/> is called to generate new value. <br/>
    /// In case it isn't the current value of the property is returned.
    /// </summary>
    /// <typeparam name="T">Type of value to generate</typeparam>
    abstract public class ConditionFulfillmentValueGenerator<T> : ValueGenerator<T>
    {
        protected abstract string PropertyName { get; }

        public override bool GeneratesTemporaryValues => false;

        public override T Next(EntityEntry entry)
        {
            T propertyValue = (T)entry.Property(PropertyName).OriginalValue;
            if (IsFulfilledCondition(propertyValue))
                return GetNewValue(entry);
            else
                return propertyValue;
        }

        protected abstract bool IsFulfilledCondition(T propertyValue);

        protected abstract T GetNewValue(EntityEntry entry);
    }
}
