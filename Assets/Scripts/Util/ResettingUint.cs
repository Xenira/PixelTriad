using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Util
{
    class CyclingByte
    {
        byte c = default(byte);

        public byte GetNext()
        {
            if (c < byte.MaxValue)
            {
                c++;
            }
            else
            {
                c = byte.MinValue;
            }

            return c;
        }
    }
}
