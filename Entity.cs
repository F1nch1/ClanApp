using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    [Serializable]
    public class Entity
    {
        public string species;
        public float health;
        //Stats
        public StatBlock statBlock;
        public List<CombatMove> combatDeck = new List<CombatMove>();
        public float attackDC { get { return (statBlock.agility * 2) + 5; } }

        
    }
}
