using Assets.Scripts.Util;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [Serialize]
    public class Deck
    {
        public string name { get; set; }
        public List<Card> cards { get; set; }
    }
}
