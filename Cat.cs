using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClanCreatorApp
{
    [Serializable]
    
    public class Cat : Entity
    {
        //Bio
        public string name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(prefix) && !string.IsNullOrWhiteSpace(suffix))
                {
                    return (prefix + suffix);
                }
                if (string.IsNullOrWhiteSpace(suffix))
                {
                    return prefix;
                }
                else
                {
                    return "error";
                }
            }
        }
        
        
        public int age { get; set; }
        public string rank
        {
            get { return Defs.rankDict[rankAccesor]; }
        }
        public string gender
        {
            get { return Defs.genderDict[genderAccesor]; }
        }
        public string prefix { get; set; }
        public string suffix { get; set; }

        //Accesors
        public Defs.Rank rankAccesor { get; set; }
        public Defs.Gender genderAccesor { get; set; }

        public int energy;
        public string[] personality;

        public SerializableDictionary<string, int> relations = new SerializableDictionary<string, int>();

        public List<Wound> wounds;

        public List<Disease> diseases;

        public Skills skills;

        public int pregnancy;

        public SerializableDictionary<string, string> familyTree = new SerializableDictionary<string, string>();

        //Initializer

        public Cat()
        {

        }
        public Cat(string pre, string suf, Defs.Rank rankAssigner, Defs.Gender genderAssigner, int ageAssigner, StatBlock stats, string[] p, Skills ss, int en = -1, float h = -1, SerializableDictionary<string, int> rs = null, List<Wound> ws = null, List<Disease> ds = null, int preg = -1, SerializableDictionary<string, string> ft = null)
        {
            prefix = pre;
            suffix = suf;
            species = "Cat";
            rankAccesor = rankAssigner;
            genderAccesor = genderAssigner;
            age = ageAssigner;
            statBlock = stats;
            skills = ss;
            if(h == -1)
            {
                if(stats.constitution >= 1)
                {
                    health = stats.constitution * 5;
                }
                else
                {
                    health = 5;
                }
            }
            else
            {
                health = h;
            }
            combatDeck.Add(new CombatMove("Claw", 1, Stats.Strength));
            if(en != -1)
            {
                energy = en;
            }
            else
            {
                float oneOrTen = Math.Max(1, stats.tenacity);
                energy = (int)Math.Floor(oneOrTen * 2.5f);
            }
            personality = p;
            if(rs == null)
            {
                relations = new SerializableDictionary<string, int>();
            }
            else
            {
                relations = rs;
            }
            if (ft == null)
            {
                familyTree = new SerializableDictionary<string, string>();
            }
            else
            {
                familyTree = ft;
            }
            if (ws == null)
            {
                wounds = new List<Wound>();
            }
            else
            {
                wounds = ws;
            }
            if (ds == null)
            {
                diseases = new List<Disease>();
            }
            else
            {
                diseases = ds;
            }
            if(preg != -1)
            {
                pregnancy = preg;
            }
            else
            {
                pregnancy = -1;
            }
        }

    }
}
