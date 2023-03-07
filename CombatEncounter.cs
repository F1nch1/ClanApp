using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    [Serializable]
    public class CombatMove
    {
        public string name;
        public float damageMultiplier;
        public Stats stat;

        public CombatMove(string n, float mult, Stats s)
        {
            name = n;
            damageMultiplier = mult;
            stat = s;
        }


    }
    class CombatEncounter
    {
        Cat[] playerCats;
        Entity[] enemies;
        Clan clan;
        
        bool showEncounterText;

        public CombatEncounter(Cat[] cats, Entity[] ens, Clan c, bool show = true, Patrol patrol = new Patrol())
        {
            playerCats = cats;
            enemies = ens;
            clan = c;
            showEncounterText = show;
        }
        public void CombatLoop(Patrol patrol)
        {
            while(CheckCombatOver())
            {
                CombatRound(patrol);
            }
        }
        
        public bool CombatRound(Patrol patrol)
        {

            
            if(showEncounterText)
            {
                Console.Clear();
                Console.WriteLine(CreateEncounterText());
            }
            //Console.WriteLine("Combat round");
            PlayerCombatRound(patrol);
            EnemyCombatRound(patrol);
            return true;
        }

        public void EnemyCombatRound(Patrol patrol)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].health > 0 && playerCats.Length > 0)
                {
                    EnemyCombat(enemies[i], patrol);
                    MainFile.PressEToContinue();
                    Console.Clear();
                }
            }
        }

        public void EnemyCombat(Entity enemy, Patrol patrol)
        {
            Random rand = new Random();
            if (enemy.health <= enemy.statBlock.constitution)
            {
                if((int)rand.Next(1, 2) == 1)
                {
                    int roll = (int)rand.Next(1, 20);
                    if (roll + enemy.statBlock.agility > 10)
                    {
                        if (enemy is Cat)
                        {
                            Cat enemyCat = (Cat)enemy;
                            Console.WriteLine(enemyCat.name + " has fled succesfully!");
                        }
                        else
                        {
                            Console.WriteLine("The " + enemy.species.ToLower() + " has fled succesfully!");
                        }
                        enemies = RemoveEntityFromCombat(enemy, patrol);

                    }
                    else
                    {
                        if (enemy is Cat)
                        {
                            Cat enemyCat = (Cat)enemy;
                            Console.WriteLine(enemyCat.name + " has failed to flee.");
                        }
                        else
                        {
                            Console.WriteLine("The " + enemy.species.ToLower() + " has failed to flee.");
                        }
                    }
                }
                else
                {
                    EntityAttack(enemy, patrol);
                }
            }
            else
            {
                EntityAttack(enemy, patrol);
            }
        }
        public void EntityAttack(Entity enemy, Patrol patrol)
        {
            Random rand = new Random();
            //Pick cat to attack
            int catChoice = rand.Next(0, playerCats.Length);
            Cat catToAttack = playerCats[catChoice];
            //Pick attack move to use
            int moveChoice = rand.Next(0, enemy.combatDeck.Count);
            CombatMove moveToUse = enemy.combatDeck[moveChoice];

            int roll = (int)((int)rand.Next(1, 20));
            if (enemy is Cat)
            {
                Cat enemyCat = (Cat)enemy;
                Console.WriteLine(enemyCat.name + " rolls a " + roll + " + " + Utils.GetStats(enemy, moveToUse.stat) + " (" + StatString(moveToUse.stat) + ")\n");
            }
            else
            {
                Console.WriteLine("The " + enemy.species.ToLower() + " rolls a " + roll + " + " + Utils.GetStats(enemy, moveToUse.stat) + " (" + StatString(moveToUse.stat) + ")\n");
            }
            
            if (roll >= catToAttack.attackDC)
            {
                if (enemy is Cat)
                {
                    Cat enemyCat = (Cat)enemy;
                    Console.WriteLine(enemyCat.name + " uses " + moveToUse.name + " on " + catToAttack.name + ".\n");
                    float damage = roll - catToAttack.attackDC;
                    damage = Math.Min(damage, 3);
                    damage = Math.Max(1, damage);
                    damage *= moveToUse.damageMultiplier;
                    catToAttack.health -= damage;
                    Console.WriteLine(enemyCat.name + " deals " + damage + " damage to " + catToAttack.name + ". " + catToAttack.name + " now has " + catToAttack.health + " health left.\n");
                    catToAttack = WoundCat(catToAttack, damage);
                }
                else
                {
                    Console.WriteLine("The " + enemy.species.ToLower() + " uses " + moveToUse.name + " on " + catToAttack.name + ".\n");
                    float damage = roll - catToAttack.attackDC;
                    damage = Math.Min(damage, 3);
                    damage = Math.Max(1, damage);
                    damage *= moveToUse.damageMultiplier;
                    catToAttack.health -= damage;
                    Console.WriteLine("The " + enemy.species.ToLower() + " deals " + damage + " damage to " + catToAttack.name + ". " + catToAttack.name + " now has " + catToAttack.health + " health left.\n");
                    catToAttack = WoundCat(catToAttack, damage);
                }
                if(catToAttack.health <= 0)
                {
                    Console.WriteLine(catToAttack.name + " has died of their wounds.\n");
                    playerCats = RemoveCatFromCombat(catToAttack, playerCats, true, patrol);
                }
            }
            else
            {
                Console.WriteLine("It's a miss!\n");
            }
            
        }

        public Entity[] RemoveEntityFromCombat(Entity entityToRemove, Patrol patrol)
        {
            List<Entity> entityList = enemies.ToList();
            for(int i = 0; i < entityList.Count; i++)
            {
                
            }
            entityList.Remove(entityToRemove);
            return entityList.ToArray();
        }

        public Cat WoundCat(Cat cat, float damage)
        {
            Wound? w = Wounds.WoundCheck(damage);

            string woundOutput = "";
            if (w != null)
            {
                Wound wound = (Wound)w;
                cat.wounds.Add(wound);
                woundOutput += (cat.name + " has received ");
                if (Utils.CheckVowel(wound.type[0]))
                {
                    woundOutput += "an ";
                }
                else
                {
                    woundOutput += "a ";
                }
                woundOutput += wound.type + " on their " + wound.location;
                Console.WriteLine(woundOutput);
            }
            return cat;
        }

        public bool CheckCombatOver()
        {
            if (enemies.Length == 0)
            {
                Console.WriteLine("All enemies have perished or fled. Your clan was victorious!\n");
                return false;
            }
            else if (playerCats.Length == 0)
            {
                Console.WriteLine("Your combat party was deafeated.\n");
                return false;
            }
            else { return true; }
        }

        public void PlayerCombatRound(Patrol patrol)
        {
            
            for (int i = 0; i < playerCats.Length; i++)
            {
                if(playerCats[i].health > 0 && enemies.Length > 0)
                {
                    PlayerCombat(playerCats[i], patrol, showEncounterText);
                }
            }
        }

        public void PlayerCombat(Cat playerCat, Patrol patrol, bool showText)
        {
            
            Console.WriteLine("What will " + playerCat.name + " do?\n");
            Console.WriteLine("[0]: Attack   [1]: Skip cat   [2]: Flee\n");
            string checkCombat = "";
            while (!checkCombat.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkCombat) || int.Parse(checkCombat) > 2)
            {
                checkCombat = Console.ReadLine();
            }
            int combatNumber = int.Parse(checkCombat);
            Console.Clear();
            switch (combatNumber)
            {
                case 0:
                    Attack(playerCat, patrol);
                    break;
                case 1:
                    break;
                case 2:
                    playerCats = TryFlee(playerCat, playerCats, patrol);
                    break;
            }
        }
        public Cat[] TryFlee(Cat playerCat, Cat[] playerCats, Patrol patrol)
        {
            Random rand = new Random();
            int roll = (int)rand.Next(1, 20);
            if(roll + playerCat.statBlock.agility > 10)
            {
                Console.WriteLine(playerCat.name + " has fled succesfully!\n");
                MainFile.PressEToContinue();
                Console.Clear();
                return RemoveCatFromCombat(playerCat, playerCats, false, patrol);
            }
            else
            {
                Console.WriteLine(playerCat.name + " failed to flee!\n");
                MainFile.PressEToContinue();
                Console.Clear();
                return playerCats;
            }
        }

        public Cat[] RemoveCatFromCombat(Cat catToRemove, Cat[] playerCats, bool death, Patrol patrol)
        {
            if(death)
            {
                clan.clanCats.Remove(catToRemove);
                patrol.patrolCats.Remove(catToRemove);
            }
            
            List<Cat> catList = playerCats.ToList();
            catList.Remove(catToRemove);
            return catList.ToArray();
        }

        public void Attack(Cat playerCat, Patrol patrol)
        {
            Entity enemy = enemies[AttackEnemyOptions(enemies)]; Console.Clear();
            CombatMove move = AttackMovesOption(playerCat);
            Console.Clear();
            string attackDisplay = "";
            attackDisplay += playerCat.name + " uses " + move.name + " on ";
            if(enemy is Cat)
            {
                Cat enemyCat = (Cat)enemy;
                attackDisplay += enemyCat.name;
            }
            else
            {
                attackDisplay += "the " + enemy.species.ToLower();
            }
            attackDisplay += "!\n";
            Console.WriteLine(attackDisplay);

            Random rand = new Random();
            float roll = (int)rand.Next(1, 20);
            Console.WriteLine(playerCat.name + " rolls a " + roll + " + " + Utils.GetStats(playerCat, move.stat) + " (" + StatString(move.stat)+ ")\n");
            roll += Utils.GetStats(playerCat, move.stat);
            if(roll >= enemy.attackDC)
            {
                float damage = roll - enemy.attackDC;
                damage = Math.Min(damage, 3);
                damage *= move.damageMultiplier;
                string combatResult = playerCat.name + " deals " + damage + " damage to ";
                enemy.health -= damage;
                if (enemy is Cat)
                {
                    Cat enemyCat = (Cat)enemy;
                    combatResult += enemyCat.name;
                    combatResult += "\n" + enemyCat.name + " now has " + enemy.health.ToString() + " health.\n";
                }
                else
                {
                    combatResult += "the " + enemy.species.ToLower();
                    combatResult += "\nThe " + enemy.species.ToLower() + " now has " + enemy.health.ToString() + " health.\n";
                }
                Console.WriteLine(combatResult);
                MainFile.PressEToContinue();
                Console.Clear();
                if(enemy.health <= 0)
                {
                    string deathResult = "";
                    if (enemy is Cat)
                    {
                        Cat enemyCat = (Cat)enemy;
                        deathResult += enemyCat.name + " has died of their wounds.\n";
                    }
                    else
                    {
                        deathResult += "The " + enemy.species + " has died of its wounds.\n";
                    }
                    Console.WriteLine(deathResult);

                    enemies = RemoveEntityFromCombat(enemy, patrol);
                    if (enemy is Prey)
                    {
                        patrol.itemsHeld.Add(new FreshKill(Prey.prey[enemy.species]));
                    }
                    MainFile.PressEToContinue();
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("It's a miss!\n");
                MainFile.PressEToContinue();
                Console.Clear();
            }
        }

        public CombatMove AttackMovesOption(Cat cat)
        {
            Console.WriteLine("Choose an attack move to use:\n");
            string attackOptions = "";
            for(int i = 0; i < cat.combatDeck.Count; i++)
            {
                attackOptions += "[" + i + "]: ";
                attackOptions += cat.combatDeck[i].name;
                attackOptions += "\n";
            }
            Console.WriteLine(attackOptions);
            string optionsNumber = "";
            int options = -1;
            while (!optionsNumber.All(Char.IsDigit) || string.IsNullOrWhiteSpace(optionsNumber) || int.Parse(optionsNumber) > cat.combatDeck.Count - 1)
            {
                optionsNumber = Console.ReadLine();
                
            }
            options = int.Parse(optionsNumber);
            return cat.combatDeck[options];
        }

        public int AttackEnemyOptions(Entity[] enemies)
        {
            Console.WriteLine("Choose a target to attack:\n");
            string enemyOptions = "";
            for(int i = 0; i < enemies.Length; i++)
            {
                enemyOptions += "[" + i + "]: ";
                if(enemies[i] is Cat)
                {
                    Cat cat = (Cat)enemies[i];
                    enemyOptions += cat.name;
                }
                else
                {
                    enemyOptions += enemies[i].species;
                }
                enemyOptions += "\n";
            }
            Console.WriteLine(enemyOptions);
            string enemyNumber = "";
            int enemy = -1;
            while (!enemyNumber.All(Char.IsDigit) || string.IsNullOrWhiteSpace(enemyNumber) || int.Parse(enemyNumber) > enemies.Length - 1)
            {
                enemyNumber = Console.ReadLine();
                
            }
            enemy = int.Parse(enemyNumber);
            return enemy;
        }

        public string CreateEncounterText()
        {
            string result = "";
            for(int i = 0; i < playerCats.Length; i++)
            {
                if(playerCats[i].health > 0)
                {
                    if (i != playerCats.Length - 1 && playerCats.Length != 1 && playerCats.Length != 2)
                    {
                        result += playerCats[i].name + ", ";
                    }
                    else if (i != playerCats.Length - 1 && playerCats.Length == 2)
                    {
                        result += playerCats[i].name + " ";
                    }
                    else if (playerCats.Length == 1)
                    {
                        result += playerCats[i].name;
                    }
                    else
                    {
                        result += "and " + playerCats[i].name;
                    }
                }

            }

            if(playerCats.Length > 1)
            {
                result += " encounter ";
            }
            else
            {
                result += " encounters ";
            }
            
            for(int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] is Cat)
                {
                    Cat enemyCat = (Cat)enemies[i];
                    if (i != enemies.Length - 1 && enemies.Length != 1 && enemies.Length != 2)
                    {
                        result += enemyCat.name + ", ";
                    }
                    else if(i != enemies.Length - 1 && enemies.Length == 2)
                    {
                        result += enemyCat.name + " ";
                    }
                    else if (i != enemies.Length - 1 && enemies.Length == 1)
                    {
                        result += enemyCat.name;
                    }
                    else
                    {
                        result += "and " + enemyCat.name;
                    }
                }
                else if(!Utils.CheckVowel(enemies[i].species[0]))
                {
                    if (i != enemies.Length - 1 && enemies.Length != 1 && enemies.Length != 2)
                    {
                        result += "a " + enemies[i].species.ToLower() + ", ";
                    }
                    else if (i != enemies.Length - 1 && enemies.Length == 2)
                    {
                        result += "a " + enemies[i].species.ToLower() + " ";
                    }
                    else if (enemies.Length == 1)
                    {
                        result += "a " + enemies[i].species.ToLower();
                    }
                    else
                    {
                        result += "and a " + enemies[i].species.ToLower();
                    }
                }
                else
                {
                    if (i != enemies.Length - 1 && enemies.Length != 1 && enemies.Length != 2)
                    {
                        result += "an " + enemies[i].species.ToLower() + ", ";
                    }
                    else if (i != enemies.Length - 1 && enemies.Length == 2)
                    {
                        result += "an " + enemies[i].species.ToLower() + " ";
                    }
                    else if (enemies.Length == 1)
                    {
                        result += "an " + enemies[i].species.ToLower();
                    }
                    else
                    {
                        result += "and an " + enemies[i].species.ToLower();
                    }
                }
            }
            result += ".\n";
            return result;
        }

        

        public string StatString(Stats s)
        {
            switch (s)
            {
                case Stats.Agility:
                    return "agility";
                case Stats.Constitution:
                    return "constitution";
                case Stats.Finesse:
                    return "finesse";
                case Stats.Intelligence:
                    return "intelligence";
                case Stats.Strength:
                    return "strength";
                case Stats.Tenacity:
                    return "tenacity";
            }
            return "";
        }
    }
}
