using System;
using System.Collections.Generic;
using System.Text;

namespace ClanCreatorApp
{
    class Biomes
    {

        public static Biome[] biomes = new Biome[]
        {
            new Biome("Desert", new string[] { "Adder", "Wren", "Mouse", "Rat", "Lizard", "Beetle", "Wood Mouse"}, "A dry area consisting of sparse grasses and parched soil. However, a variety of fauna persist, and rock formations provide protection from harsh desert winds.", '-'),
            new Biome("Broadleaf Forest", new string[] { "Starling", "Magpie", "Mouse", "Vole", "Shrew", "Robin", "Squirrel", "Thrush"}, "A dense forested area with a thick, broad-leafed canopy and a thick layer of shrubs underfood, providing a habitat for a great deal of wildlife.", '%'),
            new Biome("Coniferous Forest", new string[] { "Squirrel", "Mouse", "Vole", "Shrew", "Hawk", "Pigeon", "Dove", "Finch" }, "A forested area characterized by the large trees that it consists of, experiencing large amounts of rainfall and relatively mild winters which allow for a great variety of wildlife to thrive.", '@' ),
            new Biome("Boreal Forest", new string[] {"Rabbit", "Squirrel", "Hare", "Vole", "Pigeon", "Dove", "Starling"}, "A forested area in a colder climate, experiencing frequent snowfall that allows for a lesser density of undergrowth for prey to hide in.", '?'),
            new Biome("Shrubland", new string[] { "Rabbit", "Hare", "Vole", "Shrew", "Starling", "Pigeon", "Thrush", "Blackbird", "Chicken"}, "A large, open environment with flat, uneven grasslands and low shrubs that serve as habitats for all sorts of prey animals.", '='),
            new Biome("Moor", new string[] { "Rabbit", "Hare", "Vole", "Pheasant", "Wagtail", "Chicken", "Wood Mouse", "Rat", "Falcon", "Moorhen"}, "A vast, rolling expanse of grassy hills, bountiful in prey but less than sheltered from harsh winds.", '¬'),
            new Biome("Marsh", new string[] { "Crow", "Water Vole", "Wood Mouse", "Trout", "Carp", "Minnow", "Chub", "Rat", "Thrush", "Beetle"}, "An area consisting of sparse tree cover, dense undergrowth, and considerable areas of wetland, in which some are deep enough to swim and fish.", '<'),
            new Biome("Mountain", new string[] {"Falcon", "Eagle", "Hawk", "Mouse", "Rat", "Magpie", "Pigeon", "Dove"}, "A great mountain, littered with caves for cats to rest their heads. Prey is scarce, but that which does roam the mountains can keep a clan fed for days-- Provided they are brave enough to bring it down.", '^')
        };

        public static Dictionary<string, Biome> biomeDict = new Dictionary<string, Biome>()
        {
            {"Desert", biomes[0] },
            {"Broadleaf Forest", biomes[1] },
            {"Coniferous Forest", biomes[2] },
            {"Boreal Forest", biomes[3] },
            {"Shrubland", biomes[4] },
            {"Moor", biomes[5] },
            {"Marsh", biomes[6] },
            {"Mountain", biomes[7] }
        };
    }
}
