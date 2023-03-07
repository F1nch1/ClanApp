using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ClanCreatorApp
{
    public class Save
    {
       
        public static Patrol SavePatrol(Patrol patrol, string save)
        {
            string newFolder = Path.Combine(save, "Patrol");
            Directory.CreateDirectory(newFolder);
            save = newFolder;
            BinaryFormatter bf = new BinaryFormatter();
            string patrolItemsSaveString = Path.Combine(save, "PatrolItems.csv");
            List<string> names = new List<string>();

            SavePatrolCats(patrol.patrolCats.ToArray(), newFolder);

            if(patrol.itemsHeld.Count > 0)
            {
                using (FileStream fs = new FileStream(patrolItemsSaveString, FileMode.Create))
                    bf.Serialize(fs, patrol.itemsHeld);
            }
            return patrol;
        }

        public static WorldSaveFile SaveWorld(WorldStatus worldFile)
        {
            WorldSaveFile saveFile = new WorldSaveFile();
            saveFile.day = worldFile.day;
            saveFile.season = World.GetSeason(worldFile.day);
            saveFile.saveName = worldFile.saveName + "World.csv";
            saveFile.weather = worldFile.weather;

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<WorldSaveFile>();
            csvWriter.NextRecord(); // adds new line after header

            var world = new List<WorldSaveFile> { saveFile };


            csvWriter.WriteRecords(world);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());
            File.WriteAllText(worldFile.saveName + "World.csv", res);
            //Console.WriteLine(File.ReadAllText(worldFile.saveName + "World.csv"));
            return saveFile;
        }

        public static ClanRecord SaveClan(Clan clanFile, string save)
        {
            ClanRecord clanSave = new ClanRecord();
            clanSave.clanName = clanFile.name;
            clanSave.biome = clanFile.biome.name;
            clanSave.saveName = (save) + "Clan.csv";

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<ClanRecord>();
            csvWriter.NextRecord(); // adds new line after header

            var clan = new List<ClanRecord> { clanSave };


            csvWriter.WriteRecords(clan);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());
            File.WriteAllText(clanSave.saveName, res);
            //Console.WriteLine(File.ReadAllText(clanSave.saveName));
            return clanSave;
        }

        public static MapRecord SaveMap(Tile[,] map, Char[,] chars, string save)
        {
            MapRecord mapSave = new MapRecord();
            string[,] biomes = new string[map.GetLength(0), map.GetLength(1)];
            mapSave.map = biomes;
            for (int x = 0; x < mapSave.map.GetLength(0); x++)
            {
                for (int y = 0; y < mapSave.map.GetLength(1); y++)
                {

                    biomes[x, y] = map[x, y].biome.name;
                }
            }
            //BinaryFormatter bf = new BinaryFormatter();

            string saveName = (save) + "Map.csv";

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(saveName, FileMode.Create))
                bf.Serialize(fs, biomes);


            //Console.WriteLine(File.ReadAllText(saveName));
            return mapSave;
        }

        public static CatRecord[] SaveCats(Cat[] cats, string save)
        {
            var catRecords = new List<CatRecord>();
            foreach(Cat cat in cats)
            {
                CatRecord record = new CatRecord();
                record.prefix = cat.prefix;
                record.suffix = cat.suffix;
                record.species = "Cat";
                record.rankAssigner = Defs.rankDict[cat.rankAccesor];
                record.genderAssigner = Defs.genderDict[cat.genderAccesor];
                record.age = cat.age;
                record.energy = cat.energy;
                record.health = cat.health;
                record.agi = cat.statBlock.agility;
                record.con = cat.statBlock.constitution;
                record.fin = cat.statBlock.finesse;
                record.intl = cat.statBlock.intelligence;
                record.str = cat.statBlock.strength;
                record.ten = cat.statBlock.tenacity;
                record.personality = string.Join("&", cat.personality);
                record.hunt = cat.skills.hunting;
                record.med = cat.skills.medicine;
                record.preg = cat.pregnancy;
                catRecords.Add(record);

                string saveName = (save) + cat.name + "Relations.csv";
                XmlSerializer xml = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                using (FileStream fs = new FileStream(saveName, FileMode.Create))
                    xml.Serialize(fs, cat.relations);

                string saveNameFT = (save) + cat.name + "FamilyTree.csv";
                XmlSerializer xml2 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                using (FileStream fs = new FileStream(saveNameFT, FileMode.Create))
                    xml2.Serialize(fs, cat.familyTree);

                if (cat.wounds.Count > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string woundSaveName = save + cat.name + "Wounds.csv";
                    using (FileStream fs = new FileStream(woundSaveName, FileMode.Create))
                        bf.Serialize(fs, cat.wounds);
                }
                else
                {
                    if(File.Exists(save + cat.name + "Wounds.csv"))
                    {
                        File.Delete(save + cat.name + "Wounds.csv");
                    }
                }
                if (cat.diseases.Count > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string woundSaveName = save + cat.name + "Diseases.csv";
                    using (FileStream fs = new FileStream(woundSaveName, FileMode.Create))
                        bf.Serialize(fs, cat.diseases);
                }
                else
                {
                    if (File.Exists(save + cat.name + "Diseases.csv"))
                    {
                        File.Delete(save + cat.name + "Diseases.csv");
                    }
                }
            }

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<CatRecord>();
            csvWriter.NextRecord(); // adds new line after header

            csvWriter.WriteRecords(catRecords);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());

            string path = save + "Cats.csv";
            File.WriteAllText(path, res);
            //Console.WriteLine(File.ReadAllText(path));
            return (catRecords.ToArray());
        }

        public static CatRecord[] SavePatrolCats(Cat[] cats, string save)
        {
            var catRecords = new List<CatRecord>();
            foreach (Cat cat in cats)
            {
                CatRecord record = new CatRecord();
                record.prefix = cat.prefix;
                record.suffix = cat.suffix;
                record.species = "Cat";
                record.rankAssigner = Defs.rankDict[cat.rankAccesor];
                record.genderAssigner = Defs.genderDict[cat.genderAccesor];
                record.age = cat.age;
                record.energy = cat.energy;
                record.health = cat.health;
                record.agi = cat.statBlock.agility;
                record.con = cat.statBlock.constitution;
                record.fin = cat.statBlock.finesse;
                record.intl = cat.statBlock.intelligence;
                record.str = cat.statBlock.strength;
                record.ten = cat.statBlock.tenacity;
                record.hunt = cat.skills.hunting;
                record.med = cat.skills.medicine;
                record.preg = cat.pregnancy;
                record.personality = string.Join("&", cat.personality);
                catRecords.Add(record);

                string saveName = (save) + cat.name + "Relations.csv";
                XmlSerializer xml = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                using (FileStream fs = new FileStream(saveName, FileMode.Create))
                    xml.Serialize(fs, cat.relations);

                string saveNameFT = (save) + cat.name + "FamilyTree.csv";
                XmlSerializer xml2 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                using (FileStream fs = new FileStream(saveNameFT, FileMode.Create))
                    xml2.Serialize(fs, cat.familyTree);

                if (cat.wounds.Count > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string woundSaveName = save + cat.name + "Wounds.csv";
                    using (FileStream fs = new FileStream(woundSaveName, FileMode.Create))
                        bf.Serialize(fs, cat.wounds);
                }
                else
                {
                    if (File.Exists(save + cat.name + "Wounds.csv"))
                    {
                        File.Delete(save + cat.name + "Wounds.csv");
                    }
                }
                if (cat.diseases.Count > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string woundSaveName = save + cat.name + "Diseases.csv";
                    using (FileStream fs = new FileStream(woundSaveName, FileMode.Create))
                        bf.Serialize(fs, cat.diseases);
                }
                else
                {
                    if (File.Exists(save + cat.name + "Diseases.csv"))
                    {
                        File.Delete(save + cat.name + "Diseases.csv");
                    }
                }
            }

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<CatRecord>();
            csvWriter.NextRecord(); // adds new line after header

            csvWriter.WriteRecords(catRecords);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());

            string path = Path.Combine(save, "Cats.csv");
            File.WriteAllText(path, res);
            //Console.WriteLine(File.ReadAllText(path));
            return (catRecords.ToArray());
        }

        public static ItemRecord[] SaveItems(Item[] items, string save)
        {
            var itemRecords = new List<ItemRecord>();
            foreach (Item item in items)
            {
                ItemRecord record = new ItemRecord();
                record.type = item.type;
                record.desc = item.description;
                record.usable = item.usable;
                itemRecords.Add(record);
            }

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<ItemRecord>();
            csvWriter.NextRecord(); // adds new line after header

            csvWriter.WriteRecords(itemRecords);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());
            string path = save + "Items.csv";
            File.WriteAllText(path, res);
            //Console.WriteLine(File.ReadAllText(path));
            return (itemRecords.ToArray());
        }

        public static FreshKillRecord[] SaveFreshKillPile(FreshKill[] freshKill, string save)
        {
            var killRecords = new List<FreshKillRecord>();
            foreach (FreshKill kill in freshKill)
            {
                FreshKillRecord record = new FreshKillRecord();
                record.freshKill = kill.species;
                killRecords.Add(record);
            }

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<FreshKillRecord>();
            csvWriter.NextRecord(); // adds new line after header

            csvWriter.WriteRecords(killRecords);
            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());
            string path = save + "FreshKillPile.csv";
            File.WriteAllText(path, res);
            //Console.WriteLine(File.ReadAllText(path));
            return (killRecords.ToArray());
        }

        public WorldStatus CreateWorld(string save)
        {
            WorldStatus status = new WorldStatus();
            status.day = 1;
            status.season = World.GetSeason(1);
            status.saveName = save;
            return status;
        }

        public static void SaveGame(Clan inputClan, WorldStatus worldStatus, string saveName, TileMap tileMap)
        {
            //Console.WriteLine("Saving...");
            string folder = Directory.GetCurrentDirectory();
            string subfolder = Path.Combine(Directory.GetCurrentDirectory(), saveName);
            string newFolder;
            if (Directory.Exists(subfolder))
            {
                newFolder = subfolder;
            }
            else
            {
                newFolder = Directory.CreateDirectory(subfolder).FullName;
            }
            
            string name = saveName; 
            string path = Path.Combine(newFolder, (saveName));
            //Console.WriteLine(path);
            worldStatus.saveName = path;
            string worldPath = Path.Combine(Path.GetFileNameWithoutExtension(path), "World.csv");
            //Console.WriteLine(Path.GetFullPath(path));
            //File.WriteAllText("SaveFileName.txt", path);
            Save.SaveWorld(worldStatus);
            
            string pathSave = Path.Combine(folder, "Saves.csv");

            if(File.Exists(pathSave))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };

                List<Game> recordsList;
                using var streamReader3 = File.OpenText(pathSave);
                using var csvReader3 = new CsvReader(streamReader3, CultureInfo.CurrentCulture);
                var recordsSave = csvReader3.GetRecords<Game>();
                recordsList = recordsSave.ToList();
                streamReader3.Close();
                List<string> stringList = new List<string>();
                foreach(Game game in recordsList)
                {
                    stringList.Add(game.saveName);
                }
                if(!stringList.Contains(name))
                {
                    using (var stream = File.Open(pathSave, FileMode.Append))
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        //var records2 = csv.GetRecords<Game>();
                        Game game = new Game();
                        game.saveName = name;
                        Game[] games = { game };



                        //csvWriter2.WriteHeader<Game>();
                        //csvWriter2.NextRecord(); // adds new line after header

                        csv.WriteRecords(games);
                        writer.Flush();
                    }
                }

                
            }
            else
            {
                Game game = new Game();
                game.saveName = name;
                Game[] games = { game };


                using var writer2 = new StreamWriter(pathSave);
                using var csvWriter2 = new CsvWriter(writer2, CultureInfo.CurrentCulture);

                csvWriter2.WriteHeader<Game>();
                csvWriter2.NextRecord(); // adds new line after header

                csvWriter2.WriteRecords(games);
                writer2.Flush();
            }



            string loadPath = Path.Combine(newFolder, name);
            
            using var streamReader = File.OpenText(worldStatus.saveName + "World.csv");
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            var worlds = csvReader.GetRecords<WorldSaveFile>();

            List<WorldSaveFile> worldsList = worlds.ToList();


            CatRecord[] records = Save.SaveCats(inputClan.clanCats.ToArray(), path);

            using var streamReader2 = File.OpenText(path + "Cats.csv");
            using var csvReader2 = new CsvReader(streamReader2, CultureInfo.CurrentCulture);

            var cats = csvReader2.GetRecords<CatRecord>();
            //Console.WriteLine(records.Length);
            List<Cat> catsList = new List<Cat>();
            //Console.WriteLine(cats.GetType());
            var catRecords = cats.ToList();
            foreach(CatRecord cat in catRecords) {
                StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                Skills skills = new Skills(cat.hunt, cat.med);
                Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), skills, cat.energy, cat.health);
                //Console.WriteLine(newCat.name);
                catsList.Add(newCat);
            }
            FreshKillRecord[] killRecords = Save.SaveFreshKillPile(inputClan.freshKillPile.ToArray(), path);
            List<FreshKill> killList = new List<FreshKill>();
            foreach(var r in killRecords)
            {
                killList.Add(new FreshKill(Prey.prey[r.freshKill]));
            }
            FreshKill[] freshKillPile = killList.ToArray();
            ItemRecord[] itemRecords = Save.SaveItems(inputClan.items.ToArray(), path);
            List<Item> itemList = new List<Item>();
            foreach (var r in itemRecords)
            {
                Item item = new Item();
                item.type = r.type;
                item.description = r.desc;
                item.usable = r.usable;
                itemList.Add(item);
            }
            //Clan testClan = new Clan("Testclan", Biomes.biomes[0], catsList);
            ClanRecord clanRecord = Save.SaveClan(inputClan, path);
            MapRecord mapRecord = Save.SaveMap(tileMap.tileMap, tileMap.chars, path);
            //Console.WriteLine("Finished saving");
            if(Directory.Exists(Path.Combine(newFolder, "Patrol")))
            {
                Directory.Delete(Path.Combine(newFolder, "Patrol"), true);
            }
        }

        public static void SaveGame(Clan inputClan, WorldStatus worldStatus, string saveName, TileMap tileMap, Patrol patrol)
        {
            //Console.WriteLine("Saving...");
            string folder = Directory.GetCurrentDirectory();
            string subfolder = Path.Combine(Directory.GetCurrentDirectory(), saveName);
            string newFolder;
            if (Directory.Exists(subfolder))
            {
                newFolder = subfolder;
            }
            else
            {
                newFolder = Directory.CreateDirectory(subfolder).FullName;
            }

            string name = saveName;
            string path = Path.Combine(newFolder, (saveName));
            //Console.WriteLine(path);
            worldStatus.saveName = path;
            string worldPath = Path.Combine(Path.GetFileNameWithoutExtension(path), "World.csv");
            //Console.WriteLine(Path.GetFullPath(path));
            //File.WriteAllText("SaveFileName.txt", path);
            Save.SaveWorld(worldStatus);

            string pathSave = Path.Combine(folder, "Saves.csv");

            if (File.Exists(pathSave))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };

                List<Game> recordsList;
                using var streamReader3 = File.OpenText(pathSave);
                using var csvReader3 = new CsvReader(streamReader3, CultureInfo.CurrentCulture);
                var recordsSave = csvReader3.GetRecords<Game>();
                recordsList = recordsSave.ToList();
                streamReader3.Close();
                List<string> stringList = new List<string>();
                foreach (Game game in recordsList)
                {
                    stringList.Add(game.saveName);
                }
                if (!stringList.Contains(name))
                {
                    using (var stream = File.Open(pathSave, FileMode.Append))
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        //var records2 = csv.GetRecords<Game>();
                        Game game = new Game();
                        game.saveName = name;
                        Game[] games = { game };



                        //csvWriter2.WriteHeader<Game>();
                        //csvWriter2.NextRecord(); // adds new line after header

                        csv.WriteRecords(games);
                        writer.Flush();
                    }
                }


            }
            else
            {
                Game game = new Game();
                game.saveName = name;
                Game[] games = { game };


                using var writer2 = new StreamWriter(pathSave);
                using var csvWriter2 = new CsvWriter(writer2, CultureInfo.CurrentCulture);

                csvWriter2.WriteHeader<Game>();
                csvWriter2.NextRecord(); // adds new line after header

                csvWriter2.WriteRecords(games);
                writer2.Flush();
            }



            string loadPath = Path.Combine(newFolder, name);

            using var streamReader = File.OpenText(worldStatus.saveName + "World.csv");
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            var worlds = csvReader.GetRecords<WorldSaveFile>();

            List<WorldSaveFile> worldsList = worlds.ToList();


            CatRecord[] records = Save.SaveCats(inputClan.clanCats.ToArray(), path);

            using var streamReader2 = File.OpenText(path + "Cats.csv");
            using var csvReader2 = new CsvReader(streamReader2, CultureInfo.CurrentCulture);

            var cats = csvReader2.GetRecords<CatRecord>();
            //Console.WriteLine(records.Length);
            List<Cat> catsList = new List<Cat>();
            //Console.WriteLine(cats.GetType());
            var catRecords = cats.ToList();
            foreach (CatRecord cat in catRecords)
            {
                StatBlock stats = new StatBlock(cat.agi, cat.str, cat.intl, cat.fin, cat.ten, cat.con);
                Skills skills = new Skills(cat.hunt, cat.med);
                Cat newCat = new Cat(cat.prefix, cat.suffix, Defs.dictRank[cat.rankAssigner], Defs.dictGender[cat.genderAssigner], cat.age, stats, cat.personality.Split('&'), skills, cat.energy, cat.health,null,null, null, cat.preg);
                //Console.WriteLine(newCat.name);
                catsList.Add(newCat);
            }
            FreshKillRecord[] killRecords = Save.SaveFreshKillPile(inputClan.freshKillPile.ToArray(), path);
            List<FreshKill> killList = new List<FreshKill>();
            foreach (var r in killRecords)
            {
                killList.Add(new FreshKill(Prey.prey[r.freshKill]));
            }
            FreshKill[] freshKillPile = killList.ToArray();
            ItemRecord[] itemRecords = Save.SaveItems(inputClan.items.ToArray(), path);
            List<Item> itemList = new List<Item>();
            foreach (var r in itemRecords)
            {
                Item item = new Item();
                item.type = r.type;
                item.description = r.desc;
                item.usable = r.usable;
                itemList.Add(item);
            }
            //Clan testClan = new Clan("Testclan", Biomes.biomes[0], catsList);
            ClanRecord clanRecord = Save.SaveClan(inputClan, path);
            MapRecord mapRecord = Save.SaveMap(tileMap.tileMap, tileMap.chars, path);
            //Console.WriteLine("Finished saving");

            if(patrol.patrolCats != null)
            {
                SavePatrol(patrol, newFolder);
            }
        }
    }

    public record WorldSaveFile
    {
        public int day { get; set; }
        public World.Season season { get; set; }
        public string saveName { get; set; }
        public string weather { get; set; }
    }

    public record CatRecord
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
        public string species { get; set; }
        public string rankAssigner { get; set; }
        public string genderAssigner { get; set; }
        public int energy { get; set; }
        public int age { get; set; }
        public float health { get; set; }
        public float agi { get; set; }
        public float con { get; set; }
        public float fin { get; set; }
        public float intl { get; set; }
        public float str { get; set; }
        public float ten { get; set; }
        public string personality { get; set; }
        public float hunt { get; set; }
        public float med { get; set; }
        public int preg { get; set; }
    }

    public record Game
    {
        public string saveName { get; set; }

    }

    public record FreshKillRecord
    {
        public string freshKill { get; set; }
    }

    public record ClanRecord
    {
        public string clanName { get; set; }
        public string biome { get; set; }
        public string saveName { get; set; }
    }

    public record MapRecord
    {
        public string[,] map { get; set; }
        //public Char[,] chars { get; set; }
        //public string saveName { get; set; }
    }

    public record ItemRecord
    {
        public string type { get; set; }
        public string desc { get; set; }
        public bool usable { get; set; }
    }

}
