using hsm_api.Infrastructure.ValueGenerators;
using Microsoft.EntityFrameworkCore;

namespace hsm_api.Models.Messages
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }
        public DbSet<StartProductionMessage> StartProductionMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StartProductionMessage>()
                .Property(x => x.CoilId)
                .HasValueGenerator<CoilIdValueGenerator>();
        }
    }
}
