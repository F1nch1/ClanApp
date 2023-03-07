using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    [Serializable]
    public class Disease
    {
        public int counter;
        public string type;
        public bool treated;

        public Disease(int c, string s)
        {
            counter = c;
            type = s;
            treated = false;
        }
    }

    public class DiseaseType
    {
        public string type;
        public string[] actions;
        //Threshold of getting worse
        public int untreatedThreshold;
        public int treatedThreshold;

        public DiseaseType(string t, string[] a, int ut, int tt)
        {
            type = t;
            actions = a;
            untreatedThreshold = ut;
            treatedThreshold = tt;
        }
    }
    public static class Diseases
    {
        public static DiseaseType[] randomDiseases =
        {
            new DiseaseType("Greencough", respiratoryStrings, 70, 20),
            new DiseaseType("Whitecough", respiratoryStrings, 40, 10),
            new DiseaseType("Bellyache", gastrointestnalStrings, 80, 5),
            new DiseaseType("Fever", feverStrings, 50, 10),
            new DiseaseType("Sore Throat", soreThroatStrings, 10, 2),
        };

        public static DiseaseType[] diseases =
        {
            new DiseaseType("Greencough", respiratoryStrings, 70, 20),
            new DiseaseType("Whitecough", respiratoryStrings, 40, 10),
            new DiseaseType("Bellyache", gastrointestnalStrings, 80, 5),
            new DiseaseType("Fever", feverStrings, 50, 10),
            new DiseaseType("Sore Throat", soreThroatStrings, 10, 2),
            new DiseaseType("Poisoning", gastrointestnalStrings, 50, 5),
            new DiseaseType("Arthritis", arthritisStrings, 30, 5),
            new DiseaseType("Infection", infectionStrings, 70, 15)
        };


        public static Dictionary<string, DiseaseType> diseaseDict = new Dictionary<string, DiseaseType>()
        {
            {"Greencough", diseases[0]},
            {"Whitecough", diseases[1]},
            {"Bellyache", diseases[2]},
            {"Fever", diseases[3]},
            {"Sore Throat", diseases[4]},
            {"Poisoning", diseases[5] },
            {"Arthritis", diseases[6] },
            {"Infection", diseases[7] }
        };


        public static string[] infectionStrings =
        {
            " vomited up everything they ate.",
            " has awful cramps in their stomach.",
            " doesn't want to eat anything.",
            " has awfully hot ears today.",
            " keeps rubbing their nose.",
            " is breathing awfully quickly.",
            "'s voice sounds odd today.",
            " doesn't want to do anything today."
        };


        public static string[] respiratoryStrings =
        {
            " wakes up their denmate by coughing.",
            " coughs through the night.",
            " has an awful, scratchy cough.",
            " can't seem to stop coughing.",
            " has snot dripping from their nose.",
            " has weepy eyes."
        };

        public static string[] gastrointestnalStrings =
        {
            " vomited up everything they ate.",
            " has awful cramps in their stomach.",
            " doesn't want to eat anything."
        };

        public static string[] feverStrings =
        {
            " has awfully hot ears today.",
            " keeps rubbing their nose.",
            " is breathing awfully quickly.",
            "'s voice sounds odd today.",
            " doesn't want to do anything today."
        };

        public static string[] arthritisStrings =
        {
            " has an odd limp today.",
            " doesn't want to get up.",
            " is complaining of pain in the joints."
        };

        public static string[] soreThroatStrings =
        {
            " has a gravelly voice today.",
            " keeps coughing.",
            " complains of pain when swallowing"
        };

    }
}
