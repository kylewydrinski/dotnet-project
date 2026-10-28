using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models;

[Table("StudyGuides")]
public class StudyGuide
{
    [Key]
    public int StudyGuideId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    // null = premade study guide
    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<StudyGuideTerm> StudyGuideTerms { get; set; } = new List<StudyGuideTerm>();
}