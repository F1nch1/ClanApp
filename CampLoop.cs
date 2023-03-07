using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Threading;

namespace ClanCreatorApp
{
    [Serializable]
    public struct Patrol
    {
        public List<Cat> patrolCats;
        public List<Item> itemsHeld;
        

        public Patrol(List<Cat> patrol, List<Item> items = null)
        {
            patrolCats = patrol;
            if(items == null)
            {
                itemsHeld = new List<Item>();
            }
            else
            {
                itemsHeld = items;
            }
        }
    }
    public class CampLoop
    {
        bool playing = true;
        public Char[,] charTest = new Char[4, 4];
        public bool patrolling = false;
        public WorldStatus activeWorld;
        public Clan activeClan;
        public Map activeMap;
        Patrol activePatrol;
        public string save;
        public void MainCampLoop(Clan clan, WorldStatus world, Map map, string saveName, Patrol patrol)
        {
            int i = 0;
            while (playing)
            {
                save = saveName;
                Console.Clear();
                if (activeWorld.saveName == null)
                {
                    activeWorld = world;
                }
                if (activeClan.name == null)
                {
                    activeClan = clan;
                }
                if (activeMap == null)
                {
                    activeMap = map;
                }
                if(patrol.patrolCats != null)
                {

                    patrolling = true;
                    if (patrol.patrolCats.Count > 0)
                    {
                        activePatrol = patrol;
                        while (patrolling)
                        {
                            map.MapDisplay(ref activePatrol, ref patrolling, activeClan, this);
                        }
                        patrolling = false;
                    }
                }
                else
                {
                    patrolling = false;
                }
                save = saveName;
                if (activeClan.clanCats.Count > 0)
                {
                    //Console.WriteLine("Saving...");

                    
                    //Console.WriteLine("Saved!");
                    if (clan.clanCats.Count > 0)
                    {
                        i++;
                        var originalX = Console.CursorLeft;
                        var originalY = Console.CursorTop;
                        //Console.CursorVisible = true;
                        //var originalX = Console.CursorLeft;
                        //var originalY = Console.CursorTop;
                        Console.WriteLine(CampDisplay(clan, activeWorld));
                        switch (CampOptions(clan, activeWorld))
                        {
                            case 0:
                                Patrol(clan, activeWorld, activeMap);
                                patrolling = true;
                                break;
                            case 1:
                                ClanInformation();
                                break;
                            case 2:
                                playing = false;
                                //Thread.Sleep(1000);
                                break;
                            case 3:
                                AdvanceDay(clan, activeWorld);
                                EventLog();
                                break;
                            case 4:
                                Save.SaveGame(activeClan, activeWorld, save, activeMap.tileMap);
                                break;
                        }
                        Console.SetCursorPosition(originalX, originalY);
                        //Console.SetWindowPosition(0, i - 1);
                        if(patrolling)
                        {
                            Save.SaveGame(activeClan, activeWorld, saveName, activeMap.tileMap, activePatrol);
                        }
                        else
                        {
                            Save.SaveGame(activeClan, activeWorld, saveName, activeMap.tileMap);
                        }
                    }
                    else
                    {
                        playing = false;
                        break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("This is an empty place.");
                    Thread.Sleep(4000);
                    Console.Clear();
                    playing = false;
                    break;
                }
            }
        }

        public string CampDisplay(Clan clan, WorldStatus world)
        {
            Console.Clear();
            string output = "";
            output += "Day: " + activeWorld.day + " | Season: " + World.seasons[activeWorld.season] + " | Weather: " + activeWorld.weather;
            if(clan.freshKillPile.Count == 0)
            {
                output += "\nThe fresh kill pile is empty!";
            }
            output += "\n";
            return output;
        }

        public void ClanInformation()
        {
            Console.Clear();
            string checkCamp = "";
            while (!checkCamp.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkCamp) || int.Parse(checkCamp) > 2)
            {
                //if(!patrolling)
                //{
                Console.WriteLine("[0]: View Fresh Kill Pile   [1]: View Item Hoard   [2]: View Alliegances");
                Console.CursorVisible = true;
                checkCamp = Console.ReadLine();


                //}

            }
            int campNumber = int.Parse(checkCamp);
            switch(campNumber)
            {
                case 0:
                    ViewFreshKillPile(activeClan);
                    break;
                case 1:
                    ViewItemHoard(activeClan);
                    break;
                case 2:
                    Alleigances(activeClan.clanCats.ToArray());
                    break;
            }
        }



        public int CampOptions(Clan clan, WorldStatus world)
        {
            string checkCamp = "";
            while (!checkCamp.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkCamp) || int.Parse(checkCamp) > 4)
            {
                //if(!patrolling)
                //{
                    Console.WriteLine("[0]: Patrol   [1]: View Clan Information   [2]: Return To Main Menu   [3]: Next Day   [4]: Save");
                    Console.CursorVisible = true;
                    checkCamp = Console.ReadLine();
                    
                    
                //}
                
            }
            int campNumber = int.Parse(checkCamp);
            return campNumber;
        }

        public void Patrol(Clan clan, WorldStatus world, Map map)
        {
            if(CanPatrol(clan))
            {
                //Patrol
                Patrol patrol = CreatePatrol(clan);
                activePatrol = patrol;
                //Console.WriteLine(patrol.patrolCats.Count);
                patrolling = true;
                if (patrol.patrolCats != null)
                {
                    if(patrol.patrolCats.Count > 0)
                    {
                        while (patrolling)
                        {
                            map.MapDisplay(ref activePatrol, ref patrolling, clan, this);
                        }
                        patrolling = false;
                        activePatrol = new Patrol();
                    }


                    

                }
                if(patrol.itemsHeld != null)
                {
                    if (patrol.itemsHeld.Count > 0)
                    {
                        for (int i = 0; i < patrol.itemsHeld.Count; i++)
                        {
                            if (patrol.itemsHeld[i] is FreshKill)
                            {
                                activeClan.freshKillPile.Add((FreshKill)patrol.itemsHeld[i]);
                            }
                            else
                            {
                                activeClan.items.Add(patrol.itemsHeld[i]);
                            }
                        }
                    }
                }
            }
            else if(!AnyPatrolCats(clan))
            {
                Console.WriteLine("You have no cats that can patrol.");
                Thread.Sleep(2000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("All of your cats are too tired to patrol. They must sleep.");
                Thread.Sleep(2000);
                Console.Clear();
            }
            Save.SaveGame(activeClan, activeWorld, save, activeMap.tileMap);
        }

        public Patrol CreatePatrol(Clan clan)
        {
            

            List<Cat> catsForPatrol = FindCatsEligableForPatrol(clan);
            List<Cat> catsOnPatrol = new List<Cat>();
            string optionsNumber = "";
            bool finished = false;
            
            
                
            

            

            int options = -1;
            while(!finished)
            {
                Console.Clear();
                
                while ((!optionsNumber.All(Char.IsDigit) && optionsNumber.ToLower() != "e" && optionsNumber.ToLower() != "x") || string.IsNullOrWhiteSpace(optionsNumber) || int.Parse(optionsNumber) > catsForPatrol.Count - 1)
                    {
                        if (catsForPatrol.Count > 0)
                        {
                        Console.WriteLine("Who should go on patrol?\n\n");
                        for (int i = 0; i < catsForPatrol.Count; i++)
                            {
                                Console.WriteLine("[" + i + "]: " + CreateDescription(catsForPatrol[i]) + "    Energy: " + catsForPatrol[i].energy + " | Wounded: " + (catsForPatrol[i].wounds.Count > 0));
                            }
                            Console.WriteLine("Enter 'e' to finish adding cats. | Enter 'x' to go back.\n");
                            optionsNumber = Console.ReadLine().ToLower();
                            if (optionsNumber == "e")
                            {
                                if(catsOnPatrol.Count > 0)
                                {
                                    Patrol patrol = new Patrol(catsOnPatrol);
                                    finished = true;
                                    return patrol;
                                }
                                else
                                {
                                    finished = true;
                                    
                                    break;
                                }
                            }
                            else if(optionsNumber == "x")
                            {
                                finished = true;
                                break;
                            }
                            else if(int.TryParse(optionsNumber, out options))
                            {
                                options = int.Parse(optionsNumber);
                                Cat chosen = catsForPatrol[options];
                                catsForPatrol.Remove(chosen);
                                catsOnPatrol.Add(chosen);
                                optionsNumber = "";
                                Console.Clear();
                            }
                        }
                        else
                        {
                        Console.Clear();
                        Console.CursorVisible = false;
                        Console.WriteLine("All cats that can patrol have been added.");
                        Thread.Sleep(2000);
                        finished = true;
                            return new Patrol(catsOnPatrol);
                        }
                }
                
                
                
            }
            

            return new Patrol();


            
        }

        public string Alleigances(Cat[] clan)
        {
            //Alleigances
            Console.WriteLine("Allegiances");
            string output = "";
            List<Defs.Rank> usedRanks = new List<Defs.Rank>();
            int i = 0;
            List<Cat> catsInOrder = new List<Cat>();
            foreach(Defs.Rank rank in Defs.rankDict.Keys)
            {
                foreach(Cat cat in clan)
                {
                    if(cat.rankAccesor == rank && !CheckQueen(cat))
                    {
                        if(!usedRanks.Contains(rank))
                        {
                            usedRanks.Add(rank);
                            output += Defs.rankDict[rank] + "- \n";
                        }
                        
                        output += "   " + CreateDescription(cat, i);
                        catsInOrder.Add(cat);
                        i++;
                    }
                    else if(rank == Defs.Rank.Queen && CheckQueen(cat))
                    {
                        if (!usedRanks.Contains(Defs.Rank.Queen))
                        {
                            usedRanks.Add(rank);
                            output += Defs.rankDict[Defs.Rank.Queen] + "- \n";
                        }

                        output += "   " + CreateDescription(cat, i);
                        catsInOrder.Add(cat);
                        i++;
                    }
                }
            }
            output += "\nEnter 'e' to continue, or choose a cat to learn more.";
            Console.WriteLine(output);
            string checkCamp = "";
            int campNumber;
            while ((!checkCamp.All(Char.IsDigit) && checkCamp.ToLower() != "e") || string.IsNullOrWhiteSpace(checkCamp) || !int.TryParse(checkCamp, out campNumber) || campNumber > catsInOrder.Count - 1)
            {
                checkCamp = Console.ReadLine();
                if (checkCamp == "e")
                {
                    break;
                }
            }
            
            if(int.TryParse(checkCamp, out campNumber))
            {
                CreateAdvancedDescription(catsInOrder[campNumber]);
            }
            
            return output;
        }
        public string CreateAdvancedDescription(Cat cat)
        {
            Console.Clear();
            string output = "";
            Console.WriteLine("Name: " + cat.name);
            output += "\nName: " + cat.name;
            Console.WriteLine("Age: " + cat.age);
            output += "\nAge: " + cat.age;
            Console.WriteLine("Gender: " + cat.gender);
            output += "\nGender: " + cat.gender;
            Console.WriteLine("Personality: " + Utils.CreateList(cat.personality));
            output += "\nPersonality: " + Utils.CreateList(cat.personality);
            Console.WriteLine("Health: " + cat.health);
            output += "\nHealth: " + cat.health;
            Console.WriteLine("Energy: " + cat.energy);
            output += "\nEnergy: " + cat.energy;
            Console.WriteLine("Rank: " + GetRank(cat));
            output += "\nRank: " + GetRank(cat);
            Console.WriteLine("Stats: | " + cat.statBlock.agility + " | " + cat.statBlock.constitution + " | " + cat.statBlock.finesse + " | " + cat.statBlock.intelligence + " | " + cat.statBlock.strength + " | " + cat.statBlock.tenacity + " |");
            output += "\nStats: | " + cat.statBlock.agility + " | " + cat.statBlock.constitution + " | " + cat.statBlock.finesse + " | " + cat.statBlock.intelligence + " | " + cat.statBlock.strength + " | " + cat.statBlock.tenacity + " |";
            Console.WriteLine("       Agility Constitution Finesse Intelligence Strength Tenacity\n");
            output += "\n       Agility Constitution Finesse Intelligence Strength Tenacity\n";
            Console.WriteLine("Skills:\n   Hunting: " + cat.skills.hunting + "\n   Medicine: " + cat.skills.medicine);
            output += "\nSkills:\n\tHunting: " + cat.skills.hunting + "\n\tMedicine: " + cat.skills.medicine;
            if(cat.pregnancy != -1)
            {
                Console.WriteLine("\nPregnancy: " + cat.pregnancy + " days until delivery!");
                output += "\nPregnancy: " + cat.pregnancy + " days until delivery!";
            }
            for(int i = 0; i < cat.relations.Count; i++)
            {
                if(i == 0)
                {
                    Console.WriteLine("\nRelations:");
                    output += "\nRelations:";
                }
                Console.WriteLine("\t" + cat.relations.Keys.ElementAt(i) + ": " + cat.relations.Values.ElementAt(i));
                output += "\n\t" + cat.relations.Keys.ElementAt(i) + ": " + cat.relations.Values.ElementAt(i);
            }
            for (int i = 0; i < cat.wounds.Count; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("\nWounds:");
                    output += "\nWounds:";
                }
                Console.WriteLine("\t" + cat.wounds[i].type + ": \n\t\tSeverity - " + cat.wounds[i].counter);
                output += "\t" + cat.wounds[i].type + ": \n\tSeverity - " + cat.wounds[i].counter;
                Console.WriteLine("\t\tTreated? " + cat.wounds[i].treated);
                output += "\t\tTreated? " + cat.wounds[i].treated;
                Console.WriteLine("\t\tInfection Treated? " + cat.wounds[i].infectionTreated);
                output += "\t\tInfection Treated? " + cat.wounds[i].infectionTreated;
            }
            for (int i = 0; i < cat.diseases.Count; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("\nDiseases:");
                    output += "\nDiseases:";
                }
                Console.WriteLine("\t" + cat.diseases[i].type + ": \n\t\tDays to heal - " + cat.diseases[i].counter);
                output += "\t" + cat.diseases[i].type + ": \n\tDays to heal - " + cat.diseases[i].counter;
                Console.WriteLine("\t\tTreated? " + cat.diseases[i].treated);
                output += "\t\tTreated? " + cat.diseases[i].treated;
            }
            for(int i = 0; i < cat.familyTree.Count; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("\nFamily:");
                    output += "\nFamily:";
                }
                Console.WriteLine("\t" + cat.familyTree.Keys.ElementAt(i) + ": " + cat.familyTree.Values.ElementAt(i));
                output += "\t" + cat.familyTree.Keys.ElementAt(i) + ": " + cat.familyTree.Values.ElementAt(i);
            }
            MainFile.PressEToContinue();
            return output;
        }

        public Defs.Rank GetRank(Cat cat)
        {
            if (CheckQueen(cat))
            {
                return Defs.Rank.Queen;
            }
            else
            {
                return cat.rankAccesor;
            }
        }

        public string CreateDescription(Cat cat, int i)
        {
            string output = "[" + i + "]: ";
            output += cat.name + ": ";
            if(Utils.CheckVowel(Utils.NumberToWords(cat.age)[0]))
            {
                output += "An ";
            }
            else
            {
                output += "A ";
            }
            if(cat.genderAccesor == Defs.Gender.Other)
            {
                output += cat.age + " moon old cat.\n";
            }
            else
            {
                output += cat.age + " moon old " + cat.gender.ToLower() + ".\n";
            }
            
            return output;
        }
        public string CreateDescription(Cat cat)
        {
            string output = "";
            output += cat.name + ": ";
            if (Utils.CheckVowel(Utils.NumberToWords(cat.age)[0]))
            {
                output += "An ";
            }
            else
            {
                output += "A ";
            }
            if (cat.genderAccesor == Defs.Gender.Other)
            {
                output += cat.age + " moon old cat.\n";
            }
            else
            {
                output += cat.age + " moon old " + cat.gender.ToLower() + ".\n";
            }

            return output;
        }
        public void ViewItemHoard(Clan clan)
        {
            Console.Clear();
            //Fresh kill pile
            List<Item> items = clan.items;
            string output = "";
            if (items.Count != 0)
            {
                output += "Item Hoard contains:\n\n";
                for(int i = 0; i < items.Count; i++)
                {
                    if(i != items.Count - 1)
                    {
                        output += items[i].type.First().ToString().ToUpper() + items[i].type.Substring(1);
                        output += ": " + items[i].description + "\n\n";
                    }
                    else
                    {
                        output += items[i].type.First().ToString().ToUpper() + items[i].type.Substring(1);
                        output += ": " + items[i].description;
                    }

                }
                
            }
            else
            {
                output += "The item hoard is empty!";
            }
            Console.WriteLine(output);
            List<Item> usableItems = clan.items.Where((x => x.usable == true)).ToList();
            if(usableItems.Count == 0)
            {
                MainFile.PressEToContinue();
                
            }
            string optionsNumber = "";
            while (optionsNumber.ToLower() != "u" && optionsNumber.ToLower() != "x" && usableItems.Count > 0)
            {
                if (usableItems.Count > 0)
                {
                    
                    Console.WriteLine("\nEnter 'u' to use an item. | Enter 'x' to go back.");
                    optionsNumber = Console.ReadLine().ToLower();
                    if (optionsNumber == "u")
                    {
                        UsableItems(clan);
                    }
                    else if (optionsNumber == "x")
                    {
                        
                    }
                    
                }
                
            }

        }

        public void UsableItems(Clan clan)
        {
            Console.Clear();
            List<Item> usableItems = clan.items.Where(x => x.usable == true).ToList();
            string output = "";
            for(int i = 0; i < usableItems.Count; i++)
            {
                if(i != usableItems.Count - 1)
                {
                    output += "[" + i + "]: " + usableItems[i].type + ": " + usableItems[i].description;
                    if (Herbs.herbDict.ContainsKey(usableItems[i].type))
                    {
                        Herb herb = Herbs.herbDict[usableItems[i].type];
                        output += "\n    Uses: " + Utils.CreateList(herb.treatmentOptions) + "\n\n";
                    }
                    else
                    {
                        output += "\n\n";
                    }
                }
                else
                {
                    output += "[" + i + "]: " + usableItems[i].type + ": " + usableItems[i].description;
                    if (Herbs.herbDict.ContainsKey(usableItems[i].type))
                    {
                        Herb herb = Herbs.herbDict[usableItems[i].type];
                        output += "\n    Uses: " + Utils.CreateList(herb.treatmentOptions);
                    }
                    
                }
                
                
            }
            output += "\n\nEnter 'x' to go back";
            Console.WriteLine(output);
            string checkCamp = "";
            while ((!checkCamp.All(Char.IsDigit) || checkCamp.ToLower() != "x") || string.IsNullOrWhiteSpace(checkCamp) || int.Parse(checkCamp) > usableItems.Count - 1)
            {
                if (checkCamp.ToLower() == "x")
                {
                    break;
                }
                if (int.TryParse(checkCamp, out int campNumber))
                {
                    
                    Use(usableItems[campNumber]);
                    //Console.WriteLine("Try use " + usableItems[campNumber].type);
                    break;
                    
                }
                Console.CursorVisible = true;
                checkCamp = Console.ReadLine();
            }
            
            
            
                
            
            

        }

        public void Use(Item item)
        {
            Console.Clear();
            //Console.WriteLine("Use " + item.type);
            string output = "\n";
            List<Cat> usableCats = new List<Cat>();
            if(Herbs.herbDict.ContainsKey(item.type))
            {
                //Console.WriteLine("Item is herb");
                Herb herb = Herbs.herbDict[item.type];
                string[] uses = herb.treatmentOptions;
                for(int i = 0; i < uses.Length; i++)
                {
                    for(int i2 = 0; i2 < activeClan.clanCats.Count; i2++)
                    {
                        //Console.WriteLine(activeClan.clanCats[i2].name);
                        //Console.WriteLine(activeClan.clanCats[i2].diseases.Count);
                        //Console.WriteLine(activeClan.clanCats[i2].wounds.Count);
                        for (int i3 = 0; i3 < activeClan.clanCats[i2].diseases.Count; i3++)
                        {
                            if(activeClan.clanCats[i2].diseases[i3].type == uses[i] && activeClan.clanCats[i2].diseases[i3].treated == false)
                            {
                                usableCats.Add(activeClan.clanCats[i2]);
                            }
                            //Console.WriteLine(activeClan.clanCats[i2].diseases[i3].type);
                        }
                        for (int i5 = 0; i5 < activeClan.clanCats[i2].wounds.Count; i5++)
                        {
                            if(activeClan.clanCats[i2].wounds[i5].type.ToLower() == uses[i].ToLower() && activeClan.clanCats[i2].wounds[i5].treated == false || (activeClan.clanCats[i2].wounds[i5].infectionTreated == false && uses[i] == "Infection"))
                            {
                                usableCats.Add(activeClan.clanCats[i2]);
                            }
                            //Console.WriteLine("Type: " + activeClan.clanCats[i2].wounds[i5].type);
                            //Console.WriteLine("Use: " + uses[i]);
                            //Console.WriteLine(activeClan.clanCats[i2].wounds[i5].type);

                        }
                    }
                }
                Console.WriteLine("Use on which cat?");
                usableCats = usableCats.Distinct().ToList();
                for(int i4 = 0; i4 < usableCats.Count; i4++)
                {
                    output += "[" + i4 + "]: " + usableCats[i4].name + "\n";
                }
                output += "[" + usableCats.Count + "]: Go back";
                string checkCamp = "";
                while (!checkCamp.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkCamp) || int.Parse(checkCamp) > usableCats.Count)
                {
                    Console.WriteLine(output);
                    Console.CursorVisible = true;
                    checkCamp = Console.ReadLine();

                }
                int campNumber = int.Parse(checkCamp);
                if(campNumber == usableCats.Count)
                {

                }
                else
                {
                    Cat catToTreat = usableCats[campNumber];
                    Console.Clear();
                    Console.WriteLine("Use for which condition?");
                    List<Disease> treatableDiseases = new List<Disease>();
                    List<Wound> treatableWounds = new List<Wound>();
                    List<string> treatmentStrings = new List<string>();
                    string treatmentOutput = "\n";
                    for(int i9 = 0; i9 < herb.treatmentOptions.Length; i9++)
                    {
                        for (int i6 = 0; i6 < catToTreat.diseases.Count; i6++)
                        {
                            if(catToTreat.diseases[i6].type == herb.treatmentOptions[i9] && catToTreat.diseases[i6].treated == false)
                            {
                                treatableDiseases.Add(catToTreat.diseases[i6]);
                                treatmentStrings.Add(catToTreat.diseases[i6].type);
                            }
                        }
                        for (int i7 = 0; i7 < catToTreat.wounds.Count; i7++)
                        {
                            if (catToTreat.wounds[i7].type == herb.treatmentOptions[i9] && catToTreat.wounds[i7].treated == false)
                            {
                                treatableWounds.Add(catToTreat.wounds[i7]);
                                treatmentStrings.Add(catToTreat.wounds[i7].type);
                            }
                            else if ((catToTreat.wounds[i7].infectionTreated == false && herb.treatmentOptions[i9] == "Infection"))
                            {
                                treatableWounds.Add(catToTreat.wounds[i7]);
                                treatmentStrings.Add("Infection");
                            }
                        }
                    }

                    for (int i8 = 0; i8 < treatmentStrings.Count; i8++)
                    {
                        treatmentOutput += "[" + i8 + "]: " + treatmentStrings[i8] + "\n";
                    }
                    Console.WriteLine(treatmentOutput);
                    string checkTreatment = "";
                    while (!checkTreatment.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkTreatment) || int.Parse(checkTreatment) > treatmentStrings.Count)
                    {
                        Console.CursorVisible = true;
                        checkTreatment = Console.ReadLine();

                    }
                    int treatmentnum = int.Parse(checkTreatment);
                    string treatment = treatmentStrings[treatmentnum];
                    bool treated = false;
                    while (!treated)
                    {
                        //Console.WriteLine("checkTreatment");
                        for (int i = 0; i < catToTreat.diseases.Count; i++)
                        {
                            if (catToTreat.diseases[i].type == treatment)
                            {
                                Disease dis = catToTreat.diseases[i];
                                dis.treated = true;
                                //Console.WriteLine("Now treated");
                                catToTreat.diseases[i] = dis;
                                treated = true;
                                activeClan.items.Remove(item);
                            }
                        }
                        for (int i2 = 0; i2 < catToTreat.wounds.Count; i2++)
                        {

                            if (catToTreat.wounds[i2].type == treatment)
                            {
                                Wound dis = catToTreat.wounds[i2];
                                dis.treated = true;
                                //Console.WriteLine("Now treated");
                                catToTreat.wounds[i2] = dis;
                                treated = true;
                                activeClan.items.Remove(item);
                            }
                            else if(treatment == "Infection")
                            {
                                Wound dis = catToTreat.wounds[i2];
                                dis.infectionTreated = true;
                                //Console.WriteLine("Now treated");
                                catToTreat.wounds[i2] = dis;
                                treated = true;
                                activeClan.items.Remove(item);
                            }
                        }
                    }
                }
            }
                
                
        }

        public void ViewFreshKillPile(Clan clan)
        {
            Console.Clear();
            //Fresh kill pile
            List<FreshKill> fkp = clan.freshKillPile;
            string output = "";
            if(fkp.Count != 0)
            {
                output += "Fresh Kill Pile contains:\n\n";
                foreach (FreshKill fk in fkp)
                {
                    output += "\n" + fk.species + ":\n   This will feed " + fk.size + " cat";
                    if(fk.size > 1)
                    {
                        output += "s";
                    }
                    else
                    {
                        output += "";
                    }
                }
            }
            else
            {
                output += "The fresh kill pile is empty!";
            }
            Console.WriteLine(output);
            MainFile.PressEToContinue();
        }

        public List<Cat> FindCatsEligableForPatrol(Clan clan)
        {
            List<Cat> listCats = new List<Cat>();
            for(int i = 0; i < clan.clanCats.Count; i++)
            {
                Defs.Rank rank = clan.clanCats[i].rankAccesor;
                if (Defs.catsForPatrol.Contains(rank) && clan.clanCats[i].energy > 0 && clan.clanCats[i].health > 0 && clan.clanCats[i].age >= 6 && !CheckQueen(clan.clanCats[i]))
                {
                    listCats.Add(clan.clanCats[i]);
                }
            }
            return listCats;
        }
        public bool AnyPatrolCats(Clan clan)
        {
            for (int i = 0; i < clan.clanCats.Count; i++)
            {
                Defs.Rank rank = clan.clanCats[i].rankAccesor;
                if (Defs.catsForPatrol.Contains(rank) && clan.clanCats[i].health > 0 && clan.clanCats[i].age >= 6)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanPatrol(Clan clan)
        {
            List<Cat> cats = FindCatsEligableForPatrol(clan);
            for (int i = 0; i < cats.Count; i++)
            {
                Defs.Rank rank = clan.clanCats[i].rankAccesor;
                if (Defs.catsForPatrol.Contains(rank) && clan.clanCats[i].energy > 0 && clan.clanCats[i].health > 0 && clan.clanCats[i].age >= 6)
                {
                    return true;
                }
            }
            return false;
        }

        public void AdvanceDay(Clan clan, WorldStatus world)
        {
            activeWorld.day++;
            activeWorld.weather = World.GetRandomWeather(world.season);
            //EatFreshKill();
            
            
            

            if (activeWorld.day % 30 == 0)
            {
                AdvanceMonth();
            }
        }

        public void Interactions()
        {
            CampInteractions interactions = new CampInteractions();
            if(activeClan.clanCats.Count == 1)
            {
                Console.WriteLine(activeClan.clanCats[0].name + " is alone.");
            }
            else
            {
                for (int i = 0; i < activeClan.clanCats.Count; i++)
                {
                    Random r = new Random();
                    activeClan.clanCats[i] = interactions.Interaction(activeClan.clanCats[i], activeClan, MainFile.interactionData);
                }
            }
        }

        public void AdvanceMonth()
        {
            for (int i = 0; i < activeClan.clanCats.Count; i++)
            {
                activeClan.clanCats[i].age++;
            }
        }

        public float MaxHealth(Cat cat)
        {
            if (cat.statBlock.constitution >= 1)
            {
                return cat.statBlock.constitution * 5;
            }
            else
            {
                return 5;
            }
        }

        public void Rest(Clan clan)
        {
            for(int i = 0; i < activeClan.clanCats.Count; i++)
            {
                float oneOrTen = Math.Max(1, activeClan.clanCats[i].statBlock.tenacity);
                activeClan.clanCats[i].energy = (int)Math.Floor(oneOrTen * 2.5f);
                if(activeClan.clanCats[i].health < MaxHealth(activeClan.clanCats[i])) 
                {
                    activeClan.clanCats[i].health += .5f;
                }
                if(activeClan.clanCats[i].pregnancy != -1)
                {
                    if(activeClan.clanCats[i].pregnancy == 0)
                    {
                        Cat[] litter = LitterOfKittens(activeClan.clanCats[i]);
                        activeClan.clanCats.AddRange(litter);
                        Console.WriteLine(activeClan.clanCats[i].name + " has given birth to a litter of " + litter.Length + " kittens!");
                        activeClan.clanCats[i].pregnancy = -1;
                    }
                    else if(activeClan.clanCats[i].pregnancy > 0 && activeClan.clanCats[i].pregnancy <= 20)
                    {
                        activeClan.clanCats[i].pregnancy -= 1;
                    }
                    else
                    {
                        activeClan.clanCats[i].pregnancy -= 1;
                    }
                }
            }

        }
        public Cat[] LitterOfKittens(Cat queen)
        {
            Random rand = new Random(Utils.GetRandom());
            int litterSize;
            List<Cat> litter = new List<Cat>();
            if (queen.age <= 24)
            {
                litterSize = rand.Next(1, 4);
            }
            else
            {
                litterSize = rand.Next(1, 7);
            }
            for (int i = 0; i < litterSize; i++)
            {
                Cat kitten = new Cat(Defs.GetRandomName(), "", Defs.Rank.Kit, Defs.GetRandomGender(), 0, StatBlocks.allOrigins[0], Defs.GetRandomPersonality(), new Skills(0, 0));
                kitten.familyTree.Add(queen.name, "Mother");
                litter.Add(kitten);
            }
            return litter.ToArray();
        }
        public void EatFreshKill()
        {
            List<FreshKill> freshKillPile = activeClan.freshKillPile;
            for(int i = 0; i < activeClan.clanCats.Count; i++)
            {
                for(int i2 = 0; i2 < freshKillPile.Count; i2++)
                {
                    if (freshKillPile[i2].size <= 0)
                    {
                        freshKillPile.RemoveAt(i2);
                    }
                }
                if (freshKillPile.Count > 0 && activeClan.clanCats[i].age >= 6)
                {
                    
                    int age = activeClan.clanCats[i].age;
                    if (age < 6)
                    {

                    }
                    else if (age < 8)
                    {
                        
                        freshKillPile[0].size -= .5f;
                    }
                    else if (age < 10)
                    {
                        if (freshKillPile[0].size <= 0)
                        {
                            freshKillPile.RemoveAt(0);
                        }
                        freshKillPile[0].size -= .75f;
                    }
                    else if(CheckQueen(activeClan.clanCats[i]))
                    {
                        if (freshKillPile[0].size <= 0)
                        {
                            freshKillPile.RemoveAt(0);
                        }
                        freshKillPile[0].size -= 1.5f;
                    }
                    else
                    {
                        if (freshKillPile[0].size <= 0)
                        {
                            freshKillPile.RemoveAt(0);
                        }
                        freshKillPile[0].size -= 1;
                    }
                }
                else if(freshKillPile.Count > 0 && activeClan.clanCats[i].age < 6)
                {
                    if(FindMother(activeClan.clanCats[i]) != null)
                    {
                        Cat mother = activeClan.clanCats[i];
                        if(mother.health > 2)
                        {

                        }
                        else
                        {
                            Console.WriteLine(activeClan.clanCats[i].name + " has gone hungry.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(activeClan.clanCats[i].name + " has gone hungry.");
                }
                
                activeClan.clanCats[i].health -= 1;
            }
            activeClan.freshKillPile = freshKillPile;
        }

        public void Eat()
        {
            
            List<Cat> catsInOrder = new List<Cat>();
            for(int i = 0; i < Defs.eatingOrderLoner.Count; i++)
            {
                for(int i2 = 0; i2 < activeClan.clanCats.Count; i2++)
                {
                    if(CheckQueen(activeClan.clanCats[i2]) && Defs.eatingOrderLoner[i] == Defs.Rank.Queen)
                    {
                        catsInOrder.Add(activeClan.clanCats[i2]);
                    }
                    else if(activeClan.clanCats[i2].rankAccesor == Defs.eatingOrderLoner[i])
                    {
                        catsInOrder.Add(activeClan.clanCats[i2]);
                    }
                }
            }
            for(int i = 0; i < catsInOrder.Count; i++)
            {
                float foodToEat = 0;
                Cat cat = catsInOrder[i];
                if(CheckQueen(cat))
                {
                    foodToEat = 1.5f;
                }
                else if(cat.age < 6)
                {
                    KittenEat(cat);
                }
                else if(cat.age >= 6 && cat.age < 12)
                {
                    foodToEat = .75f;
                }
                else
                {
                    foodToEat = 1;
                }
                while(activeClan.freshKillPile.Count > 0 && foodToEat > 0)
                {
                    
                    for(int i2 = 0; i2 < activeClan.freshKillPile.Count; i2++)
                    {
                        while(activeClan.freshKillPile[i2].size > 0 && foodToEat > 0)
                        {
                            
                            float amountToRemove = foodToEat;
                            activeClan.freshKillPile[i2].size -= amountToRemove;
                            foodToEat -= amountToRemove;

                        }
                        if(activeClan.freshKillPile[i2].size <= 0)
                        {
                            activeClan.freshKillPile.Remove(activeClan.freshKillPile[i2]);
                        }
                        
                    }
                }
                if(activeClan.freshKillPile.Count <= 0 && foodToEat > 0)
                {
                    Console.WriteLine(cat.name + " has gone hungry.");
                    cat.health -= 1f;
                }
            }
            
        }

        public void KittenEat(Cat kit)
        {
            if (FindMother(kit) != null)
            {
                Cat mother = FindMother(kit);
                if (mother.health > 2)
                {

                }
                else
                {
                    Console.WriteLine(kit.name + " has gone hungry.");
                }
            }
            else
            {
                Console.WriteLine(kit.name + " has gone hungry.");
            }
        }

        public void EventLog()
        {
            Console.Clear();
            Eat();
            CheckWounds();
            CheckDiseases();
            Interactions();
            CheckAlive();
            AgeUp();
            Rest(activeClan);
            MainFile.PressEToContinue();
            Console.Clear();
        }

        public void CheckAlive()
        {
            for (int i = 0; i < activeClan.clanCats.Count; i++)
            {
                if (activeClan.clanCats[i].health <= 0)
                {
                    Console.WriteLine(activeClan.clanCats[i].name + " has died during the night.");
                    activeClan.clanCats.Remove(activeClan.clanCats[i]);
                }
            }
        }

        public void AgeUp()
        {
            for (int i = 0; i < activeClan.clanCats.Count; i++)
            {
                int age = activeClan.clanCats[i].age;
                Defs.Rank rank = activeClan.clanCats[i].rankAccesor;
                if (age >= 6 && rank == Defs.Rank.Kit)
                {
                    activeClan.clanCats[i].rankAccesor = Defs.Rank.Apprentice;
                    Console.WriteLine("The clan has celebrated " + activeClan.clanCats[i].name + "'s apprentice ceremony!");
                }
            }
        }

        public void CheckWounds()
        {
            for (int i = 0; i < activeClan.clanCats.Count; i++)
            {
                List<Wound> wounds = activeClan.clanCats[i].wounds;
                for(int i2 = 0; i2 < wounds.Count; i2++)
                {
                    //Console.WriteLine(activeClan.clanCats[i].name + " has a " + activeClan.clanCats[i].wounds[i2].type + " with counter " + activeClan.clanCats[i].wounds[i2].counter);
                    //MainFile.PressEToContinue();
                    if (wounds[i2].counter >= 15)
                    {
                        Console.WriteLine(activeClan.clanCats[i].name + " has died during the night.");
                        activeClan.clanCats.Remove(activeClan.clanCats[i]);
                    }
                    else
                    {
                        CheckInfection(wounds[i2], activeClan.clanCats[i]);
                        if (wounds[i2].treated)
                        {
                            wounds[i2] = TreatedWoundRecovery(wounds[i2]);
                            if (wounds[i2].counter <= 0)
                            {
                                activeClan.clanCats[i].wounds.Remove(wounds[i2]);
                            }
                        }
                        else
                        {
                            wounds[i2] = UntreatedWoundRecovery(wounds[i2]);
                            if (wounds[i2].counter <= 0)
                            {
                                activeClan.clanCats[i].wounds.Remove(wounds[i2]);
                            }
                        }

                        
                    }
                    
                }
            }
        }

        public void CheckDiseases()
        {
            for (int i = 0; i < activeClan.clanCats.Count; i++)
            {
                List<Disease> diseases = activeClan.clanCats[i].diseases;
                for (int i2 = 0; i2 < diseases.Count; i2++)
                {
                    //Console.WriteLine(activeClan.clanCats[i].name + " has a " + activeClan.clanCats[i].diseases[i2].type + " with counter " + activeClan.clanCats[i].diseases[i2].counter);
                    if (diseases[i2].treated)
                    {
                        diseases[i2] = TreatedDiseaseRecovery(diseases[i2]);
                        if(diseases[i2].counter <= 0)
                        {
                            activeClan.clanCats[i].diseases.Remove(diseases[i2]);
                        }
                    }
                    else
                    {
                        diseases[i2] = UntreatedDiseaseRecovery(diseases[i2]);
                        if (diseases[i2].counter <= 0)
                        {
                            activeClan.clanCats[i].diseases.Remove(diseases[i2]);
                        }
                    }
                    if (diseases[i2].counter >= 10)
                    {
                        Console.WriteLine(activeClan.clanCats[i].name + " has died during the night.");
                        activeClan.clanCats.Remove(activeClan.clanCats[i]);
                    }
                }
            }
        }
        public Wound CheckInfection(Wound wound, Cat cat)
        {
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, 101);
            int chance = 0;
            if(wound.infectionTreated)
            {
                chance = wound.counter;
                if(num <= chance && !cat.diseases.Exists(x => x.type == "Infection"))
                {
                    //Infection
                    cat.diseases.Add(new Disease(5, Diseases.diseaseDict["Infection"].type));
                    Console.WriteLine(cat.name + "'s " + wound.type.ToLower() + " has become infected.");
                }
            }
            else
            {
                chance = wound.counter * 5;
                if (num <= chance && !cat.diseases.Exists(x => x.type == "Infection"))
                {
                    //Infection
                    cat.diseases.Add(new Disease(5, Diseases.diseaseDict["Infection"].type));
                    Console.WriteLine(cat.name + "'s " + wound.type.ToLower() + " has become infected.");
                }
            }
            return wound;
        }

        public bool CheckQueen(Cat cat)
        {

            bool hasKit = false;
            
                    
                    for(int i = 0; i < cat.familyTree.Values.Count; i++)
                    {
                        if(Utils.GetCatInClan(activeClan, cat.familyTree.Keys.ElementAt(i)) != null)
                        {
                            Cat relationCat = Utils.GetCatInClan(activeClan, cat.familyTree.Keys.ElementAt(i));
                            if (relationCat.age < 6 && relationCat.familyTree[cat.name] == "Mother")
                            {
                            
                                hasKit = true;
                        }
                    
                    }
                    if(cat.pregnancy <= 20 && cat.pregnancy != -1)
                    {
                        return true;
                    }
                
            }
                    
                
            return hasKit;
            
        }
        public Cat FindMother(Cat cat)
        {
            for (int i = 0; i < cat.familyTree.Values.Count; i++)
            {
                if (Utils.GetCatInClan(activeClan, cat.familyTree.Keys.ElementAt(i)) != null)
                {
                    Cat relationCat = Utils.GetCatInClan(activeClan, cat.familyTree.Keys.ElementAt(i));
                    if (cat.familyTree[relationCat.name] == "Mother")
                    {

                        return relationCat;
                    }

                }
            }
            return null;
        }
        public Wound TreatedWoundRecovery(Wound wound)
        {
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, 4);
            switch(num)
            {
                case 0:
                    wound.counter--;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    wound.counter++;
                    break;
            }
            return wound;
        }
        public Wound UntreatedWoundRecovery(Wound wound)
        {
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, 101);
            if(num <= 70)
            {
                wound.counter--;
            }
            else if(num > 70 && num < 85)
            {
                wound.counter++;
            }
            return wound;
        }

        public Disease TreatedDiseaseRecovery(Disease disease)
        {
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, 101);
            int nothingChance = (100 - Diseases.diseaseDict[disease.type].treatedThreshold) / 2;
            if (num <= Diseases.diseaseDict[disease.type].treatedThreshold)
            {
                disease.counter++;
            }
            else if (num > Diseases.diseaseDict[disease.type].treatedThreshold && num < Diseases.diseaseDict[disease.type].treatedThreshold + nothingChance)
            {
                disease.counter--;
            }
            return disease;
        }

        public Disease UntreatedDiseaseRecovery(Disease disease)
        {
            //Console.WriteLine(disease.type);
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, 101);
            int nothingChance = (100 - Diseases.diseaseDict[disease.type].untreatedThreshold) / 2;
            if (num <= Diseases.diseaseDict[disease.type].untreatedThreshold)
            {
                disease.counter++;
            }
            else if (num > Diseases.diseaseDict[disease.type].untreatedThreshold && num < Diseases.diseaseDict[disease.type].untreatedThreshold + nothingChance)
            {
                disease.counter--;
            }
            return disease;
        }
    }
}
