using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class CategorieDbModelConfiguration : IEntityTypeConfiguration<CategorieDbModel>
    {
        public void Configure(EntityTypeBuilder<CategorieDbModel> entity)
        {
            entity.ToTable("Categorie");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.IsEditavel).HasDefaultValue(false);
  
        }
    }
}
