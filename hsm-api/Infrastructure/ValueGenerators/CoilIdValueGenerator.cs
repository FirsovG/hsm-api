﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hsm_api.Infrastructure.ValueGenerators
{
    public class CoilIdValueGenerator : ValueGenerator<string>
    {
        private int _current;
        private readonly string propertyName = nameof(CoilIdValueGenerator).Replace("ValueGenerator", "");

        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            string propertyValue = (string)entry.Property(propertyName).OriginalValue;
            if (propertyValue == null)
            {
                Interlocked.Increment(ref _current);
                return "HB" + _current.ToString().PadLeft(10, '0');
            }
            else
                return propertyValue;
        }
    }
}
