using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace MovieInfoAPI.Models.Domain
{
    [Table("Franchise")]
    public class Franchise
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FranchiseId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
