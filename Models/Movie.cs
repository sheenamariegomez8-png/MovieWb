using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movieweb.Models;

[Table("Movies", Schema = "dbo")]
public sealed class Movie
{
    [Key]
    [Column("MovieID")]
    public int MovieID { get; set; }

    [Required]
    [StringLength(200)]
    [Column("MovieTitle")]
    public string MovieTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Column("Genre")]
    public string Genre { get; set; } = string.Empty;

    [Column("DatePublished", TypeName = "date")]
    public DateOnly DatePublished { get; set; }

    [Required]
    [StringLength(150)]
    [Column("Director")]
    public string Director { get; set; } = string.Empty;

    [Column("Duration")]
    public int Duration { get; set; }

    [Column("Rating", TypeName = "decimal(3,1)")]
    public decimal Rating { get; set; }

    [Required]
    [Column("Synopsis", TypeName = "nvarchar(max)")]
    public string Synopsis { get; set; } = string.Empty;
}
