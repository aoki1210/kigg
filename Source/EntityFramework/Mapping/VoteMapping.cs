namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class VoteMapping : EntityTypeConfiguration<Vote>
    {
        public VoteMapping()
        {
            HasKey(v => new {v.UserId, v.StoryId});
            
            Property(v => v.FromIPAddress).HasColumnName("IPAddress").IsRequired().HasMaxLength(15);
            Property(v => v.PromotedAt).HasColumnName("Timestamp").IsRequired();
            HasRequired(v => v.ByUser).WithMany().HasForeignKey(v=>v.UserId);
            HasRequired(v => v.ForStory).WithMany().HasForeignKey(v => v.StoryId);

            ToTable("StoryVote");
        }

    }
}
