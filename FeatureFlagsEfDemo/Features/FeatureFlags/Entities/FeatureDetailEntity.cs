using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeatureFlagsEfDemo.Features.FeatureFlags.Entities;

[Table("FeatureDetail")]
public sealed record FeatureDetailEntity
{
    [Key]
    public int Id { get; set; }

    public bool IsEnabled { get; set; } = false;
    
    [ForeignKey(nameof(Id))]
    public FeatureEntity Feature { get; set; } = null!;
}