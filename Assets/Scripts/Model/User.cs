using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class User
    {
        public string login { get; set; }
        public string name { get; set; }

        public int exp { get; set; }
        public int gold { get; set; }
        public int tickets { get; set; }

        public List<Deck> decks { get; set; }
    }
}
