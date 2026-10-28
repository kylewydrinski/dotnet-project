using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models;

[Table("StudyGuideTerms")]
public class StudyGuideTerm
{
    // Composite primary key (StudyGuideId, TermId) is configured in OnModelCreating
    public int StudyGuideId { get; set; }
    public StudyGuide StudyGuide { get; set; } = null!;

    public int TermId { get; set; }
    public Term Term { get; set; } = null!;

    public int? SortOrder { get; set; }
}