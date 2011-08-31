namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class TagMapping : EntityTypeConfiguration<Tag>
    {
        public TagMapping()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.CreatedAt).IsRequired();
            Property(t => t.Name).IsRequired().IsUnicode().HasMaxLength(64);
            Property(t => t.UniqueName).IsRequired().IsUnicode().HasMaxLength(64);
            
            ToTable("Tag");
        }
    }
}
