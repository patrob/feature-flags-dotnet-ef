using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeatureFlagsEfDemo.Features.FeatureFlags.Entities;

[Index(nameof(Name), IsUnique = true)]
[Table("Feature")]
public record FeatureEntity : IEntityTypeConfiguration<FeatureEntity>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }
    
    public virtual FeatureDetailEntity FeatureDetail { get; set; } = null!;

    public void Configure(EntityTypeBuilder<FeatureEntity> entityBuilder)
    {
        entityBuilder
            .HasData(Enum.GetValues<FeatureEnum>()
                .Select(e => new FeatureEntity { Id = (int)e, Name = e.ToString() }));
        entityBuilder
            .OwnsOne(e => e.FeatureDetail)
            .HasData(Enum.GetValues<FeatureEnum>()
                .Select(e => new FeatureDetailEntity { Id = (int)e }));
    }
}