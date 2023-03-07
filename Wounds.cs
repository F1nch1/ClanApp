using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    [Serializable]
    public struct Wound
    {
        public int counter;
        public string type;
        public string location;
        public bool infectionTreated;
        public bool treated;

        public Wound(int c, string t, string l)
        {
            counter = c;
            type = t;
            location = l;
            infectionTreated = false;
            treated = false;
        }
    }

    public struct WoundType
    {
        public string type;
        public Stats stat;
        public string location;

        public WoundType(string t, Stats s, string l = null)
        {
            type = t;
            stat = s;
            location = l;
        }
    }
    public static class Wounds
    {
        public static string[] locations =
        {
            "Forehead",
            "Head",
            "Left Cheek",
            "Right Cheek",
            "Right Eye",
            "Left Eye",
            "Left Ear",
            "Right Ear",
            "Nose",
            "Lip",
            "Jaw",
            "Chin",
            "Chest",
            "Shoulder",
            "Neck",
            "Upper Back",
            "Lower Back",
            "Right Foreleg",
            "Right Front Paw",
            "Left Foreleg",
            "Left Front Paw",
            "Belly",
            "Right Flank",
            "Right Hip",
            "Right Hindleg",
            "Right Hind Paw",
            "Left Flank",
            "Left Hip",
            "Left Hind Paw",
            "Left Flank",
            "Left Hind Paw",
            "Croup",
            "Tail"
        };

        public static WoundType[] woundTypes =
        {
            new WoundType("Abrasion", Stats.Finesse),
            new WoundType("Laceration", Stats.Strength),
            new WoundType("Contusion", Stats.Agility),
            new WoundType("Stab", Stats.Strength),
            new WoundType("Bruise", Stats.Strength),
            new WoundType("Puncture", Stats.Agility),
            new WoundType("Bite", Stats.Strength),
            new WoundType("Slash", Stats.Strength),
            new WoundType("Slice", Stats.Strength),
            new WoundType("Cut", Stats.Strength),
            new WoundType("Sore", Stats.Agility),
            new WoundType("Broken Leg", Stats.Agility, "Right Foreleg"),
            new WoundType("Broken Leg", Stats.Agility, "Right Hindleg"),
            new WoundType("Broken Leg", Stats.Agility, "Left Foreleg"),
            new WoundType("Broken Leg", Stats.Agility, "Left Hindleg"),
            new WoundType("Sprain", Stats.Agility, "Right Foreleg"),
            new WoundType("Sprain", Stats.Agility, "Right Hindleg"),
            new WoundType("Sprain", Stats.Agility, "Left Foreleg"),
            new WoundType("Sprain", Stats.Agility, "Left Hindleg"),
        };


        public static WoundType concussion = new WoundType("Concussion", Stats.Intelligence, "Head");

        public static Dictionary<string, WoundType> woundDict = new Dictionary<string, WoundType>()
        {
            {"Abrasion", woundTypes[0] },
            {"Laceration", woundTypes[1] },
            {"Contusion", woundTypes[2] },
            {"Stab", woundTypes[3] },
            {"Bruise", woundTypes[4] },
            {"Puncture", woundTypes[5] },
            {"Bite", woundTypes[6] },
            {"Slash", woundTypes[7] },
            {"Slice", woundTypes[8] },
            {"Cut", woundTypes[9] },
            {"Sore", woundTypes[10] },
            {"Broken Leg", woundTypes[11] },
            {"Sprain", woundTypes[16] }
        };

        public static Wound? WoundCheck(float dam)
        {
            int damage = (int)dam;
            Random rand = new Random(Utils.GetRandom());
            int chance = (int)Math.Pow(damage, 2);
            if (chance >= 100)
            {
                Wound w = new Wound();
                w.counter = damage;
                w.type = GetRandomType();
                w.location = GetRandomLocation();
                if(woundDict[w.type].location != null)
                {
                    w.location = woundDict[w.type].location;
                }
                return w;
            }
            int num = rand.Next(0, 100);
            if (chance <= num)
            {
                Wound w = new Wound();
                w.counter = damage;
                w.type = GetRandomType();
                w.location = GetRandomLocation();
                if (woundDict[w.type].location != null)
                {
                    w.location = woundDict[w.type].location;
                }

                return w;
            }
            else
            {
                return null;
            }
        }

        

        public static Wound CreateWound(int count, WoundType type)
        {
            Wound w = new Wound();
            w.counter = count;
            w.type = type.type;
            if(w.location != null)
            {
                w.location = type.location;
            }
            else
            {
                w.location = GetRandomLocation();
            }
            return w;
        }

        public static Wound CreateWound(int count)
        {
            Wound w = new Wound();
            w.counter = count;
            w.type = GetRandomType();

            w.location = GetRandomLocation();
            
            return w;
        }

        public static string GetRandomLocation()
        {
            Random rand = new Random(Utils.GetRandom());
            int r = rand.Next(0, locations.Length);
            return locations[r];
        }

        public static string GetRandomType()
        {
            Random rand = new Random(Utils.GetRandom());
            int r = rand.Next(0, woundTypes.Length);
            return woundTypes[r].type;
        }
    }
}
