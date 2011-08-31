namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class UserScoreMapping : EntityTypeConfiguration<UserScore>
    {
        public UserScoreMapping()
        {
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.Score).IsRequired();
            Property(s => s.CreatedAt).IsRequired().HasColumnName("Timestamp");
            this.Property<UserScore, int>("ActionType").IsRequired();
            HasRequired(s => s.ScoredBy).WithMany().Map(s => s.MapKey("UserId"));
            
            ToTable("UserScore");
        }
    }
}
