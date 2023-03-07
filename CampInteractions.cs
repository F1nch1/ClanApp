using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClanCreatorApp
{
    class CampInteractions
    {
        public string[] neutralPositiveStrings =
        {
            " shares tongues with ",
            " has a chat with ",
            " relaxes in the sun with ",
            " chases a butterfly around camp with "
        };

        public string[] neutralNegativeStrings =
        {
            " swats at ",
            " has an argument over clan politics with ",
            " has an argument about the dead with ",
            " has an argument about hunting techniques with ",
            " snaps at the tail of "
        };

        public string[] friendPositiveStrings =
        {
            " plays a game using stones with ",
            " tells a story to ",
            " listens to a story from ",
            " scratches at a tree with "
        };

        public string[] friendNegativeStrings =
        {
            " has a misunderstanding with ",
            " gets the silent treatment from ",
            " accidentally says something rude to "
        };

        public string[] bestFriendPositiveStrings =
        {
            " naps in the sun with ",
            " practices hunting moves with ",
            " practices fighting moves with ",
            " reveals a secret to ",
            " teases "
        };

        public string[] bestFriendNegativeStrings =
        {
            " accidentally upsets ",
            " gives the silent treatment to ",
            " fails to make a joke with ",
            " embarasses "
        };

        public string[] matePositiveStrings =
        {
            " goes on a walk around the camp with ",
            " spends some alone time with ",
            " licks the ear of ",
            " tells a dirty joke to ",
            " talks about kit names with ",
            " talks about the nursery with "
        };

        public string[] mateNegativeStrings =
        {
            " has a disagreement with ",
            " accidentally wakes up ",
            " has an argument with ",
            " snarls at "
        };

        public Cat Interaction(Cat cat, Clan clan, DataTable table)
        {
             
            List<Cat> interactionCats;
            interactionCats = clan.clanCats.Cast<Cat>().ToList();
            interactionCats.Remove(cat);
            if(interactionCats.Count > 0)
            {
                for (int i = 0; i < interactionCats.Count; i++)
                {
                    if (!cat.relations.ContainsKey(interactionCats[i].name))
                    {
                        cat.relations.Add(interactionCats[i].name, 0);
                        
                    }
                }
                //Choose cat
                Cat interactionCat;
                if (interactionCats.Count > 1)
                {
                    Random rand = new Random();
                    int num = rand.Next(0, interactionCats.Count);
                    interactionCat = interactionCats[num];
                }
                else if (interactionCats.Count > 0)
                {
                    interactionCat = interactionCats[0];
                }
                else
                {
                    return cat;
                }
                //Get interaction score
                float score = Persuasion.GetInteractionScore(interactionCat, cat, clan, table);
                Cat catToReturn = cat;
                if(cat.relations[interactionCat.name] < 5)
                {
                    catToReturn = InteractNeutral(cat, interactionCat, clan, score); 
                }
                else if (cat.relations[interactionCat.name] >= 5 && cat.relations[interactionCat.name] < 15)
                {
                    catToReturn = InteractFriend(cat, interactionCat, clan, score);
                }
                else if (cat.relations[interactionCat.name] >= 15 && cat.relations[interactionCat.name] < 50)
                {
                    catToReturn = InteractBestFriend(cat, interactionCat, clan, score);
                }
                else if (cat.relations[interactionCat.name] >= 25)
                {
                    catToReturn = InteractMate(cat, interactionCat, clan, score);
                }
                CheckMate(cat, interactionCat);
                return catToReturn;
            }
            return cat;
        }

        public void CheckMate(Cat cat1, Cat cat2)
        {
            if (MateRelations(cat1, cat2) && MateGenders(cat1, cat2) && MateAges(cat1, cat2) && MateGenders(cat1, cat2) && MateFamilyTree(cat1, cat2))
            {
                //Somewhere here check that they are NOT family members once that code exists
                Console.WriteLine(cat1.name + " and " + cat2.name + " have announced their decision to become mates!");
                cat1.relations[cat2.name] = 50;
                cat2.relations[cat1.name] = 50;
                cat1.familyTree.Add(cat2.name, "Mate");
                cat2.familyTree.Add(cat1.name, "Mate");
            }
        }

        public Cat InteractNeutral(Cat cat1, Cat cat2, Clan clan, float interactionScore)
        {
            Random rand = new Random(Utils.GetRandom());
            int interactionChance = (int)Math.Max(2, (1 / interactionScore) + 1);
            int interaction = rand.Next(0, interactionChance);
            if(interaction == 0)
            {
                Console.WriteLine(cat1.name + neutralPositiveStrings[rand.Next(0, neutralPositiveStrings.Length)] + cat2.name + ".");
                
                cat1.relations[cat2.name] += 1;
                if (!cat2.relations.ContainsKey(cat1.name))
                {
                    cat2.relations.Add(cat1.name, 0);

                }
                cat2.relations[cat1.name] += 1;
                return cat1;
            }
            else
            {
                Console.WriteLine(cat1.name + neutralNegativeStrings[rand.Next(0, neutralNegativeStrings.Length)] + cat2.name + ".");
                cat1.relations[cat2.name] -= 1;
                if (!cat2.relations.ContainsKey(cat1.name))
                {
                    cat2.relations.Add(cat1.name, 0);

                }
                cat2.relations[cat1.name] -= 1;
                return cat1;
            }

            
        }

        public Cat InteractFriend(Cat cat1, Cat cat2, Clan clan, float interactionScore)
        {
            string[] friendPositiveArray = neutralPositiveStrings.Concat(friendPositiveStrings).ToArray();
            string[] friendNegativeArray = neutralNegativeStrings.Concat(friendNegativeStrings).ToArray();
            Random rand = new Random(Utils.GetRandom());
            int interactionChance = (int)Math.Max(2, (1 / interactionScore));
            int interaction = rand.Next(0, interactionChance);
            if (interaction == 0)
            {
                Console.WriteLine(cat1.name + friendPositiveArray[rand.Next(0, friendPositiveArray.Length)] + cat2.name + ".");

                cat1.relations[cat2.name] += 1;
                cat2.relations[cat1.name] += 1;
                return cat1;
            }
            else
            {
                Console.WriteLine(cat1.name + friendNegativeArray[rand.Next(0, friendNegativeArray.Length)] + cat2.name + ".");
                cat1.relations[cat2.name] -= 1;
                cat2.relations[cat1.name] -= 1;
                return cat1;
            }


        }

        public Cat InteractBestFriend(Cat cat1, Cat cat2, Clan clan, float interactionScore)
        {
            string[] friendPositiveArray = neutralPositiveStrings.Concat(friendPositiveStrings).ToArray();
            string[] friendNegativeArray = neutralNegativeStrings.Concat(friendNegativeStrings).ToArray();
            string[] bestFriendPositiveArray = friendPositiveArray.Concat(bestFriendPositiveStrings).ToArray();
            string[] bestFriendNegativeArray = friendNegativeArray.Concat(bestFriendNegativeStrings).ToArray();
            Random rand = new Random(Utils.GetRandom());
            int interactionChance = (int)Math.Max(2, (1 / interactionScore));
            int interaction = rand.Next(0, interactionChance);
            if (interaction == 0)
            {
                Console.WriteLine(cat1.name + bestFriendPositiveArray[rand.Next(0, bestFriendPositiveArray.Length)] + cat2.name + ".");

                cat1.relations[cat2.name] += 1;
                cat2.relations[cat1.name] += 1;
                return cat1;
            }
            else
            {
                Console.WriteLine(cat1.name + bestFriendNegativeArray[rand.Next(0, bestFriendNegativeArray.Length)] + cat2.name + ".");
                cat1.relations[cat2.name] -= 1;
                cat2.relations[cat1.name] -= 1;
                return cat1;
            }


        }

        public Cat InteractMate(Cat cat1, Cat cat2, Clan clan, float interactionScore)
        {
            string[] friendPositiveArray = neutralPositiveStrings.Concat(friendPositiveStrings).ToArray();
            string[] friendNegativeArray = neutralNegativeStrings.Concat(friendNegativeStrings).ToArray();
            string[] bestFriendPositiveArray = friendPositiveArray.Concat(bestFriendPositiveStrings).ToArray();
            string[] bestFriendNegativeArray = friendNegativeArray.Concat(bestFriendNegativeStrings).ToArray();
            string[] matePositiveArray = bestFriendPositiveArray.Concat(matePositiveStrings).ToArray();
            string[] mateNegativeArray = bestFriendNegativeArray.Concat(mateNegativeStrings).ToArray();
            Random rand = new Random(Utils.GetRandom());
            int interactionChance = (int)Math.Max(2, (1 / interactionScore));
            int interaction = rand.Next(0, interactionChance);
            if (interaction == 0)
            {
                Console.WriteLine(cat1.name + matePositiveArray[rand.Next(0, matePositiveArray.Length)] + cat2.name + ".");

                cat1.relations[cat2.name] += 1;
                cat2.relations[cat1.name] += 1;
                TryForBaby(cat1, cat2);
                return cat1;
            }
            else
            {
                Console.WriteLine(cat1.name + mateNegativeArray[rand.Next(0, mateNegativeArray.Length)] + cat2.name + ".");
                cat1.relations[cat2.name] -= 1;
                cat2.relations[cat1.name] -= 1;
                return cat1;
            }


        }

        public void TryForBaby(Cat cat1, Cat cat2)
        {
            Random rand = new Random(Utils.GetRandom());
            int chance = rand.Next(0, 6);
            if(chance == 0)
            {
                if(MateRelations(cat1, cat2) && MateGenders(cat1, cat2) && MateAges(cat1, cat2) && MateGenders(cat1, cat2) && MateFamilyTree(cat1, cat2))
                {
                    Cat preg = FindPregnantCat(cat1, cat2);
                    Cat father;
                    if(preg == cat1)
                    {
                        father = cat2;
                    }
                    else
                    {
                        father = cat1;
                    }
                    if(preg.species != null)
                    {
                        Pregnancy(preg, father);
                    }
                }
            }
        }

        

        public Cat FindPregnantCat(Cat cat1, Cat cat2)
        {
            if (cat1.genderAccesor == Defs.Gender.SheCat)
            {
                return cat1;
            }
            else if (cat2.genderAccesor == Defs.Gender.SheCat)
            {
                return cat2;
            }
            else if (cat1.genderAccesor == Defs.Gender.Other && cat2.genderAccesor != Defs.Gender.Other)
            {
                return cat1;
            }
            else if (cat1.genderAccesor != Defs.Gender.Other && cat2.genderAccesor == Defs.Gender.Other)
            {
                return cat2;
            }
            else if (cat1.genderAccesor == Defs.Gender.Other && cat2.genderAccesor == Defs.Gender.Other)
            {
                Random rand = new Random(Utils.GetRandom());
                int mother = rand.Next(0, 2);
                switch (mother)
                {
                    case 0:
                        return cat1;
                    case 1:
                        return cat2;
                }
            }
            return new Cat();
        }

        public void Pregnancy(Cat queen, Cat father)
        {
            if (queen.age <= 70)
            {
                Random rand = new Random(Utils.GetRandom());
                int gest = rand.Next(62, 69);
                queen.pregnancy = gest;
                Console.WriteLine(queen.name + " and " + father.name + " have announced " + queen.name + "'s pregnancy!");
                if(!queen.familyTree.ContainsKey(father.name))
                {
                    queen.familyTree.Add(father.name, "Mate");
                }
                else
                {
                    queen.familyTree[father.name] = "Mate";
                }
                if (!father.familyTree.ContainsKey(queen.name))
                {
                    father.familyTree.Add(queen.name, "Mate");
                }
                else
                {
                    father.familyTree[queen.name] = "Mate";
                }
            }
        }

        public bool MateRelations(Cat cat1, Cat cat2)
        {
            if (cat1.relations[cat2.name] >= 25 && cat2.relations[cat1.name] >= 25)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MateAges(Cat cat1, Cat cat2)
        {
            if (cat1.age >= 10 && cat2.age >= 10)
            {
                return true;
            }
            else return false;
        }

        public bool MateGenders(Cat cat1, Cat cat2)
        {
            Defs.Gender[] genders = { cat1.genderAccesor, cat2.genderAccesor };
            if (genders.Contains(Defs.Gender.Tom) && genders.Contains(Defs.Gender.SheCat))
            {
                return true;
            }
            else if (genders.Contains(Defs.Gender.Other) && genders.Contains(Defs.Gender.SheCat))
            {
                return true;
            }
            else if (genders.Contains(Defs.Gender.Tom) && genders.Contains(Defs.Gender.Other))
            {
                return true;
            }
            else if (genders.Contains(Defs.Gender.Other) && genders.Contains(Defs.Gender.Other))
            {
                return true;
            }
            else return false;
        }

        public bool MateRanks(Cat cat1, Cat cat2)
        {
            if (cat1.rankAccesor == Defs.Rank.Apprentice && cat2.rankAccesor == Defs.Rank.Apprentice)
            {
                return true;
            }
            else if ((cat1.rankAccesor != Defs.Rank.Apprentice && cat2.rankAccesor != Defs.Rank.Apprentice))
            {
                return true;
            }
            else return false;
        }

        public bool MateFamilyTree(Cat cat1, Cat cat2)
        {
            if(cat1.familyTree.ContainsKey(cat2.name) && cat2.familyTree.ContainsKey(cat1.name))
            {
                if (cat1.familyTree[cat2.name] == "Child" || cat2.familyTree[cat1.name] == "Child")
                {
                    if (cat1.familyTree[cat2.name] == "Father" || cat2.familyTree[cat1.name] == "Father")
                    {
                        if (cat1.familyTree[cat2.name] == "Mother" || cat2.familyTree[cat1.name] == "Mother")
                        {
                            if (cat1.familyTree[cat2.name] == "Littermate" || cat2.familyTree[cat1.name] == "Littermate")
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
            
        }
    }
}
