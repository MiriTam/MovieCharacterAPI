using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace MovieInfoAPI.Models.Domain
{
    [Table("Character")]
    public class Character
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Alias { get; set; }
        [Required, MaxLength(6)]
        public string Gender { get; set; }
        public string URL { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
