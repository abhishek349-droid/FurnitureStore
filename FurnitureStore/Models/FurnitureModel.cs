using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class FurnitureModel
    {
        [Key]
        public int FurnitureId { get; set; }

        [Required]
        [MaxLength(30)]
        public string FurnitureName { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public int? Quantity { get; set; }
    }
}
