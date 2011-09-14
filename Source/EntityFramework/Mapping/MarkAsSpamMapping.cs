namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class MarkAsSpamMapping : EntityTypeConfiguration<MarkAsSpam>
    {
        public MarkAsSpamMapping()
        {
            HasKey(s => new {s.UserId, s.StoryId});
            Property(s => s.FromIPAddress).HasColumnName("IPAddress").IsRequired().HasMaxLength(15);
            Property(s => s.MarkedAt).HasColumnName("Timestamp").IsRequired();
            HasRequired(s => s.ByUser).WithMany().HasForeignKey(v => v.UserId);
            HasRequired(s => s.ForStory).WithMany().HasForeignKey(v => v.StoryId);

            ToTable("StoryMarkAsSpam");
        }
    }
}
