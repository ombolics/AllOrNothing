using System;

namespace AllOrNothing.Helpers
{
    internal static class TimeSpanHelper
    {
        public static TimeSpan ShiftToRight(this TimeSpan t)
        {
            return new TimeSpan(0, t.Hours, t.Minutes);
        }
    }
}
