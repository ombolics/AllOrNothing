using AllOrNothing.Mapping;
using System.Collections.Generic;

namespace AllOrNothing.Helpers
{
    public class QuestionDtoComparer : IComparer<QuestionDto>
    {
        public int Compare(QuestionDto x, QuestionDto y)
        {
            if (x.Value > y.Value)
                return 1;
            if (x.Value < y.Value)
                return -1;

            return 0;
        }
    }
}
