using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPMLab.Turboreactors.Models
{
    public class TurboreactorType
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Turboreactor> Turboreactors { get; set; }
    }
}