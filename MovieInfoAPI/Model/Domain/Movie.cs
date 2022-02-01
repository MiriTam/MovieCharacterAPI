using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MovieCharacters.Model.Domain
{
    [Table("Movie")]
    public class Movie
    {
        public int Id { get; set; }
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
        public ICollection<Character> characters { get; set; }
        public int FranchiseId { get; set; }
    }
}
