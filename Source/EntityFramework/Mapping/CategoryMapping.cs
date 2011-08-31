namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;
    
    using DomainObjects;

    public class CategoryMapping : EntityTypeConfiguration<Category>
    {
        public CategoryMapping()
        {
            HasKey(c => c.Id);
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.CreatedAt).IsRequired();
            Property(c => c.Name).IsRequired().IsUnicode().HasMaxLength(64);
            Property(c => c.UniqueName).IsRequired().IsUnicode().HasMaxLength(64);

            ToTable("Category");
        }
    }
}
