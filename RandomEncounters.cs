using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    class RandomEncounters
    {
        static public Dictionary<string, Predator> predators = new Dictionary<string, Predator>()
        {
            {"Dog", new Predator("Dog", new StatBlock(2,5,2,1,1,5), new CombatMove[] { new CombatMove("Chomp", 3f, Stats.Strength), new CombatMove("Claw", 1, Stats.Strength)}) },
            {"Fox", new Predator("Fox", new StatBlock(5, 2, 3, 4, 1, 3), new CombatMove[] { new CombatMove("Swipe", 1.5f, Stats.Finesse), new CombatMove("Claw", 1, Stats.Strength), new CombatMove("Dash", 1f, Stats.Agility)}) },
            {"Stoat", new Predator("Stoat", new StatBlock(3,1,1,2,1,2), new CombatMove[] { new CombatMove("Swipe", 1.5f, Stats.Finesse), new CombatMove("Cling", 1, Stats.Tenacity), new CombatMove("Dash", 1f, Stats.Agility)}) },
            {"Snake", new Predator("Snake", new StatBlock(3,1,1,3,1,2), new CombatMove[] { new CombatMove("Bite", 1.5f, Stats.Strength), new CombatMove("Constrict", 1.5f, Stats.Strength)}) },
        };

        //Other cats:
        //Rogues
        //Loners
        //Lost kit

        //Rogues: You find a group of rogues. They demand an item from you. If you refuse, a fight occurs
        //(Item system, saving items, saving patrols)

        //Loners: A loner is on your territory, hunting. You may invite them to join, but it will take some convincing.
        //(Territory system, persuasion system)

        //Lost Kit: You hear the wailing of a lost kit. You can try to find their mother, but only have a small chance of doing so. If a mother is not assigned to the kit, it will likely die
        //(Kit/Mother system)


        public void RandomEncounter(Cat[] catsInvolved, Clan clan, Patrol patrol) 
        {
            Random rand = new Random(Utils.GetRandom());
            int maxEncounterTypes = 13;
            int randomChance = rand.Next(0, 4);
            int num;
            if(randomChance == 0)
            {

                num = rand.Next(0, maxEncounterTypes + 1);
                switch (num)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine("You've encountered a twoleg's dog, loose and on the run! A leash waves in the air as it runs.\n");
                        Entity[] dog = new Entity[] { predators["Dog"] };
                        CombatEncounter dogEncounter = new CombatEncounter(catsInvolved, dog, clan, false);
                        dogEncounter.CombatLoop(patrol);
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("A snarling fox stalks from the undergrowth. You must be too close to its territory!\n");
                        Entity[] fox = new Entity[] { predators["Fox"] };
                        CombatEncounter foxEncounter = new CombatEncounter(catsInvolved, fox, clan, false);
                        foxEncounter.CombatLoop(patrol);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Out of nowhere, a writhing stoat leaps onto " + catsInvolved[0].name + "'s back!\n");
                        Entity[] stoat = new Entity[] { predators["Stoat"] };
                        CombatEncounter stoatEncounter = new CombatEncounter(catsInvolved, stoat, clan, false);
                        stoatEncounter.CombatLoop(patrol);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine(catsInvolved[0].name + " notices a hissing sound. From nowhere, a snake lunges!\n");
                        Entity[] snake = new Entity[] { predators["Snake"] };
                        CombatEncounter snakeEncounter = new CombatEncounter(catsInvolved, snake, clan, false);
                        snakeEncounter.CombatLoop(patrol);
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("Trundling through the undergrowth, making enough noise to scare off every bird in the sky, a fat kittypet appears.\nA bell jingles from its collar.");
                        Persuasion per = new Persuasion();
                        Cat cat = CreateRandomCats(1, StatBlocks.housePet)[0];
                        Cat persuader = catsInvolved.FirstOrDefault(x => (x.rankAccesor == Defs.Rank.LonerLeader) || (x.rankAccesor == Defs.Rank.Leader));
                        if (per.PersuadeCat(cat, persuader, clan, MainFile.interactionData))
                        {
                            Cat[] cats = { cat };
                            AddAllCats(cats, patrol, clan);
                        }
                        
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("In the distance, you hear laughter, the source of which becomes immediately apparent. \nA gaggle of kittypets crashes through the undergrowth before you.");
                        Persuasion per2 = new Persuasion();
                        Random rand2 = new Random();
                        int num3 = rand2.Next(1, 4);
                        Cat[] cats2 = CreateRandomCats(num3, StatBlocks.housePet);
                        Cat persuader2 = catsInvolved.FirstOrDefault(x => (x.rankAccesor == Defs.Rank.LonerLeader) || (x.rankAccesor == Defs.Rank.Leader));
                        Console.WriteLine("From the group, one of the cats approaches. They must be their leader.");
                        if(per2.PersuadeCat(cats2[0], persuader2, clan, MainFile.interactionData))
                        {
                            AddAllCats(cats2, patrol, clan); 
                        }
                        
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine(catsInvolved[0].name + " jumps as another cat appears at their back. The strange cat gives a sly smile. \nThey look like a loner, or maybe a barn cat?");
                        Persuasion per3 = new Persuasion();
                        Cat cat3 = CreateRandomCats(1, StatBlocks.loner)[0];
                        Cat persuader3 = catsInvolved.FirstOrDefault(x => (x.rankAccesor == Defs.Rank.LonerLeader) || (x.rankAccesor == Defs.Rank.Leader));
                        if (per3.PersuadeCat(cat3, persuader3, clan, MainFile.interactionData))
                        {
                            Cat[] cats3 = { cat3 };
                            AddAllCats(cats3, patrol, clan);
                        }
                        break;
                    case 7:
                        Console.Clear();
                        Random rand3 = new Random(Utils.GetRandom());
                        int num4 = rand3.Next(0, StatBlocks.origins.Length);
                        Cat cat4 = CreateRandomCats(1, StatBlocks.origins[num4])[0];
                        Wound w = Wounds.CreateWound(10);
                        cat4.wounds.Add(w);
                        Console.WriteLine("From the undergrowth, " + catsInvolved[0].name + " hears a groaning noise. Following the smell of sickness, you find a cat with a " + w.type + " on \ntheir " + w.location + ". They look too injured to walk.");
                        Console.WriteLine("[0]: Offer to help the stranger\n[1]: Leave it");
                        string check = "";
                        while (!check.All(Char.IsDigit) || string.IsNullOrWhiteSpace(check) || int.Parse(check) > 1)
                        {
                            check = Console.ReadLine();
                        }
                        int checkNum = int.Parse(check);
                        if(checkNum == 0)
                        {
                            Console.Clear();
                            Console.WriteLine(cat4.name + " has joined your clan!");
                            clan.clanCats.Add(cat4);
                            MainFile.PressEToContinue();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You leave the stranger to their fate.");
                            MainFile.PressEToContinue();
                        }
                        break;
                    case 8:
                        Console.Clear();
                        Console.WriteLine("You see an odd item in the undergrowth. The twolegs must've left it behind.\nIt does look rather tasty...");
                        Console.WriteLine("[0]: Try to eat it\n[1]: Leave it be");
                        string checkGarbage = "";
                        while (!checkGarbage.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkGarbage) || int.Parse(checkGarbage) > 1)
                        {
                            checkGarbage = Console.ReadLine();
                        }
                        int checkGarbageNumber = int.Parse(checkGarbage);
                        switch(checkGarbageNumber)
                        {
                            case 0:
                                int garNum = rand.Next(0, 2);
                                if(garNum == 0)
                                {
                                    Console.WriteLine("You don't feel so well.");
                                    int diseaseRand = rand.Next(0, Diseases.randomDiseases.Length);
                                    catsInvolved.FirstOrDefault(x => (x.rankAccesor == Defs.Rank.LonerLeader) || (x.rankAccesor == Defs.Rank.Leader)).diseases.Add(new Disease(4, Diseases.randomDiseases[diseaseRand].type));
                                }
                                else
                                {
                                    Console.WriteLine("Tasty! Your patrol gains some energy.");
                                    patrol = AddEnergy(patrol);
                                }
                                MainFile.PressEToContinue();
                                break;
                            

                        }
                        break;
                    case 9:
                        Console.Clear();
                        Console.WriteLine("The undergrowth shakes. From within, a group of plump, well-groomed kittypets exits.\nTheir leader approaches. To your surprise, they say that they have been looking for you,\nand that they would like to learn how to hunt!");
                        string checkHuntHelp = "";
                        Console.WriteLine("[0]: Spend some time teaching them.\n[1]: Tell them to leave your territory.");
                        while (!checkHuntHelp.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkHuntHelp) || int.Parse(checkHuntHelp) > 1)
                        {
                            checkHuntHelp = Console.ReadLine();
                        }
                        int checkHuntHelpNumber = int.Parse(checkHuntHelp);
                        switch(checkHuntHelpNumber)
                        {
                            case 0:
                                Console.WriteLine("The kittypets gratefully accept your lessons. They aren't... Great, but they have a good time!\nOnce they're all two exhausted to keep going, one of the kittypets approaches you.\nThey have a gift!");
                                int randHerbNum = rand.Next(0, Herbs.herbs.Length);
                                Herb randHerb = Herbs.herbs[randHerbNum];
                                Console.WriteLine("Your patrol has been given " + randHerb.type + "!"); MainFile.PressEToContinue();
                                patrol.itemsHeld.Add(randHerb);
                                break;
                            case 1:
                                Console.WriteLine("The kittypets run off with their tails between their legs."); MainFile.PressEToContinue();
                                break;
                            
                        }
                        break;
                    case 10:
                        Console.Clear();
                        Console.WriteLine("In the distance, you hear an awful, ear-shattering rumble. Before you have time to flee,\nthe stench of twoleg filth fills the air. A horrible contraption bursts into view, coming straight towards you!");
                        for(int i = 0; i < patrol.patrolCats.Count; i++)
                        {
                            float roll = rand.Next(0, 21);
                            roll += patrol.patrolCats[i].statBlock.agility;
                            if(roll >= 10)
                            {
                                Console.WriteLine(patrol.patrolCats[i].name + " manages to dodge out of the way.");
                            }
                            else
                            {
                                int injuryRoll = rand.Next(0, 5);
                                if(injuryRoll < 4)
                                {
                                    int randomInjury = rand.Next(0, Wounds.woundTypes.Length);
                                    Wound w2 = Wounds.CreateWound(8, Wounds.woundTypes[randomInjury]);
                                    patrol.patrolCats[i].wounds.Add(w2);
                                    Console.WriteLine(patrol.patrolCats[i].name + " has been wounded."); MainFile.PressEToContinue();
                                }
                                else
                                {
                                    patrol.patrolCats[i].health = 0;
                                    Console.WriteLine(patrol.patrolCats[i].name + " has lost their life beneath the wheels of the monster."); MainFile.PressEToContinue();
                                    clan.clanCats.Remove(patrol.patrolCats[i]);
                                    patrol.patrolCats.Remove(patrol.patrolCats[i]);
                                    
                                    
                                }
                            }
                            
                        }
                        break;
                    case 11:
                        Console.Clear();
                        int randCat = rand.Next(0, patrol.patrolCats.Count);
                        Cat randomCat = patrol.patrolCats[randCat];
                        Console.WriteLine(randomCat.name + " lets out a terrible howl. Their neck has been caught in a twoleg device!");
                        randomCat.wounds.Add(new Wound(7, Wounds.woundDict["Slice"].type, "Neck"));
                        for(int i2 = 0; i2 < patrol.patrolCats.Count; i2++)
                        {
                            if(patrol.patrolCats[i2].name != randomCat.name)
                            {
                                float tryReleaseRoll = rand.Next(0, 21);
                                tryReleaseRoll += patrol.patrolCats[i2].statBlock.intelligence;
                                if(tryReleaseRoll >= 15)
                                {
                                    Console.WriteLine(patrol.patrolCats[i2].name + " manages to free " + randomCat.name + "!");
                                    break;
                                }
                            }
                        }
                        Console.WriteLine(randomCat.name + " has lost too much blood. They die in the trap.");
                        randomCat.health = 0;
                        patrol.patrolCats.Remove(randomCat);
                        clan.clanCats.Remove(randomCat); MainFile.PressEToContinue();
                        break;
                    case 12:
                        Console.Clear();
                        Console.WriteLine("You hear a horrible howl. Is that a twoleg kit?!\nThe patrol flees until they lose their pursuer. Everyone is exhausted.");
                        patrol = RemoveEnergy(patrol); MainFile.PressEToContinue();
                        break;
                }
            }
            
        }

        public Patrol AddEnergy(Patrol clan)
        {
            for(int i = 0; i < clan.patrolCats.Count; i++)
            {
                clan.patrolCats[i].energy += 3;
            }
            return clan;
        }

        public Patrol RemoveEnergy(Patrol clan)
        {
            for (int i = 0; i < clan.patrolCats.Count; i++)
            {
                clan.patrolCats[i].energy -= 2;
            }
            return clan;
        }

        public Cat[] CreateRandomCats(int count, StatBlock origin)
        {
            List<Cat> cats = new List<Cat>();
            for(int i = 0; i < count; i++)
            {
                string name = Defs.GetRandomName();
                Defs.Gender gender = Defs.GetRandomGender();
                Defs.Rank rank = Defs.Rank.Loner;
                int age = Defs.GetRandomAgeAdult();
                string[] personality = Defs.GetRandomPersonality();
                Skills skills = new Skills(0, 0);
                Cat cat = new Cat(name, "", rank, gender, age, origin, personality, skills);
                cats.Add(cat);
            }
            return cats.ToArray();
        }

        public Cat ChoosePersuader(Cat[] patrolCats)
        {
            Console.WriteLine("\n\nWho should try to speak to this stranger?");
            string catOptions = "";
            for (int i = 0; i < patrolCats.Length; i++)
            {
                catOptions += "[" + i + "]: ";

                Cat cat = (Cat)patrolCats[i];
                catOptions += cat.name;
                
                catOptions += "\n";
            }
            Console.WriteLine(catOptions);
            string checkPersuade = "";
            while (!checkPersuade.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkPersuade) || int.Parse(checkPersuade) > patrolCats.Length)
            {
                checkPersuade = Console.ReadLine();

            }
            int introNumber = int.Parse(checkPersuade);
            return patrolCats[introNumber];
        }
        public void AddAllCats(Cat[] cats, Patrol patrol, Clan clan)
        {
            for(int i = 0; i < cats.Length; i++)
            {
                patrol.patrolCats.Add(cats[i]);
                clan.clanCats.Add(cats[i]);
            }
        }

    }
}
