namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using Domain.Entities;

    public class StoryViewMapping : EntityTypeConfiguration<StoryView>
    {
        public StoryViewMapping()
        {
            HasKey(v => v.Id);
            Property(v => v.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(v => v.FromIPAddress).HasColumnName("IPAddress").IsRequired().HasMaxLength(15);
            Property(v => v.ViewedAt).HasColumnName("Timestamp").IsRequired();
            HasRequired(v => v.ForStory).WithMany().Map(v => v.MapKey("StoryId"));

            ToTable("StoryView");
        }

    }
}
