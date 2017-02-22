using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    [Serialize]
    public class Card
    {
        public string name { get; set; }
        public int rating { get; set; }
        public int dmgUp { get; set; }
        public int dmgDown { get; set; }
        public int dmgLeft { get; set; }
        public int dmgRight { get; set; }
    }
}
