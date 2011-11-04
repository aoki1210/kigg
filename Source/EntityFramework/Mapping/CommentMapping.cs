namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using Domain.Entities;

    public class CommentMapping : EntityTypeConfiguration<Comment>
    {
        public CommentMapping()
        {
            HasKey(c => c.Id);
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.CreatedAt).IsRequired();
            Property(c => c.HtmlBody).IsRequired().IsMaxLength();
            Property(c => c.TextBody).IsRequired().IsMaxLength();
            Property(c => c.FromIPAddress).HasColumnName("IPAddress").IsRequired().HasMaxLength(15);
            Property(c => c.IsOffended).IsRequired();
            HasRequired(c => c.ByUser).WithMany().Map(m => m.MapKey("UserId"));
            HasRequired(c => c.ForStory).WithMany().Map(m => m.MapKey("StoryId"));

            ToTable("StoryComment");
        }
    }
}
