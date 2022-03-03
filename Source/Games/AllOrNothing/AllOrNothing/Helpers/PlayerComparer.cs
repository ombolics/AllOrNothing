using AllOrNothing.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
