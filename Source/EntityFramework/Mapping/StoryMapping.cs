namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class StoryMapping : EntityTypeConfiguration<Story>
    {
        public StoryMapping()
        {
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.Title).IsRequired().IsUnicode().HasMaxLength(256);
            Property(s => s.UniqueName).IsRequired().IsUnicode().HasMaxLength(256);
            Property(s => s.FromIPAddress).HasColumnName("IPAddress").IsRequired().HasMaxLength(15);
            Property(s => s.HtmlDescription).IsRequired().IsMaxLength();
            Property(s => s.TextDescription).IsRequired().IsMaxLength();
            Property(s => s.CreatedAt).IsRequired();
            Property(s => s.ApprovedAt).IsOptional();
            Property(s => s.LastActivityAt).IsOptional();
            Property(s => s.LastProcessedAt).IsRequired();
            Property(s => s.PublishedAt).IsOptional();
            Property(s => s.Rank).IsOptional();
            Property(s => s.Url).IsRequired().IsUnicode().HasMaxLength(2048);
            Property(s => s.UrlHash).IsRequired().IsUnicode().IsFixedLength().HasMaxLength(24);
            HasRequired(s => s.PostedBy).WithMany().Map(m => m.MapKey("UserId"));
            HasRequired(s => s.BelongsTo).WithMany().Map(m => m.MapKey("CategoryId"));
            HasMany(s => s.Tags).WithMany(t => t.Stories)
                                .Map(m => m.MapLeftKey("StoryId").MapRightKey("TagId").ToTable("StoryTag"));
            
            ToTable("Story");
        }
    }
}
