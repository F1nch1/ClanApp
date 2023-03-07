using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClanCreatorApp
{
    public struct Biome
    {
        
        public string name;
        public string[] preyTypes;
        public string description;
        public Char character;

        public Biome(string nom, string[] types, string desc, Char c)
        {
            name = nom;
            preyTypes = types;
            description = desc;
            character = c;
        }
    }
}
