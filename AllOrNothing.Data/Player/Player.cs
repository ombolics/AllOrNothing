using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Player
    {
        [Key]
        public int Id { get; }
        [Required]
        public string Name { get; set; }
        public string Institue { get; set; }
        public List<string> NickNames { get; set; }
    }
}