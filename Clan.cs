using System;
using System.Collections.Generic;
using System.Text;

namespace ClanCreatorApp
{
    public struct Clan
    {
        public string name;
        public List<FreshKill> freshKillPile;
        public Biome biome;
        public List<Cat> clanCats;
        public List<Item> items;

        public Clan(string nom, Biome bio, List<Cat> list, List<FreshKill> freshKillList, List<Item> itemList)
        {
            name = nom;
            freshKillPile = freshKillList;
            biome = bio;
            clanCats = list;
            items = itemList;
        }
    }
}
