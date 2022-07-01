using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPMLab.Turboreactors.Models
{
    public class Turboreactor
    {
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int Power { get; set; }
        [Display(Name = "Blades Count")]
        public int BladesCount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        public int ManufactureID { get; set; }
        public virtual Manufacture Manufacture { get; set; }
        private ICollection<TurboreactorType> _types;
        public virtual ICollection<TurboreactorType> Types
        {
            get
            {
                return _types ?? (_types = new List<TurboreactorType>());
            }
            set
            {
                _types = value;
            }
        }
    }
}