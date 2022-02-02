using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace MovieInfoAPI.Models.Domain
{
    [Table("Movie")]
    public class Movie
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MovieId { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string Genre { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required, MaxLength(50)]
        public string Director { get; set; }
        public string Picture { get; set; }
        public string Trailer { get; set; }
        public ICollection<Character> Characters { get; set; }
        public Franchise Franchise { get; set; }
    }
}
