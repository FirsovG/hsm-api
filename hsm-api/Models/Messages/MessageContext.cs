using hsm_api.ConfigurationOptions.DimensionSettings;
using hsm_api.Infrastructure.ValueGenerators;
using Microsoft.EntityFrameworkCore;

namespace hsm_api.Models.Messages
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }
        public DbSet<StartProductionMessage> StartProductionMessages { get; set; }
        public DbSet<FinishProductionMessage> FinishProductionMessages { get; set; }
        public DbSet<ProductionStatusMessage> ProductionStatusMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StartProductionMessage>()
                .Property(x => x.CoilId)
                .HasValueGenerator<CoilIdValueGenerator>();

            builder.Entity<FinishProductionMessage>()
                .Property(x => x.Width)
                .HasValueGenerator<DimensionValueGenerator<WidthGeneratorSettings>>();

            builder.Entity<FinishProductionMessage>()
                .Property(x => x.Weight)
                .HasValueGenerator<DimensionValueGenerator<WeightGeneratorSettings>>();

            builder.Entity<FinishProductionMessage>()
                .Property(x => x.Thickness)
                .HasValueGenerator<DimensionValueGenerator<ThicknessGeneratorSettings>>();

            builder.Entity<ProductionStatusMessage>()
                .Property(x => x.MillSpeed)
                .HasValueGenerator<DimensionValueGenerator<MillSpeedGeneratorSettings>>();

            builder.Entity<ProductionStatusMessage>()
                .Property(x => x.CoilingSpeed)
                .HasValueGenerator<DimensionValueGenerator<CoilingSpeedGeneratorSettings>>();
        }
    }
}
