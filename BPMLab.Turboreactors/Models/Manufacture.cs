using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPMLab.Turboreactors.Models
{
    public class Manufacture
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string OfficeBuildingColor { get; set; }

        public StoredFile LogoImage { get; set; }
        public ICollection<Turboreactor> Turboreactors { get; set; }
    }
}