using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class User {
        public string id;
        public string name;
        public string email;
        public string picture;
        public int elo;
        public Deck[] decks = new Deck[5];
    }
}
