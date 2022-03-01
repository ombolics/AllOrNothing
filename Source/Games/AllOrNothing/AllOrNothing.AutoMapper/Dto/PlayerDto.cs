using AllOrNothing.Data;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Player),ReverseMap = true)]
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institue { get; set; }
        public string NickName { get; set; }
        [Ignore]
        public ICommand RemoveCommand { get; set; }
        [Ignore]
        public ICommand TestCommand { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
