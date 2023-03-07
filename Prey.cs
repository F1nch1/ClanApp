using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    public class Prey : Entity
    {
        public Prey(string spec, StatBlock stats, CombatMove[] moves = null)
        {
            species = spec;
            statBlock = stats;
            health = stats.constitution * 5;
            health = Math.Min(health, 15);
            if(moves != null)
            {
                combatDeck = moves.ToList();
            }
            
        }

        static public Dictionary<string, Prey> prey = new Dictionary<string, Prey>()
        {
            {"Mouse", new Prey("Mouse", new StatBlock(0, -3, 0, 0, 0, 1)) },
            {"Toad", new Prey("Toad", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Frog", new Prey("Frog", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Eagle", new Prey("Eagle", new StatBlock(2, 1, 2, 2, 2, 3), new CombatMove[] { new CombatMove("Swoop", 1.5f, Stats.Agility), new CombatMove("Claw", 1, Stats.Strength)}) },
            {"Hawk", new Prey("Hawk",new StatBlock(2, 0, 1, 1, 2, 3), new CombatMove[] { new CombatMove("Swoop", 1.5f, Stats.Agility), new CombatMove("Claw", 1, Stats.Strength)})  },
            {"Falcon", new Prey("Falcon", new StatBlock(3, 1, 1, 1, 1, 3), new CombatMove[] { new CombatMove("Swoop", 1.5f, Stats.Agility), new CombatMove("Claw", 1, Stats.Strength)}) },
            {"Finch", new Prey("Finch", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Thrush", new Prey("Thrush", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Sparrow", new Prey("Sparrow", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Crow", new Prey("Crow", new StatBlock(1, -2, 0, 1, 1, 1)) },
            {"Pigeon", new Prey("Pigeon", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Dove", new Prey("Dove", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Starling", new Prey("Starling", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Magpie", new Prey("Magpie", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Pheasant", new Prey("Pheasant", new StatBlock(0, 0, 1, 1, 0, 2), new CombatMove[] { new CombatMove("Peck", 1.5f, Stats.Finesse), new CombatMove("Claw", 1, Stats.Strength)}) },
            {"Wren", new Prey("Wren", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Moorhen", new Prey("Moorhen", new StatBlock(2, -1, 0, 2, 0, 2)) },
            {"Blackbird", new Prey("Blackbird", new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Chicken", new Prey("Chicken", new StatBlock(0, 0, 1, 1, 1, 3), new CombatMove[] { new CombatMove("Peck", 1.5f, Stats.Finesse), new CombatMove("Claw", 1, Stats.Strength)}) },
            {"Robin", new Prey("Robin", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Wagtail",new Prey("Wagtail",  new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Trout", new Prey("Trout", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Carp", new Prey("Carp", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Minnow", new Prey("Minnow", new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Chub", new Prey("Chub", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Bat", new Prey("Bat", new StatBlock(2, -1, 1, 1, 1, 2), new CombatMove[] { new CombatMove("Swoop", 2, Stats.Agility), new CombatMove("Claw", 1, Stats.Strength), new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Shrew", new Prey("Shrew", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Water Shrew", new Prey("Water Shrew", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Rabbit", new Prey("Rabbit", new StatBlock(3, -2, 0, 1, 0, 2), new CombatMove[] { new CombatMove("Claw", 1, Stats.Strength), new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Hare", new Prey("Hare", new StatBlock(3, -1, 0, 1, 0, 2), new CombatMove[] { new CombatMove("Claw", 1, Stats.Strength), new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Adder", new Prey("Adder", new StatBlock(2, 0, 0, 1, 0, 1), new CombatMove[] { new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Lizard", new Prey("Lizard", new StatBlock(1, -3, 0, 0, 0, 1), new CombatMove[] { new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Squirrel", new Prey("Squirrel", new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Vole", new Prey("Vole", new StatBlock(1, -3, 0, 0, 0, 1)) },
            {"Water Vole", new Prey("Water Vole", new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Wood Mouse", new Prey("Wood Mouse", new StatBlock(1, -3, 0, 0, 0, 2)) },
            {"Rat", new Prey("Rat", new StatBlock(2, -3, 0, 0, 0, 1), new CombatMove[] { new CombatMove("Bite", 1.5f, Stats.Strength)}) },
            {"Beetle", new Prey("Beetle", new StatBlock(1, -4, 0, 0, 0, 1)) }
        };
    }

    public class FreshKill : Item
    {
        public Prey preyValues;
        public string species;
        public float size;

        public FreshKill(Prey preyType)
        {
            preyValues = Prey.prey[preyType.species];
            species = preyType.species;
            size = preyType.statBlock.constitution;
            if(preyType.statBlock.intelligence > 0)
            {
                size *= 2;
            }
        }
    }
}
