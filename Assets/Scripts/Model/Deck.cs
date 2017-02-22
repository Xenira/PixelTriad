using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    [Serialize]
    public class Deck
    {
        public string name { get; set; }
        public Card[] cards { get; set; }
    }
}
