﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Input;

namespace AllOrNothing.Data
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Institue { get; set; }
        public string NickName { get; set; }
        //public ICommand RemoveCommand { get; set; }
        //public ICommand TestCommand { get; set; }
        //public override string ToString()
        //{
        //    return Name;
        //}
    }
}