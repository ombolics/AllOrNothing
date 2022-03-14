using AllOrNothing.AutoMapper.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Helpers
{
    public class StandingDtoComparer : Comparer<StandingDto>, IComparer
    {
        public StandingDtoComparer()
        {

        }
        public override int Compare(StandingDto x, StandingDto y)
        {
            if (x.Score > y.Score)
                return 1;
            if (x.Score < y.Score)
                return -1;

            return 0;
        }
    }
}
