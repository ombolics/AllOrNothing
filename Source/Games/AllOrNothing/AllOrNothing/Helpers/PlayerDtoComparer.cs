using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Data;
using System.Collections;
using System.Collections.Generic;

namespace AllOrNothing.Helpers
{
    class PlayerDtoComparer : Comparer<PlayerDto>, IComparer
    {
        public override int Compare(PlayerDto x, PlayerDto y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
