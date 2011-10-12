namespace Kigg.Infrastructure.EntityFramework.Mapping
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            HasKey(u => u.Id);
            Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(u => u.UserName).IsUnicode().HasMaxLength(256).IsRequired();
            Property(u => u.Password).IsUnicode().HasMaxLength(64).IsOptional();
            Property(u => u.DisplayName).IsUnicode().HasMaxLength(256).IsOptional();
            Property(u => u.About).IsUnicode().IsMaxLength().IsOptional();
            Property(u => u.Website).IsUnicode().HasMaxLength(256).IsOptional();
            Property(u => u.Email).IsUnicode().HasMaxLength(256).IsRequired();
            Property(u => u.IsActive).IsRequired();
            Property(u => u.IsLockedOut).IsRequired();
            Property(u => u.LastActivityAt).IsRequired();
            Property(u => u.CreatedAt).IsRequired();
            Property(u => u.Role).HasColumnType("int").IsRequired();
            HasMany(u => u.Tags).WithMany().Map(m => m.MapLeftKey("UserId").MapRightKey("TagId").ToTable("UserTag"));

            Ignore(u => u.CurrentScore);
            
            ToTable("User");
        }
    }
}
