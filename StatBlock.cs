using System;
using System.Collections.Generic;
namespace ClanCreatorApp
{
    [Serializable]
    public struct Stat
    {
        public float value;

        public Stat(float val)
        {
            value = val;
        }
    }
    [Serializable]
    public enum Stats
    {
        Agility,
        Constitution,
        Finesse,
        Intelligence,
        Strength,
        Tenacity
    }
    [Serializable]
    public struct  StatBlock
    {

        public float agility;
        public float constitution;
        public float finesse;
        public float intelligence;
        public float strength;
        public float tenacity;

        public Stat agiStat;
        public Stat conStat;
        public Stat finStat;
        public Stat intStat;
        public Stat strStat;
        public Stat tenStat;
        

        //Initializer
        public StatBlock(float agi, float str, float intl, float fin, float ten, float con)
        {
            agiStat = new Stat(agi);
            strStat = new Stat(str);
            intStat = new Stat(intl);
            finStat = new Stat(fin);
            tenStat = new Stat(ten);
            conStat = new Stat(con);

            agility = agi;
            strength = str;
            intelligence = intl;
            finesse = fin;
            tenacity = ten;
            constitution = con;
        }
    }
}
