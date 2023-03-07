using System;
using System.Collections.Generic;
using System.Text;

namespace ClanCreatorApp
{
    class PreyEncounter
    {
        

        public PreyEncounter(string preyName, Cat cat, Clan clan)
        {
            Random rand = new Random(Utils.GetRandom());
            Console.WriteLine(cat.name + " has encountered a " + preyName.ToLower() + "!");
            Console.WriteLine(cat.name + " pounces on the " + preyName.ToLower());
            int roll = (int)rand.Next(1, 20);
            Console.WriteLine(cat.name + " rolls a " + roll + " + " + cat.statBlock.finesse + " (finesse)" + " + " + cat.skills.hunting + " (hunting)");
            int preyDc = (int)(Prey.prey[preyName].statBlock.agility * 2) + 10;
            roll += (int)cat.statBlock.finesse;

            if(roll >= preyDc)
            {
                Console.WriteLine(cat.name + " catches the " + preyName.ToLower() + "!");
                Console.WriteLine("You now have one more piece of prey for you and your cats");
                FreshKill preyCaught = new FreshKill(Prey.prey[preyName]);
                clan.freshKillPile.Add(preyCaught);
            }
            else
            {
                Console.WriteLine("Drat! The " + preyName.ToLower() + " got away. Better luck next time.");
            }
        }

        public PreyEncounter(string preyName, Cat cat, Clan clan, Patrol patrol)
        {
            Random rand = new Random(Utils.GetRandom());
            Console.WriteLine(cat.name + " has encountered a " + preyName.ToLower() + "!");
            Console.WriteLine(cat.name + " pounces on the " + preyName.ToLower());
            int roll = (int)rand.Next(1, 20);
            Console.WriteLine(cat.name + " rolls a " + roll + " + " + cat.statBlock.finesse + " (finesse)" + " + " + cat.skills.hunting + " (hunting)");
            int preyDc = (int)(Prey.prey[preyName].statBlock.agility) + 10;
            roll += (int)cat.statBlock.finesse + (int)cat.skills.hunting;

            if (roll >= preyDc)
            {
                Console.WriteLine(cat.name + " catches the " + preyName.ToLower() + "!");
                Console.WriteLine("You now have one more piece of prey for you and your cats");
                FreshKill preyCaught = new FreshKill(Prey.prey[preyName]);
                patrol.itemsHeld.Add(preyCaught);
                cat.skills.hunting += 0.2f;
            }
            else
            {
                Console.WriteLine("Drat! The " + preyName.ToLower() + " got away. Better luck next time.");
            }
        }
    }
}
