using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClanCreatorApp
{
    public static class Defs
    {
        public enum Rank
        {
            Kit,
            Apprentice,
            Warrior,
            Elder,
            Deputy,
            MedicineCat,
            Leader,
            Loner,
            LonerLeader,
            Queen
        }

        public enum Gender
        {
            SheCat,
            Tom,
            Other
        }

        public static List<Rank> eatingOrderLoner = new List<Rank> { Rank.Queen, Rank.Kit, Rank.LonerLeader, Rank.Loner, Rank.Elder, Rank.MedicineCat, Rank.Warrior, Rank.Apprentice, Rank.Deputy, Rank.Leader };

        public enum FamilyTree
        {
            Mother,
            Father,
            Child,
            Mate,
            Littermate
        }

        public static Dictionary<FamilyTree, string> familyDict = new Dictionary<FamilyTree, string>()
        {
            {FamilyTree.Mother, "Mother" },
            {FamilyTree.Father, "Father" },
            {FamilyTree.Child, "Child" },
            {FamilyTree.Mate, "Mate" },
            {FamilyTree.Littermate, "Littermate" }
        };

        public static Dictionary<string, FamilyTree> dictFamily = new Dictionary<string, FamilyTree>()
        {
            {"Mother", FamilyTree.Mother },
            {"Father", FamilyTree.Father },
            {"Child",FamilyTree.Child },
            {"Mate", FamilyTree.Mate },
            {"Littermate", FamilyTree.Littermate },
        };

        public static Dictionary<Rank, string> rankDict = new Dictionary<Rank, string>()
        {
            {Rank.Kit, "Kit" },
            {Rank.Apprentice, "Apprentice"},
            {Rank.Warrior, "Warrior"},
            {Rank.Elder, "Elder"},
            {Rank.Deputy, "Deputy"},
            {Rank.Leader, "Leader"},
            {Rank.Loner, "Loner" },
            {Rank.MedicineCat, "Medicine Cat" },
            {Rank.LonerLeader, "Loner Leader" },
            {Rank.Queen, "Queen"}
        };

        public static Rank[] catsForPatrol = { Rank.Apprentice, Rank.Deputy, Rank.Leader, Rank.Loner, Rank.Warrior, Rank.LonerLeader };

        public static Dictionary<Gender, string> genderDict = new Dictionary<Gender, string>()
        {{Gender.Tom, "Tom" },
            {Gender.SheCat, "She-Cat"},
            
            {Gender.Other, "Other" }
        };

        public static Dictionary<string, Rank> dictRank = new Dictionary<string, Rank>()
        {
            { "Kit", Rank.Kit},
            {"Apprentice", Rank.Apprentice },
            {"Warrior", Rank.Warrior},
            {"Elder", Rank.Elder },
            {"Deputy", Rank.Deputy },
            {"Leader", Rank.Leader },
            {"Loner", Rank.Loner },
            {"Loner Leader", Rank.LonerLeader }
        };

        public static Dictionary<string, Gender> dictGender = new Dictionary<string, Gender>()
        {
            {"She-Cat", Gender.SheCat },
            {"Tom", Gender.Tom },
            {"Other", Gender.Other }
        };

        public static Dictionary<string, string> traitDict = new Dictionary<string, string>()
        {
            {"Loyal", "Just" },
            {"Strong", "Just" },
            {"Confident", "Just" },
            {"Honest", "Just" },
            {"Brave", "Just" },
            {"Driven", "Assertive" },
            {"Insistent", "Assertive" },
            {"Rough", "Assertive" },
            {"Arrogant", "Assertive" },
            {"Loud", "Assertive" },
            {"Outspoken", "Assertive" },
            {"Strict", "Assertive" },
            {"Grumpy", "Bad-Tempered" },
            {"Snappy", "Bad-Tempered" },
            {"Power-Hungry", "Bad-Tempered" },
            {"Hot-Headed", "Bad-Tempered" },
            {"Bully", "Bad-Tempered" },
            {"Scornful", "Bad-Tempered" },
            {"Timid", "Nervous" },
            {"Quiet", "Nervous" },
            {"Submissive", "Nervous" },
            {"Careful", "Nervous" },
            {"Placid", "Nervous" },
            {"Relaxed", "Carefree" },
            {"Lazy", "Carefree" },
            {"Blunt", "Carefree" },
            {"Dramatic", "Carefree" },
            {"Curious", "Carefree" },
            {"Adventurous", "Carefree" },
            {"Sociable", "Carefree" },
            {"Soft", "Kind" },
            {"Parental", "Kind" },
            {"Cheerful", "Kind" },
            {"Patient", "Kind" },
            {"Observant", "Kind" },
            {"Sweet", "Kind" },
            {"Deceptive", "Clever" },
            {"Sarcastic", "Clever" },
            {"Careless", "Clever" },
            {"Brutal", "Clever" },
            {"Selfish", "Clever" },
            {"Demanding", "Clever" }
        };

        public static string[] GetRandomPersonality()
        {
            List<string> traits = new List<string>();
            while(traits.Distinct().ToList().Count != 2)
            {
                traits = traits.Distinct().ToList();
                Random rand = new Random();
                int num = rand.Next(0, traitDict.Keys.Count);
                traits.Add(traitDict.Keys.ElementAt(num));
            }
            return traits.ToArray();
            
        }

        public static string GetPersonalityType(string p)
        {
            return traitDict[p];
        }

        public static string GetRandomName()
        {
            Random rand = new Random();
            int num = rand.Next(0, Names.NonClanNames.Length);
            return Names.NonClanNames[num];
        }

        public static Defs.Gender GetRandomGender()
        {
            Random rand = new Random();
            int num = rand.Next(0, genderDict.Keys.Count);
            return genderDict.Keys.ElementAt(num);
        }

        public static int GetRandomAgeAdult()
        {
            Random rand = new Random();
            int num = rand.Next(6, 120);
            return num;
        }
    }
}
