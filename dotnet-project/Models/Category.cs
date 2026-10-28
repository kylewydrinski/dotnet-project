using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models;

[Table("Categories")]
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required, MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    // null = premade category
    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Term> Terms { get; set; } = new List<Term>();
}