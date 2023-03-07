using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Xml.Serialization;

namespace ClanCreatorApp
{
    public class MainFile
    {
        bool playing = true;
        bool debug = false;
        public static string[] quitInputs = { "x", "X", "quit", "Quit" };
        public static string[] yesInputs = { "yes", "Yes", "y", "Y", "ye", "Ye", "yeah", "Yeah" };
        public static string[] noInputs = { "no", "No", "n", "N", "nah", "Nah", "nope", "Nope" };
        Cat temporaryCat;
        Cat leader;
        Clan playerClan;
        List<Biome> biomeChoices = new List<Biome>();
        string fileName;
        WorldStatus activeWorld;
        Map map;
        string[,] strings;
        public static DataTable interactionData;
        public Patrol activePatrol;
        public bool patrolling;
        static void Main(string[] args)
        {
            MainFile program = new MainFile();
            program.MainLoop();
            //program.DataTest(); 
        }

        public DataTable LoadData()
        {
            DataTable dt = Utils.ConvertCSVtoDataTable(ClanCreatorApp.Properties.Resources.Interactions);
            return dt;
        }

        public void WoundTest()
        {
            Skills skills = new Skills(0, 0);
            Cat testCat1 = (new Cat("Leaf", "", Defs.Rank.Kit, Defs.Gender.Other, 5, StatBlocks.origins[0], Defs.GetRandomPersonality(), skills));
            Wound? w = Wounds.WoundCheck(10);
            if (w != null)
            {
                Wound wound = (Wound)w;
                testCat1.wounds.Add(wound);
                Console.WriteLine(wound.type + "\n" + wound.location + "\n");
            }

        }

        public void DiseaseTest()
        {
            Skills skills = new Skills(0, 0);
            Cat testCat1 = (new Cat("Leaf", "", Defs.Rank.Kit, Defs.Gender.Other, 5, StatBlocks.origins[0], Defs.GetRandomPersonality(), skills));
            testCat1.diseases.Add(new Disease(5, Diseases.diseases[0].type));
        }

        public void MainLoop()
        {
            interactionData = LoadData();
            while (playing)
            {
                
                if(debug)
                {
                    int debug = DebugScreen();
                    switch (debug)
                    {
                        case 0:
                            Console.Clear();
                            Setup();
                            break;
                        case 1:
                            Console.Clear();
                            Load();
                            if (playerClan.clanCats.Count > 0)
                            {
                                CampLoop loop = new CampLoop();
                                if(activePatrol.patrolCats != null)
                                {
                                    loop.MainCampLoop(playerClan, activeWorld, map, fileName, activePatrol);
                                }
                                else
                                {
                                    loop.MainCampLoop(playerClan, activeWorld, map, fileName, new Patrol());
                                }

                            }
                            else
                            {
                                Console.WriteLine("This is an empty place.");
                            }
                            break;
                        case 2:
                            Console.Clear();
                            break;
                        case 3:
                            Console.Clear();
                            Load();
                            break;
                        case 4:
                            WoundTest();
                            break;
                        default:
                            Console.Clear();
                            Setup();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    OpeningScreen();
                    //Load();
                    if (playerClan.clanCats != null)
                    {
                        CampLoop loop = new CampLoop();


                        if (activePatrol.patrolCats != null)
                        {
                            loop.MainCampLoop(playerClan, activeWorld, map, fileName, activePatrol);
                        }
                        
                        else
                        {
                            loop.MainCampLoop(playerClan, activeWorld, map, fileName, new Patrol());
                        }
                        activePatrol = new Patrol();
                    }
                }
            }
        }

        public void PersuasionTest()
        {
            Skills skills = new Skills(0, 0);
            Cat testCat1 = (new Cat("Leaf", "", Defs.Rank.Kit, Defs.Gender.Other, 5, StatBlocks.origins[0], Defs.GetRandomPersonality(), skills));
            Cat testCat2 = (new Cat("Leaf", "", Defs.Rank.Kit, Defs.Gender.Other, 5, StatBlocks.origins[0], Defs.GetRandomPersonality(), skills));
            Clan clan = new Clan();
            clan.clanCats = new List<Cat>();
            Persuasion per = new Persuasion();
            per.PersuadeCat(testCat1, testCat2, clan, interactionData);
        }
        public void OpeningScreen()
        {
            string output = "";
            output += "[0]: New Game\n[1]: Load Game\n[2]: Delete All Saves";
            string checkOpen = "";
            Console.WriteLine(output);
            while (!checkOpen.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkOpen) || int.Parse(checkOpen) > 2)
            {
                checkOpen = Console.ReadLine();
                CheckQuit(checkOpen);
            }
            int num = int.Parse(checkOpen);
            switch(num)
            {
                case 0:
                    Setup();
                    break;
                case 1:
                    Load();
                    //Console.WriteLine(ClanScreen());
                    break;
                case 2:
                    DeleteAllFiles();
                    break;
            }
        }

        public void DeleteAllFiles()
        {
            
            string input = "";
            while (!yesInputs.Contains(input) && !noInputs.Contains(input))
            {
                Console.WriteLine("Are you sure you want to delete all save files?");
                input = Console.ReadLine();
                CheckQuit(input);

                if (yesInputs.Contains(input))
                {
                    Console.WriteLine("Deleting files.");
                    string folder = Directory.GetCurrentDirectory();
                    string pathSave = Path.Combine(folder, "Saves.csv");
                    File.Delete(pathSave);
                    foreach(var dir in Directory.GetDirectories(folder))
                    {
                        Directory.Delete(dir, true);
                    }
                    string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                    foreach (string s in files)
                    {
                        if (s.Contains("Cats.csv") || s.Contains("World.csv") || s.Contains("FreshKillPile.csv") || s.Contains("Clan.csv"))
                        {
                            File.Delete(s);
                        }
                    }
                    playerClan.freshKillPile = null;
                    playerClan.clanCats = null;
                    leader = null;
                    temporaryCat = null;
                }
                else if (noInputs.Contains(input))
                {
                    OpeningScreen();
                }
                else
                {
                }

            }
            
        }

        public void Load()
        {
            string folder = Directory.GetCurrentDirectory();
            string pathSave = Path.Combine(folder, "Saves.csv");
            if (File.Exists(pathSave))
            {
                List<Game> recordsList;
                using (var reader = new StreamReader(pathSave))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var recordsSave = csv.GetRecords<Game>();
                    recordsList = recordsSave.ToList();
                    //Console.WriteLine(recordsSave.ToList().Count);
                }
                

                Console.WriteLine("Choose a file to load:\n");
                string loadOptions = "";
                for (int i = 0; i < recordsList.Count; i++)
                {
                    loadOptions += "[" + i + "]: ";
                    loadOptions += recordsList[i].saveName;
                    loadOptions += "\n";
                }
                Console.WriteLine(loadOptions);
                string optionsNumber = "";
                int options = -1;
                while (!optionsNumber.All(Char.IsDigit) || string.IsNullOrWhiteSpace(optionsNumber) || int.Parse(optionsNumber) > recordsList.Count - 1)
                {
                    optionsNumber = Console.ReadLine();
                    
                }
                options = int.Parse(optionsNumber);
                string subfolder = Path.Combine(folder, recordsList[options].saveName);
                string path = Path.Combine(subfolder, recordsList[options].saveName);
                //Console.Write(path);
                using var streamReader = File.OpenText(Path.Combine(path) + "World.csv");
                using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

                var worlds = csvReader.GetRecords<WorldSaveFile>();

                List<WorldSaveFile> worldsList = worlds.ToList();
                WorldStatus world = new WorldStatus(worldsList[0].saveName, worldsList[0].day, worldsList[0].weather);
                activeWorld = world;
                fileName = recordsList[options].saveName;

                using var streamReader2 = File.OpenText(Path.Combine(path + "FreshKillPile.csv"));
                using var csvReader2 = new CsvReader(streamReader2, CultureInfo.CurrentCulture);

                var freshKill = csvReader2.GetRecords<FreshKillRecord>();
                List<FreshKill> killList = new List<FreshKill>();
                foreach (var r in freshKill)
                {
                    killList.Add(new FreshKill(Prey.prey[r.freshKill]));
                }
                FreshKill[] freshKillPile = killList.ToArray();

                using var streamReader5 = File.OpenText(Path.Combine(path + "Items.csv"));
                using var csvReader5 = new CsvReader(streamReader5, CultureInfo.CurrentCulture);

                var items = csvReader5.GetRecords<ItemRecord>();
                List<Item> itemList = new List<Item>();
                foreach (var r in items)
                {
                    Item item = new Item();
                    item.type = r.type;
                    item.description = r.desc;
                    item.usable = r.usable;
                    itemList.Add(item);
                }
                Item[] itemArray = itemList.ToArray();


                using var streamReader4 = File.OpenText(Path.Combine((path) + "Cats.csv"));
                using var csvReader4 = new CsvReader(streamReader4, CultureInfo.CurrentCulture);
                BinaryFormatter bf = new BinaryFormatter();
                var cats = csvReader4.GetRecords<CatRecord>();

                List<Cat> catsList = new List<Cat>();
                XmlSerializer xml = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                XmlSerializer xml2 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                foreach (CatRecord cat in cats)
                {
                    StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                    SerializableDictionary<string, int> dict;
                    SerializableDictionary<string, string> dictFT;
                    using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Relations.csv", FileMode.Open))
                    {
                        dict = (SerializableDictionary<string, int>)xml.Deserialize(fs);
                    }
                    using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "FamilyTree.csv", FileMode.Open))
                    {
                        dictFT = (SerializableDictionary<string, string>)xml2.Deserialize(fs);
                    }
                    List<Wound> wounds = null;
                    if (File.Exists((path) + cat.prefix + cat.suffix + "Wounds.csv"))
                    {
                        
                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Wounds.csv", FileMode.Open))
                        {
                            var woundsVar = (bf.Deserialize(fs));
                            wounds = (List<Wound>)woundsVar;
                        }
                    }
                    List<Disease> diseases = null;
                    if (File.Exists((path) + cat.prefix + cat.suffix + "Diseases.csv"))
                    {
                        
                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Diseases.csv", FileMode.Open))
                        {
                            var diseasesVar = (List<Disease>)bf.Deserialize(fs);
                            diseases = diseasesVar;
                        }
                    }
                    Skills skills = new Skills(cat.hunt, cat.med);
                    Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), skills, cat.energy, cat.health, dict, wounds, diseases, cat.preg, dictFT);
                    catsList.Add(newCat);
                }
                
                using var streamReader3 = File.OpenText(Path.Combine((path) + "Clan.csv"));
                using var csvReader3 = new CsvReader(streamReader3, CultureInfo.CurrentCulture);

                var clanFile = csvReader3.GetRecords<ClanRecord>();
                List<ClanRecord> clanList = clanFile.ToList();
                Clan clan = new Clan(clanList[0].clanName, Biomes.biomeDict[clanList[0].biome], catsList, killList, itemList);
                playerClan = clan;
                string[,] stringArray;
                using (FileStream fs = new FileStream(Path.Combine((path) + "Map.csv"), FileMode.Open))
                {
                    stringArray = (string[,])bf.Deserialize(fs);
                }
                strings = stringArray;
                Biome[,] biomes = new Biome[stringArray.GetLength(0), stringArray.GetLength(1)];
                Tile[,] tileMap = new Tile[stringArray.GetLength(0), stringArray.GetLength(1)];
                Char[,] chars = new Char[stringArray.GetLength(0), stringArray.GetLength(1)];
                List<Biome> biomeChoices = new List<Biome>();
                for (int x = 0; x < stringArray.GetLength(0); x++)
                {
                    for (int y = 0; y < stringArray.GetLength(1); y++)
                    {
                        biomes[x, y] = Biomes.biomeDict[stringArray[x, y]];
                        tileMap[x, y] = new Tile(biomes[x, y]);
                        chars[x, y] = biomes[x, y].character;
                        if(!biomeChoices.Contains(biomes[x,y]))
                        {
                            biomeChoices.Add(biomes[x, y]);
                            //Console.WriteLine("Add " + biomes[x, y].name);
                        }
                        //Console.WriteLine(biomes[x,y].character);
                    }
                }


                

                TileMap tileMap2 = new TileMap(tileMap, chars, playerClan.biome, biomeChoices);
                map = new Map(stringArray.GetLength(0), stringArray.GetLength(1), tileMap2);



                //output += "\n\nBiome: " + tileMap.tileMap[playerPos.playerX, playerPos.playerY].biome.name + " | Prey Types: " + Utils.CreateList(tileMap.tileMap[playerPos.playerX, playerPos.playerY].biome.preyTypes) + "\nPlayer X : " + playerPos.playerX + "   Player Y: " + playerPos.playerY;
                
                string[] directories = Directory.GetDirectories(subfolder);
                string patrolDirectory = directories.FirstOrDefault(x => x.Contains("Patrol"));

                patrolDirectory = Path.Combine(subfolder, "Patrol");
                
                //PressEToContinue();
                if (Directory.Exists(patrolDirectory))
                {
                    
                    //PressEToContinue();
                    using var streamReader6 = File.OpenText(Path.Combine((patrolDirectory), "Cats.csv"));
                    
                    using var csvReader6 = new CsvReader(streamReader6, CultureInfo.CurrentCulture);
                    var cats2 = csvReader6.GetRecords<CatRecord>();

                    List<Cat> catsList2 = new List<Cat>();
                    XmlSerializer xml3 = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                    XmlSerializer xml4 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    foreach (CatRecord cat in cats2)
                    {
                        StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                        SerializableDictionary<string, int> dict;
                        SerializableDictionary<string, string> dictFT;
                        using (FileStream fs = new FileStream((patrolDirectory) + cat.prefix + cat.suffix + "Relations.csv", FileMode.Open))
                        {
                            dict = (SerializableDictionary<string, int>)xml3.Deserialize(fs);
                        }
                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "FamilyTree.csv", FileMode.Open))
                        {
                            dictFT = (SerializableDictionary<string, string>)xml4.Deserialize(fs);
                        }
                        List<Wound> wounds = null;
                        if (File.Exists((patrolDirectory) + cat.prefix + cat.suffix + "Wounds.csv"))
                        {

                            using (FileStream fs = new FileStream((patrolDirectory) + cat.prefix + cat.suffix + "Wounds.csv", FileMode.Open))
                            {
                                var woundsVar = (bf.Deserialize(fs));
                                wounds = (List<Wound>)woundsVar;
                            }
                        }
                        List<Disease> diseases = null;
                        if (File.Exists((patrolDirectory) + cat.prefix + cat.suffix + "Diseases.csv"))
                        {

                            using (FileStream fs = new FileStream((patrolDirectory) + cat.prefix + cat.suffix + "Diseases.csv", FileMode.Open))
                            {
                                var diseasesVar = (List<Disease>)bf.Deserialize(fs);
                                diseases = diseasesVar;
                            }
                        }
                        Skills skills = new Skills(cat.hunt, cat.med);
                        Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), skills, cat.energy, cat.health, dict, wounds, diseases, cat.preg, dictFT);
                        catsList2.Add(newCat);
                    }


                    List<Item> patrolItemList = null;
                    if (File.Exists(patrolDirectory + "PatrolItems.csv"))
                    {
                        using (FileStream fs = new FileStream((path) + "PatrolItems.csv", FileMode.Open))
                        {
                            var patrolItemVar = (List<Item>)bf.Deserialize(fs);
                            patrolItemList = patrolItemVar;
                        }
                    }
                    Patrol patrol = new Patrol(catsList2, patrolItemList);
                    activePatrol = patrol;
                }
                
                
            }
            else
            {
                Console.WriteLine("No files to load. Try creating a new game.");
                OpeningScreen();
            }
        }

        public string ClanScreen()
        {
            string output = "";
            output += "Clan Name: " + playerClan.name + "\n";
            output += "Cats: \n";
            foreach(Cat cat in playerClan.clanCats)
            {
                output += cat.name + "\n";
            }
            output += "Fresh Kill Pile: \n";
            foreach(FreshKill prey in playerClan.freshKillPile)
            {
                output += prey.species + "\n";
            }
            output += "Day: " + activeWorld.day + "\n";
            output += "Season: " + World.GetSeason(activeWorld.day) + "\n";
            return output;
        }

        

        public void CheckQuit(string input)
        {
            if (quitInputs.Contains(input))
            {
                playing = false;
                System.Environment.Exit(1);
            }
        }

        public static void PressEToContinue()
        {
            Console.WriteLine("\nEnter 'e' to continue.");
            ConsoleKey key = ConsoleKey.K;
            while (key != ConsoleKey.E)
            {
                key = Console.ReadKey().Key;
            }
        }

        public void Setup()
        {
            Strings strings = new Strings();
            if (leader == null)
            {
                leader = CreateLeader(strings); 
                Console.Clear();
                Console.WriteLine(strings.stringDictionary["introString2"]);
                PressEToContinue();
                Console.Clear();
                playerClan = new Clan();
                List<Cat> catList = new List<Cat>() { leader };
                playerClan.clanCats = catList;
                playerClan.name = "Unnamed Clan";
                playerClan.freshKillPile = new List<FreshKill>();
                playerClan.items = new List<Item>();
                PreyEncounter introPreyEncounter = new PreyEncounter("Mouse", leader, playerClan);
                PressEToContinue();
                Console.Clear();
                Console.WriteLine(strings.stringDictionary["introString3"]);
                PressEToContinue();
                Console.Clear();
                Biome biome = ChooseBiome();
                playerClan.biome = biome;
                PressEToContinue();
                Console.Clear();
                Console.WriteLine("What should this new save file be saved under?");
                string name = "";
                while (string.IsNullOrWhiteSpace(name))
                {
                    name = Console.ReadLine();
                    CheckQuit(name);
                    name.Replace(' ', '-');
                }
                //Console.Clear();

                activeWorld = new WorldStatus(name);
                Map map = new Map(23, 60, new TileMap());
                Save.SaveGame(playerClan, activeWorld, name, map.CreateTileMap(map.sizeX, map.sizeY, playerClan.biome, this.biomeChoices));
                this.map = map;
                //Load();
                Console.Clear();
                string folder = Directory.GetCurrentDirectory();
                string subfolder = Path.Combine(folder, name);
                string path = Path.Combine(subfolder, name);
                using var streamReader = File.OpenText(Path.Combine(path) + "World.csv");
                using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

                var worlds = csvReader.GetRecords<WorldSaveFile>();

                List<WorldSaveFile> worldsList = worlds.ToList();
                WorldStatus world = new WorldStatus(worldsList[0].saveName, worldsList[0].day, worldsList[0].weather);
                activeWorld = world;
                fileName = name;

                using var streamReader2 = File.OpenText(Path.Combine(path + "FreshKillPile.csv"));
                using var csvReader2 = new CsvReader(streamReader2, CultureInfo.CurrentCulture);

                var freshKill = csvReader2.GetRecords<FreshKillRecord>();
                List<FreshKill> killList = new List<FreshKill>();
                foreach (var r in freshKill)
                {
                    killList.Add(new FreshKill(Prey.prey[r.freshKill]));
                }
                FreshKill[] freshKillPile = killList.ToArray();
                using var streamReader5 = File.OpenText(Path.Combine(path + "Items.csv"));
                using var csvReader5 = new CsvReader(streamReader5, CultureInfo.CurrentCulture);

                var items = csvReader5.GetRecords<ItemRecord>();
                List<Item> itemList = new List<Item>();
                foreach (var r in items)
                {
                    Item item = new Item();
                    item.type = r.type;
                    item.description = r.desc;
                    item.usable = r.usable;
                    itemList.Add(item);
                }
                Item[] itemArray = itemList.ToArray();

                using var streamReader4 = File.OpenText(Path.Combine((path) + "Cats.csv"));
                using var csvReader4 = new CsvReader(streamReader4, CultureInfo.CurrentCulture);

                var cats = csvReader4.GetRecords<CatRecord>();

                BinaryFormatter bf = new BinaryFormatter();

                List<Cat> catsList = new List<Cat>();
                XmlSerializer xml = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                XmlSerializer xml3 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                foreach (CatRecord cat in cats)
                {
                    StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                    SerializableDictionary<string, int> dict;
                    SerializableDictionary<string, string> dictFT;
                    using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Relations.csv", FileMode.Open))
                    {
                        dict = (SerializableDictionary<string, int>)xml.Deserialize(fs);
                    }
                    using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "FamilyTree.csv", FileMode.Open))
                    {
                        dictFT = (SerializableDictionary<string, string>)xml3.Deserialize(fs);
                    }
                    List<Wound> wounds = null;
                    if (File.Exists((path) + cat.prefix + cat.suffix + "Wounds.csv"))
                    {

                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Wounds.csv", FileMode.Open))
                        {
                            var woundsVar = (bf.Deserialize(fs));
                            wounds = (List<Wound>)woundsVar;
                        }
                    }
                    List<Disease> diseases = null;
                    if (File.Exists((path) + cat.prefix + cat.suffix + "Diseases.csv"))
                    {

                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Diseases.csv", FileMode.Open))
                        {
                            var diseasesVar = (List<Disease>)bf.Deserialize(fs);
                            diseases = diseasesVar;
                        }
                    }
                    Skills catSkills = new Skills(cat.hunt, cat.med);
                    Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), catSkills, cat.energy, cat.health, dict, wounds, diseases, cat.preg, dictFT);
                    catsList.Add(newCat);
                }

                using var streamReader3 = File.OpenText(Path.Combine((path) + "Clan.csv"));
                using var csvReader3 = new CsvReader(streamReader3, CultureInfo.CurrentCulture);

                var clanFile = csvReader3.GetRecords<ClanRecord>();
                List<ClanRecord> clanList = clanFile.ToList();
                Clan clan = new Clan(clanList[0].clanName, Biomes.biomeDict[clanList[0].biome], catsList, killList, itemList);
                playerClan = clan;
                //BinaryFormatter bf = new BinaryFormatter();
                string[,] stringArray;
                using (FileStream fs = new FileStream(Path.Combine((path) + "Map.csv"), FileMode.Open))
                {
                    stringArray = (string[,])bf.Deserialize(fs);
                }
                Biome[,] biomes = new Biome[stringArray.GetLength(0), stringArray.GetLength(1)];
                Tile[,] tileMap = new Tile[stringArray.GetLength(0), stringArray.GetLength(1)];
                Char[,] chars = new Char[stringArray.GetLength(0), stringArray.GetLength(1)];
                List<Biome> biomeChoices = new List<Biome>();
                for (int x = 0; x < stringArray.GetLength(0); x++)
                {
                    for (int y = 0; y < stringArray.GetLength(1); y++)
                    {
                        biomes[x, y] = Biomes.biomeDict[stringArray[x, y]];
                        tileMap[x, y] = new Tile(biomes[x, y]);
                        chars[x, y] = biomes[x, y].character;
                        if (!biomeChoices.Contains(biomes[x, y]))
                        {
                            biomeChoices.Add(biomes[x, y]);
                            //Console.WriteLine("Add " + biomes[x, y].name);
                        }
                        //Console.WriteLine(biomes[x,y].character);
                    }
                }




                TileMap tileMap2 = new TileMap(tileMap, chars, playerClan.biome, biomeChoices);
                this.map = new Map(stringArray.GetLength(0), stringArray.GetLength(1), tileMap2);

                string[] directories = Directory.GetDirectories(subfolder);
                string patrolDirectory = directories.FirstOrDefault(x => x.Contains("Patrol"));

                patrolDirectory = path + "Patrol";
                //Console.WriteLine(patrolDirectory);
                //PressEToContinue();
                if (Directory.Exists(patrolDirectory))
                {
                    Console.WriteLine("Patrolling");
                    using var streamReader6 = File.OpenText(Path.Combine((patrolDirectory) + "Cats.csv"));
                    using var csvReader6 = new CsvReader(streamReader6, CultureInfo.CurrentCulture);
                    var cats2 = csvReader6.GetRecords<CatRecord>();

                    List<Cat> catsList2 = new List<Cat>();
                    XmlSerializer xml2 = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                    XmlSerializer xml4 = new XmlSerializer(typeof(SerializableDictionary<string, int>));

                    foreach (CatRecord cat in cats)
                    {
                        StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                        SerializableDictionary<string, int> dict;
                        SerializableDictionary<string, string> dictFT;
                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Relations.csv", FileMode.Open))
                        {
                            dict = (SerializableDictionary<string, int>)xml2.Deserialize(fs);
                        }
                        using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "FamilyTree.csv", FileMode.Open))
                        {
                            dictFT = (SerializableDictionary<string, string>)xml4.Deserialize(fs);
                        }
                        List<Wound> wounds = null;
                        if (File.Exists((path) + cat.prefix + cat.suffix + "Wounds.csv"))
                        {

                            using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Wounds.csv", FileMode.Open))
                            {
                                var woundsVar = (bf.Deserialize(fs));
                                wounds = (List<Wound>)woundsVar;
                            }
                        }
                        List<Disease> diseases = null;
                        if (File.Exists((path) + cat.prefix + cat.suffix + "Diseases.csv"))
                        {

                            using (FileStream fs = new FileStream((path) + cat.prefix + cat.suffix + "Diseases.csv", FileMode.Open))
                            {
                                var diseasesVar = (List<Disease>)bf.Deserialize(fs);
                                diseases = diseasesVar;
                            }
                        }
                        Skills catSkills = new Skills(cat.hunt, cat.med);
                        Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), catSkills, cat.energy, cat.health, dict, wounds, diseases, cat.preg, dictFT);
                        catsList2.Add(newCat);
                    }


                    List<Item> patrolItemList = null;
                    if (File.Exists(patrolDirectory + "PatrolItems.csv"))
                    {
                        using (FileStream fs = new FileStream((path) + "PatrolItems.csv", FileMode.Open))
                        {
                            var patrolItemVar = (List<Item>)bf.Deserialize(fs);
                            patrolItemList = patrolItemVar;
                        }
                    }
                    Patrol patrol = new Patrol(catsList2, patrolItemList);
                    activePatrol = patrol;
                }
            }

        }

        public Biome ChooseBiome()
        {
            List<int> usedNums = new List<int>();
            while(biomeChoices.Count <= 4)
            {
                Random rand = new Random();
                int num = rand.Next(0, Biomes.biomes.Length);
                
                if(!usedNums.Contains(num))
                {
                    biomeChoices.Add(Biomes.biomes[num]);
                    usedNums.Add(num);
                }
            }
            Console.WriteLine("On your journey, you have seen the following locations:");
            Console.WriteLine("\n[0]: " + DisplayBiome(biomeChoices[0]) + "\n[1]: " + DisplayBiome(biomeChoices[1]) + "\n[2]: " + DisplayBiome(biomeChoices[2]) + "\n[3]: " + DisplayBiome(biomeChoices[3]));
            string checkBiome = "";
            Console.WriteLine("\nWhere will you settle your clan?");
            while (!checkBiome.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkBiome) || int.Parse(checkBiome) > 3)
            {
                checkBiome = Console.ReadLine();
                CheckQuit(checkBiome);
            }
            int biomeNumber = int.Parse(checkBiome);
            Biome biome = biomeChoices[biomeNumber];
            playerClan.biome = biome;
            bool check = ConfirmBiome(biome);
            while (!check)
            {
                check = ConfirmBiome(biome);
                biome = playerClan.biome;
            }



            return biome;
        }

        bool ConfirmBiome(Biome biome)
        {
            string input = "";

            while (!yesInputs.Contains(input) && !noInputs.Contains(input))
            {
                Console.WriteLine("Are you sure you wish to settle in the " + playerClan.biome.name.ToLower() + "?");
                input = Console.ReadLine();
                CheckQuit(input);

                if (yesInputs.Contains(input))
                {
                    Console.WriteLine("The " + playerClan.biome.name + " is a great place to settle. Your clan will thrive for generations.");
                    return true;
                }
                else if (noInputs.Contains(input))
                {
                    Console.WriteLine("Where will you settle your clan?");
                    string checkBiome = "";
                    while (!checkBiome.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkBiome) || int.Parse(checkBiome) > 3)
                    {
                        checkBiome = Console.ReadLine();
                        CheckQuit(checkBiome);
                    }
                    int biomeNumber = int.Parse(checkBiome);
                    biome = biomeChoices[biomeNumber];
                    playerClan.biome = biome;
                    return false;
                }
                else
                {
                    return false;
                }

            }

            return false;
        }

        int DebugScreen()
        {
            Console.WriteLine("Where do you want to start?\n[0]: Beginning\n[1]: Clan Screen\n[2]: Encounter Test\n[3]: Load Test\n[4]: Pregnancy test");
            string checkDebug = "";
            while (!checkDebug.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkDebug) || int.Parse(checkDebug) > 4)
            {
                checkDebug = Console.ReadLine();
                
                
            }
            int debug = int.Parse(checkDebug); return debug;
        }

        public string DisplayBiome(Biome biome)
        {
            string output = biome.name + "\nDescription: " + biome.description + "\nAvailible Prey: " + Utils.CreateList(biome.preyTypes);
            return output;
        }


        public Cat CreateLeader(Strings strings)
        {
            Console.Clear();
            Console.WriteLine(strings.stringDictionary["introString"]);
            //Intro
            StatBlock origin = GetOrigin();
            Console.Clear();
            //Naming
            Console.WriteLine("What is your name? (Prefix only)");
            string name = GetNameLoner();
            name = name.First().ToString().ToUpper() + name.Substring(1);
            Console.Clear();
            //Age
            Console.WriteLine("How old are you? (In moons)");
            int age = GetAge();
            Console.Clear();
            //Gender
            Console.WriteLine("What is your gender?");
            Defs.Gender gender = GetGender();
            Console.Clear();
            //Rank
            Defs.Rank rank = Defs.Rank.LonerLeader;

            
            string[] personality = GetPersonality();
            Console.Clear();
            Skills skills = new Skills(0, 0);
            //Temporary cat
            Cat tempCat = new Cat(name, "", rank, gender, age, origin, personality, skills);
            temporaryCat = tempCat;
            bool check = CheckError(tempCat);
            while (!check)
            {
                Console.Clear();
                check = CheckError(temporaryCat);
                tempCat = temporaryCat;
            }
            Console.Clear();
            return tempCat;
        }

        public bool CheckError(Cat cat)
        {
            string input = "";

            while (!yesInputs.Contains(input) && !noInputs.Contains(input))
            {
                Console.WriteLine("You've created your cat! Are the following values correct?");
                Console.WriteLine(DisplayBioLoner(cat));
                Console.WriteLine(DisplayStats(cat.statBlock));
                input = Console.ReadLine();
                CheckQuit(input);

                if (yesInputs.Contains(input))
                {
                    Console.WriteLine("It is good to meet you, " + cat.name);
                    return true;
                }
                else if (noInputs.Contains(input))
                {
                    temporaryCat = CorrectError(cat);
                    return false;
                }
                else
                {
                    return false;
                }

            }

            return false;
        }
        public Cat CorrectError(Cat cat)
        {
            Console.WriteLine("Which part is incorrect? \n[0]: Name\n[1]: Age\n[2]: Gender\n[3]: Personality");
            string checkError = "";
            while (!checkError.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkError) || int.Parse(checkError) > 3)
            {
                checkError = Console.ReadLine();
                CheckQuit(checkError);
            }
            string name = cat.name;
            int age = cat.age;
            Defs.Gender gender = cat.genderAccesor;
            string[] personality = cat.personality;
            int error = int.Parse(checkError);
            switch (error)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("What is your name? (Prefix only)");
                    name = GetNameLoner();
                    name = name.First().ToString().ToUpper() + name.Substring(1);
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("How old are you? (In moons)");
                    age = GetAge();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("What is your gender?");
                    gender = GetGender();
                    break;
                case 3:
                    Console.Clear();
                    //Console.WriteLine("What is your personality like?");
                    personality = GetPersonality();
                    break;
            }
            Skills skills = new Skills(0, 0);
            Cat tempCat = new Cat(name, "", cat.rankAccesor, gender, age, cat.statBlock, personality, skills);
            return tempCat;

        }

        public string GetNameLoner()
        {
            string checkName = "";
            while (!checkName.All(Char.IsLetter) || string.IsNullOrWhiteSpace(checkName))
            {
                checkName = Console.ReadLine();
                CheckQuit(checkName);
            }
            string name = checkName;
            return name;
        }

        public int GetAge()
        {
            string checkAge = "";
            int age = 0;
            while (!checkAge.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkAge) || !int.TryParse(checkAge, out age) || age < 6 || age > 180)
            {
                checkAge = Console.ReadLine();
                CheckQuit(checkAge);
                if(int.TryParse(checkAge, out age))
                {
                    age = int.Parse(checkAge);
                    
                        if (age < 6)
                        {
                            Console.WriteLine("You're too young, yet. Wait a few more months to begin your journey.");
                        }
                        else if (age > 180)
                        {
                            Console.WriteLine("You do not have the strength for this journey. A warm human home would be a better place for your last days.");
                        }
                    
                }
            }
            
            
            return age;
        }

        public string[] GetPersonality()
        {
            List<string> traits = new List<string>();
            while (traits.Count < 2)
            {
                Console.WriteLine("What is your personality like?");
                List<string> traitTypes = Defs.traitDict.Values.ToList().Distinct().ToList();
                for (int i = 0; i < traitTypes.Count; i++)
                {
                    Console.WriteLine("[" + i + "]: I am " + traitTypes[i]);
                }
                string checkPersonality = "";
                while (!checkPersonality.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkPersonality) || int.Parse(checkPersonality) > traitTypes.Count - 1)
                {
                    checkPersonality = Console.ReadLine();
                    CheckQuit(checkPersonality);
                }
                int personalityTypeNumber = int.Parse(checkPersonality);
                string chosenPersonality = traitTypes[personalityTypeNumber];
                Console.Clear();

                List<string> shrunkDict = new List<string>();
                for (int i = 0; i < Defs.traitDict.Count; i++)
                {
                    if (Defs.traitDict.Values.ElementAt(i) == chosenPersonality)
                    {
                        if (traits.Count == 0 || !traits.Contains(Defs.traitDict.Keys.ElementAt(i)))
                        {
                            shrunkDict.Add(Defs.traitDict.Keys.ElementAt(i));
                        }
                            
                    }
                }
                


                Console.WriteLine("Choose two personality traits:");
                for (int i = 0; i < shrunkDict.Count; i++)
                {
                    Console.WriteLine("[" + i + "]: I am " + shrunkDict[i]);
                }
                string checkPersonality2 = "";
                while (!checkPersonality2.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkPersonality2) || int.Parse(checkPersonality2) > shrunkDict.Count - 1)
                {
                    checkPersonality2 = Console.ReadLine();
                    CheckQuit(checkPersonality2);
                }
                int traitNumber = int.Parse(checkPersonality2);
                string chosenTrait = shrunkDict[traitNumber];
                traits.Add(chosenTrait);
                shrunkDict.Remove(chosenTrait); Console.Clear();


            }


            return traits.ToArray();
        }

        public Defs.Gender GetGender()
        {
            Console.WriteLine("[0]: Tom \n[1]: She-cat \n[2]: Other");
            string checkGender = "";
            while (!checkGender.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkGender) || int.Parse(checkGender) > 2)
            {
                checkGender = Console.ReadLine();
                CheckQuit(checkGender);
            }
            int genderNumber = int.Parse(checkGender);
            Defs.Gender gender = Defs.genderDict.Keys.ElementAt(genderNumber);
            return gender;
        }

        public StatBlock GetOrigin()
        {
            string checkOrigin = "";
            while (!checkOrigin.All(Char.IsDigit) || string.IsNullOrWhiteSpace(checkOrigin) || int.Parse(checkOrigin) > 3)
            {
                checkOrigin = Console.ReadLine();
                CheckQuit(checkOrigin);
            }
            int originNumber = int.Parse(checkOrigin);
            StatBlock origin = StatBlocks.origins[originNumber];
            return origin;
        }

        public string DisplayBioLoner(Cat cat)
        {
            string display = cat.name + " \nAge: " + cat.age + " moons \nGender: " + Defs.genderDict[cat.genderAccesor] + "\nPersonality: " + Utils.CreateList(cat.personality);
            return display;
        }

        public string DisplayStats(StatBlock stats)
        {
            string display = "\nAgility: " + stats.agility + "\nConstitution: " + stats.constitution + "\nIntelligence:" + stats.intelligence + "\nFinesse: " + stats.finesse + "\nStrength: " + stats.strength + "\nTenacity: " + stats.tenacity;
            return display;
        }
    }
}
