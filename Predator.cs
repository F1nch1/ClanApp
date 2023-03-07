using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    class Predator : Entity
    {
        public Predator(string spec, StatBlock stats, CombatMove[] moves = null)
        {
            species = spec;
            statBlock = stats;
            health = stats.constitution * 5;
            health = Math.Min(health, 15);
            if (moves != null)
            {
                combatDeck = moves.ToList();
            }

        }
    }
}
