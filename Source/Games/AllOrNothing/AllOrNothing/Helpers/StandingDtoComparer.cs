using AllOrNothing.Mapping;
using System.Collections;
using System.Collections.Generic;

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
