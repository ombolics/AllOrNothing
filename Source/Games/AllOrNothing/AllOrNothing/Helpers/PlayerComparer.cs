using AllOrNothing.Data;
using System.Collections;
using System.Collections.Generic;

namespace AllOrNothing.Helpers
{
    class PlayerComparer : Comparer<Player>, IComparer
    {
        public override int Compare(Player x, Player y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
