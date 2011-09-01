namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class KnownSourceMapping : EntityTypeConfiguration<KnownSource>
    {
        public KnownSourceMapping()
        {
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.CreatedAt).IsRequired();
            Property(s => s.Url).IsRequired().IsUnicode().HasMaxLength(450);
            this.Property<KnownSource, int>("GradeInternal").HasColumnName("Grade").IsRequired();

            ToTable("KnownSource");
        }
    }
}
