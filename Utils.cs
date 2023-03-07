using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace ClanCreatorApp
{
    public static class Utils
    {
        private static Random rnd = new Random();
        public static int GetRandom()
        {
            return rnd.Next();
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (TextReader sr = new StringReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (sr.Peek() != -1)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }
        public static int GetColumn(string trait, DataTable table)
        {
            switch(trait)
            {
                case "Just":
                    return 1;
                case "Assertive":
                    return 2;
                case "Bad-Tempered":
                    return 3;
                case "Nervous":
                    return 4;
                case "Carefree":
                    return 5;
                case "Kind":
                    return 6;
                case "Clever":
                    return 7;
                default:
                    return -1;
            }
        }

        public static int GetRow(string trait, DataTable table)
        {
            switch (trait)
            {
                case "Just":
                    return 0;
                case "Assertive":
                    return 1;
                case "Bad-Tempered":
                    return 2;
                case "Nervous":
                    return 3;
                case "Carefree":
                    return 4;
                case "Kind":
                    return 5;
                case "Clever":
                    return 6;
                default:
                    return -1;
            }
        }
        public static string CreateList(string[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                if (i != array.Length - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }

        public static int FindWounds(Cat cat, Stats s)
        {
            int totalWounds = 0;
            for(int i = 0; i < cat.wounds.Count; i++)
            {
                if(Wounds.woundDict[cat.wounds[i].type].stat == s)
                {
                    totalWounds += cat.wounds[i].counter;
                }
            }
            return totalWounds;
        }

        public static float GetStats(Entity enti, Stats s)
        {
            if(enti is Cat)
            {
                Cat cat = (Cat)enti;
                switch (s)
                {
                    case Stats.Agility:
                        float agi = 0;
                        agi = enti.statBlock.agility - FindWounds(cat, Stats.Agility);
                        return agi;
                    case Stats.Constitution:
                        float con = 0;
                        con = enti.statBlock.constitution - FindWounds(cat, Stats.Constitution);
                        return con;
                    case Stats.Finesse:
                        float fin = 0;
                        fin = enti.statBlock.finesse - FindWounds(cat, Stats.Finesse);
                        return fin;
                    case Stats.Intelligence:
                        float intl = 0;
                        intl = enti.statBlock.intelligence - FindWounds(cat, Stats.Intelligence);
                        return intl;
                    case Stats.Strength:
                        float str = 0;
                        str = enti.statBlock.strength - FindWounds(cat, Stats.Strength);
                        return str;
                    case Stats.Tenacity:
                        float ten = 0;
                        ten = enti.statBlock.tenacity - FindWounds(cat, Stats.Tenacity);
                        return ten;
                }
            }
            else
            {
                switch (s)
                {
                    case Stats.Agility:
                        return enti.statBlock.agility;
                    case Stats.Constitution:
                        return enti.statBlock.constitution;
                    case Stats.Finesse:
                        return enti.statBlock.finesse;
                    case Stats.Intelligence:
                        return enti.statBlock.intelligence;
                    case Stats.Strength:
                        return enti.statBlock.strength;
                    case Stats.Tenacity:
                        return enti.statBlock.tenacity;
                }
            }
            
            return 0f;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static bool CheckVowel(Char c)
        {
            
            Char[] vowelList =
            {
                'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U'
            };
            if (vowelList.Contains(c))
            {
                return true;
            }
            return false;
        }

        public static Cat GetCatInClan(Clan clan, string name)
        {
            for(int i = 0; i < clan.clanCats.Count; i++)
            {
                if(clan.clanCats[i].name == name)
                {
                    return clan.clanCats[i];
                }
            }
            return null;
        }
        
    }

    
}
