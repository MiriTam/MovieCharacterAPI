using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MovieCharacters.Model.Domain
{
    public enum Gender
    {
        Female,
        Male
    }

    [Table("Character")]
    public class Character
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Alias { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string URL { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
