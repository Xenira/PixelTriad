using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Util
{
    class ResettingUint
    {
        ushort c = default(ushort);

        public ushort GetNext()
        {
            if (c < ushort.MaxValue)
            {
                c++;
            } else
            {
                c = ushort.MinValue;
            }

            return c;
        }
    }
}
