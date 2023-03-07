namespace ClanCreatorApp
{
    class StatBlocks
    {
        public static StatBlock loner = new StatBlock(2, 0, -1, 1, -2, 1);
        public static StatBlock housePet = new StatBlock(-1, 0, -1, 1, 1, 1);
        public static StatBlock stray = new StatBlock(1, 0, 1, -1, -1, 1);
        public static StatBlock rouge = new StatBlock(-1, 2, 0, -2, 1, 1);
        public static StatBlock kit = new StatBlock(-5, -5, -5, -5, -5, -5);

        public static StatBlock[] origins = { loner, housePet, stray, rouge };
        public static StatBlock[] allOrigins = { kit };
    }
}
