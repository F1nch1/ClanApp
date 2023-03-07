using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    [Serializable]
    public class Item
    {
        public string type;
        public string description;
        public bool usable;
    }
    [Serializable]
    public class Herb : Item
    {
        public string[] treatmentOptions;

        public Herb(string t, string d, string[] tos, bool u)
        {
            type = t;
            description = d;
            treatmentOptions = tos;
            usable = u;
        }
    }

    public static class Herbs
    {
        public static Herb[] herbs =
        {
            new Herb("Bindweed", "Blue petals with white throat and yellow center.", new string[] {"Broken Leg"}, true),
            new Herb("Borage", "A plant with small blue or pink star-shaped leaves and a hairy stem.", new string[] {"Fever", "Bellyache"}, true),
            new Herb("Broom", "Small yellow flowers collected from shrubs.", new string[] {"Broken Leg", "Abrasion", "Laceration", "Contusion", "Cut"}, true),
            new Herb("Catmint", "A tall, leafy, and delicious-smelling plant.", new string[] {"Greencough", "Whitecough"}, true),
            new Herb("Chervil", "A sweet-smelling plant with large, leafy, fern-like leaves and small white flowers.", new string[] {"Bellyache", "Infection" }, true),
            new Herb("Cobweb", "Long, thin, shiny strands spun into a web by spiders.", new string[] { "Abrasion", "Laceration", "Contusion", "Cut", "Stab", "Puncture", "Bite", "Slash", "Slice" }, true),
            new Herb("Comfrey", "A herb with large leaves and a tangy smell.", new string[] {"Sore", "Arthritis"}, true),
            new Herb("Coltsfoot", "A flowering plant resembling a dandelion.", new string[] { "Sore"}, true),
            new Herb("Daisy Leaves", "The leaves of a daisy", new string[] {"Arthritis"}, true),
            new Herb("Dock Leaves", "Large, dark green leaves with a tangy smell and taste.", new string[] {"Sore", "Abrasion", "Slash", "Slice" }, true),
            new Herb("Elder Leaves", "Leaves from the elder tree.", new string[] { "Sprain"}, true),
            new Herb("Feverfew", "Small bush with flowers resembling daisies.", new string[] {"Fever"}, true),
            new Herb("Goldenrod", "A tall plant with bright, yellow flowers.", new string[] { "Abrasion", "Laceration", "Contusion", "Cut"}, true),
            new Herb("Honey", "A sweet, golden-colored liquid made by bees.", new string[] { "Infection"}, true),
            new Herb("Horsetail", "A tall, bristly-stemmed plant.", new string[] { "Infection"}, true),
            new Herb("Juniper Berries", "Purple-blue berries from the dark green, spiky-leaved juniper bush.", new string[] {"Bellyache"}, true),
            new Herb("Mallow Leaves", "Large fuzzy three-nubbed leaves from a flowering shrub.", new string[] {"Bellyache"}, true),
            new Herb("Marigold", "A low-growing flower; yellow to bright orange.", new string[] {"Infection"}, true),
            new Herb("Parsley", "A long-stemmed plant with ragged-edged crinkly leaves.", new string[] {"Bellyache"}, true),
            new Herb("Ragwort Leaves", "Tall shrub with yellow flowers.", new string[] {"Arthritis"}, true),
            new Herb("Rushes", "It has long narrow leaves and lavender-colored head stalks.", new string[] {"Broken Leg"}, true),
            new Herb("Stinging Nettle", "It has green, spiny seeds.", new string[] {"Poisoning"}, true),
            new Herb("Sweet Sedge", "Thick green stem with long buds at the top.", new string[] {"Infection"}, true),
            new Herb("Tansy", "The tansy plant has round, yellow flowers.", new string[] {"Sore Throat", "Poisoning"}, true),
            new Herb("Yarrow", "A flowering plant with green, jagged leaves, a tangy scent and a bitter taste.", new string[] {"Poisoning", "Sore"}, true)
        };

        public static Dictionary<string, Herb> herbDict = new Dictionary<string, Herb>()
        {
            {"Bindweed", herbs[0] },
            {"Borage", herbs[1] },
            {"Broom", herbs[2] },
            {"Catmint", herbs[3] },
            {"Chervil", herbs[4] },
            {"Cobweb", herbs[5] },
            {"Comfrey", herbs[6] },
            {"Coltsfoot", herbs[7] },
            {"Daisy Leaves", herbs[8] },
            {"Dock Leaves", herbs[9] },
            {"Elder Leaves", herbs[10] },
            {"Feverfew", herbs[11] },
            {"Goldenrod", herbs[12] },
            {"Honey", herbs[13] },
            {"Horsetail", herbs[14] },
            {"Juniper Berries", herbs[15] },
            {"Mallow Leaves", herbs[16] },
            {"Marigold", herbs[17] },
            {"Parsley", herbs[18] },
            {"Ragwort Leaves", herbs[19] },
            {"Rushes", herbs[20] },
            {"Stinging Nettle", herbs[21] },
            {"Sweet Sedge", herbs[22] },
            {"Tansy", herbs[23] },
            {"Yarrow", herbs[23] },
        };
    }
}
