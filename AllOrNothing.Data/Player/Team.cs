﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Data
{
    public class Team
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public List<Player> Players { get; set; }
        [Required]
        public string TeamName { get; set; }
    }
}
