using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MovieCharacters.Model.Domain
{
    [Table("Franchise")]
    public class Franchise
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
