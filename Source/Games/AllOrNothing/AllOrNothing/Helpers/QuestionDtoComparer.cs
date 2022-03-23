using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
