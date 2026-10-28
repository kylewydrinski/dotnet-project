using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Models;

[Table("Terms")]
public class Term
{
    [Key]
    public int TermId { get; set; }

    [Required, MaxLength(200)]
    public string TermName { get; set; } = string.Empty;

    [Required]
    public string TermDescription { get; set; } = string.Empty;

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // null = premade term
    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<StudyGuideTerm> StudyGuideTerms { get; set; } = new List<StudyGuideTerm>();
}