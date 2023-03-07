using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ClanCreatorApp
{
    public struct Tile
    {
        //public Char character;
        public Biome biome;

        public Tile(Biome b)
        {
            //character = c;
            biome = b;
        }
    }

    public struct TileMap
    {
        public Tile[,] tileMap;
        public List<Biome> biomeList;
        public Char[,] chars;
        public Biome playerBiome;
        public List<Biome> choices;
        //public Clan[,] territoryOwners;


        public TileMap(Tile[,] tiles, Char[,] cs, Biome pb, List<Biome> biomeChoices)
        {
            tileMap = tiles;
            biomeList = new List<Biome>();
            chars = cs;
            playerBiome = pb;
            choices = biomeChoices;
        }

        public TileMap(Tile[,] tiles, List<Biome> b, Char[,] cs, Biome pb, List<Biome> biomeChoices)
        {
            tileMap = tiles;
            biomeList = b;
            chars = cs;
            playerBiome = pb;
            choices = biomeChoices;
        }
    }

    public struct PlayerPosition
    {
        public int playerX;
        public int playerY;

        public PlayerPosition(int x, int y)
        {
            playerX = x;
            playerY = y;
        }
    }

    public struct Position
    {
        public int x;
        public int y;

        public Position(int ex, int ey)
        {
            x = ex;
            y = ey;
        }
    }
    public class Map
    {
        ConsoleKey[] upKeys = { ConsoleKey.UpArrow, ConsoleKey.W };
        ConsoleKey[] leftKeys = { ConsoleKey.LeftArrow, ConsoleKey.A };
        ConsoleKey[] rightKeys = { ConsoleKey.RightArrow, ConsoleKey.D };
        ConsoleKey[] downKeys = { ConsoleKey.DownArrow, ConsoleKey.S };
        public bool playing = true;
        public int moveCounter = 0;
        public enum MoveDirection
        {
            Right,
            Left,
            Up,
            Down,
            None
        }

        //Map
        public int sizeX;
        public int sizeY;
        public TileMap tileMap;
        PlayerPosition playerPos;

        public Map(int x, int y, TileMap map)
        {
            sizeX = x;
            sizeY = y;
            tileMap = map;
            playerPos = new PlayerPosition(sizeX / 2 - 1, sizeY / 2 - 1);
        }


        public MoveDirection GetDirection(ConsoleKey key)
        {
            
            //ConsoleKey key = Console.ReadKey(true).Key;
            //Console.WriteLine("Loop");
            if (upKeys.Contains(key))
            {
                return MoveDirection.Up;
            }
            else if(downKeys.Contains(key))
            {
                return MoveDirection.Down;
            }
            else if(rightKeys.Contains(key))
            {
                return MoveDirection.Right;
            }
            else if(leftKeys.Contains(key))
            {
                return MoveDirection.Left;
            }
            else
            {
                return MoveDirection.None;
            }
        }

        public void RandomEncounter(Cat[] cats, Clan clan, Patrol patrol)
        {
            RandomEncounters rand = new RandomEncounters();
            rand.RandomEncounter(cats, clan, patrol);
        }
        public PlayerPosition Move(MoveDirection dir, PlayerPosition pos)
        {
            //Console.WriteLine("Move");
            switch(dir)
            {
                case MoveDirection.Down:
                    moveCounter++;
                    pos.playerX += 1;
                    return pos;
                case MoveDirection.Up:
                    moveCounter++;
                    pos.playerX -= 1;
                    return pos;
                case MoveDirection.Right:
                    moveCounter++;
                    pos.playerY += 1;
                    return pos;
                case MoveDirection.Left:
                    moveCounter++;
                    pos.playerY -= 1;
                    return pos;
                case MoveDirection.None:
                    return pos;
            }
            
            return pos;
        }
        public TileMap CreateTileMap(int ex, int ey, Biome pb, List<Biome> biomes)
        {
            //Console.WriteLine("Biomes: " + biomes.Count);
            TileMap newMap = new TileMap(new Tile[ex, ey], new Char[ex, ey], pb, biomes);
            playerPos = new PlayerPosition(sizeX / 2 - 1, sizeY / 2 - 1);
            /*for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    tileMap[x, y] = new Tile('X', Biomes.biomes[0]);
                }
            }*/
            for (int x = 0; x < newMap.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < newMap.tileMap.GetLength(1); y++)
                {

                    //newMap.tileMap[x, y].character = '#';
                    newMap.chars[x, y] = '#';
                   //output += tileMap[x, y].character;

                }
                //output += "\n";
            }
            
            //Add the biomes
            Random rand = new Random();
            //List<int> usedNums = new List<int>();
            //List<Char> charsToUse = new List<Char>();
            foreach(Biome b in biomes)
            {
                
                //int num = rand.Next(0, Biomes.biomes.Length );


                newMap.biomeList.Add(b);

            }
            
            List<Position> startPosList = new List<Position>();
            for (int i = 0; i < newMap.biomeList.Count; i++)
            {
                int posX;
                int posY;
                if (newMap.biomeList[i].name == pb.name)
                {
                    posX = ex / 2 - 1;
                    posY = ey / 2 - 1;
                }
                else
                {
                    posX = rand.Next(0, sizeX);
                    posY = rand.Next(0, sizeY);
                }
                
                Position center = new Position(posX, posY);
                startPosList.Add(center);
            }
            int size = 1;
            while (ContainsDefault(newMap.chars))
                {


                        
                        for (int i = 0; i < newMap.biomeList.Count; i++)
                        {
                            Biome biome = newMap.biomeList[i];
                            //if (CanExpand(biome, startPosList[i], size, newMap))
                            //{
                                ExpandFromTile(startPosList[i], biome, size, newMap.tileMap, newMap.chars);
                            //}
                        }
                        
                        size++;
                //Console.WriteLine("Size is " + size);
                    
                
            }


            tileMap = newMap;
            return newMap;
        }
            
            
        

        public bool ContainsDefault(Char[,] cs)
        {
            for (int x = 0; x < cs.GetLength(0); x++)
            {
                for (int y = 0; y < cs.GetLength(1); y++)
                {

                    if(cs[x, y] == '#')
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public Char RandomChar()
        {
            Char[] chars = "abcdefghijklmnopqrstuvwxyz^&".ToCharArray();
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }

        bool CanExpand(Biome biome, Position center, int size, TileMap map)
        {

            for (int x = center.x - size; x < center.x + size + 1; x++)
            {
                for (int y = center.y - size; y < center.y + size + 1; y++)
                {
                    if (x < map.tileMap.GetLength(0) && y < map.tileMap.GetLength(1) && x >= 0 && y >= 0)
                    {
                        if (map.tileMap[x, y].biome.name != biome.name && map.chars[x, y] != '#')
                        {
                            if (biome.name == map.biomeList[map.biomeList.Count - 1].name)
                            {
                                //finished = true;
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                }
            }
            return true;
        }

        public void ExpandFromTile(Position startPos, Biome biome, int size, Tile[,] map, Char[,] cs)
        {
            map[startPos.x, startPos.y].biome = biome;
            //map[startPos.x, startPos.y].character = c;
            cs[startPos.x, startPos.y] = biome.character;
            for (int x = startPos.x - size; x < startPos.x + size + 1; x++)
            {
                for (int y = startPos.y - size; y < startPos.y + size + 1; y++)
                {
                    if (x < map.GetLength(0) && y < map.GetLength(1) && x >= 0 && y >= 0)
                    {
                        if (cs[x, y] == '#')
                        {

                            //Debug.Log(x + ", " + dots.GetLength(0));
                            //Debug.Log(y + ", " + dots.GetLength(1));
                            map[x, y].biome = biome;
                            cs[x, y] = biome.character;
                            //blankCount -= 1;
                            //Debug.Log(x + ", " + y);
                            //Console.WriteLine("Expand " + c);
                        }

                        else if ((cs[x, y] != '#'))
                        {
                            //finished = true;
                            //Console.WriteLine(finished);
                        }
                    }

                }
            }
        }
        public bool CheckAlive(Patrol clan)
        {
            for (int i = 0; i < clan.patrolCats.Count; i++)
            {
                if (clan.patrolCats[i].health <= 0)
                {
                    Console.WriteLine(clan.patrolCats[i].name + " has perished. The patrol will now return home to bury them.");
                    clan.patrolCats.Remove(clan.patrolCats[i]);
                }
            }
            if(clan.patrolCats.Count <= 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public void MapDisplay(ref Patrol patrol, ref bool patrolling, Clan clan, CampLoop loop)
        {

            playerPos = new PlayerPosition(sizeX / 2 - 1, sizeY / 2 - 1);
            Console.Clear();
            while (CheckAlive(patrol))
            {
                while (AverageEnergy(patrol) > 0 && patrolling == true)
                {
                    
                    Char[] chars = { '#', '#' };
                    int patrolEnergy;
                    //Console.WriteLine("Map display");

                    Console.CursorVisible = false;
                    foreach (Char c in chars)
                    {
                        int originalX = Console.CursorLeft;
                        int originalY = Console.CursorTop;
                        string output = "";
                        for (int x = 0; x < tileMap.tileMap.GetLength(0); x++)
                        {
                            for (int y = 0; y < tileMap.tileMap.GetLength(1); y++)
                            {
                                if (x == playerPos.playerX && y == playerPos.playerY)
                                {
                                    tileMap.chars[x, y] = 'X';
                                    output += tileMap.chars[x, y];
                                }
                                else
                                {
                                    tileMap.chars[x, y] = tileMap.tileMap[x, y].biome.character;
                                    output += tileMap.tileMap[x, y].biome.character;
                                }
                            }
                            output += "\n";
                        }
                        output += "\n\nBiome: " + tileMap.tileMap[playerPos.playerX, playerPos.playerY].biome.name + " | Prey Types: " + Utils.CreateList(tileMap.tileMap[playerPos.playerX, playerPos.playerY].biome.preyTypes) + "\nPlayer X : " + playerPos.playerX + "   Player Y: " + playerPos.playerY + "\n";
                        if (moveCounter == 3)
                        {
                            RemoveEnergy(patrol);
                            moveCounter = 0;
                        }
                        patrolEnergy = AverageEnergy(patrol);
                        string energyString = "";

                        for (int i = 0; i < patrolEnergy; i++)
                        {
                            energyString += "E";
                        }
                        output += "Patrol Energy: " + patrolEnergy;
                        output += "\n[C]: Check Area | [G]: Go Home | [T]: Save and Quit";
                        Console.WriteLine(output);
                        //PatrolOptions(ref patrolling);
                        Console.SetCursorPosition(originalX, originalY);
                        //Curses.SetCursorYX(originalY, originalX);
                    }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (upKeys.Contains(key) || downKeys.Contains(key) || rightKeys.Contains(key) || leftKeys.Contains(key))
                        {
                            PlayerPosition testPos = Move(GetDirection(key), playerPos);
                            if (testPos.playerX < sizeX && testPos.playerY < sizeY && testPos.playerX >= 0 && testPos.playerY >= 0)
                            {
                                playerPos = testPos;
                                RandomEncounter(patrol.patrolCats.ToArray(), clan, patrol);
                            }
                        }
                        else if (key == ConsoleKey.C || key == ConsoleKey.G || key == ConsoleKey.T)
                        {
                            int option = PatrolOptions(ref patrolling, key, loop, patrol);
                            if (option != -1)
                            {
                                switch (option)
                                {
                                    case 0:

                                        SearchArea(patrol, tileMap.tileMap[playerPos.playerX, playerPos.playerY].biome, clan, loop);
                                        patrolEnergy = AverageEnergy(patrol);
                                        break;
                                    case 1:
                                        Console.WriteLine("Your patrol decides to head home.");
                                        Thread.Sleep(2000);
                                        patrolling = false;
                                        break;
                                    case 2:
                                        Console.Clear();
                                        Save.SaveGame(loop.activeClan, loop.activeWorld, loop.save, loop.activeMap.tileMap, patrol);
                                        Console.WriteLine("Saved successfully!");
                                        MainFile.PressEToContinue();
                                        Environment.Exit(0);
                                        break;
                                }
                                break;
                            }
                        }
                        
                    }
                    if (AverageEnergy(patrol) <= 0)
                    {
                        if (!CheckAlive(patrol))
                        {
                            //Console.WriteLine("The patrol has ended in tragedy.");
                            //MainFile.PressEToContinue();
                            patrolling = false;
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Your patrol is too tired to continue.");
                            patrolling = false;
                            Thread.Sleep(2000);
                            break;
                        }
                        

                    }
                }
                if (!patrolling)
                {
                    break;
                }
                if (AverageEnergy(patrol) <= 0 && CheckAlive(patrol))
                {
                    Console.Clear();
                    Console.WriteLine("Your patrol is too tired to continue.");
                    patrolling = false;
                    Thread.Sleep(2000);
                    break;
                }
            }
            if(!CheckAlive(patrol))
            {
                Console.WriteLine("The patrol has ended in tragedy.");
                MainFile.PressEToContinue();
                patrolling = false;
            }
            
            Console.Clear();
            patrolling = false;
            
        }

        public int PatrolOptions(ref bool patrolling, ConsoleKey key, CampLoop loop, Patrol patrol)
        {

            ConsoleKey checkPatrol;
            checkPatrol = key;

                if(checkPatrol == ConsoleKey.C)
                {
                    //patrolling = false;
                    return 0;
                }
                else if(checkPatrol == ConsoleKey.G)
                {
                    patrolling = false;
                    Console.Clear();
                    return 1;
                }
                else if(checkPatrol == ConsoleKey.T)
                {
                    return 2;
                
                }
                return -1;

        }

        public int AverageEnergy(Patrol patrol)
        {
            if(patrol.patrolCats.Count > 0)
            {
                int energy = 0;
                foreach (Cat cat in patrol.patrolCats)
                {
                    energy += cat.energy;
                }
                energy /= patrol.patrolCats.Count;
                return energy;
            }
            else
            {
                return -1;
            }
        }

        public void RemoveEnergy(Patrol patrol)
        {
            foreach (Cat cat in patrol.patrolCats)
            {
                cat.energy -= 1;
            }
            
        }

        public void SearchArea(Patrol patrol, Biome biome, Clan clan, CampLoop loop)
        {
            Console.Clear();

            Random rand = new Random(Utils.GetRandom());
            for (int i = 0; i < patrol.patrolCats.Count; i++)
            {
                Cat cat = patrol.patrolCats[i];
                int roll = rand.Next((int)World.PreyChance(loop.activeWorld.season), 5);
                //Console.WriteLine(roll);
                //MainFile.PressEToContinue();

                if (roll == 0)
                {
                    Console.WriteLine(cat.name + " has found nothing.");
                }
                else if (roll == 1 || roll == 2 || roll == 3)
                {
                    if (Hunting(patrol, biome, clan, loop.activeWorld, cat))
                    {

                    }
                    else
                    {
                        Console.WriteLine(cat.name + " has found nothing.");

                    }
                }
                else
                {
                    if (SearchHerb(patrol, biome, clan, loop.activeWorld, cat))
                    {

                    }
                    else
                    {
                        Console.WriteLine(cat.name + " has found nothing.");

                    }
                }
               
            }
            MainFile.PressEToContinue();
            Console.Clear();
            RemoveEnergy(patrol);
            
        }

        public bool SearchHerb(Patrol patrol, Biome biome, Clan clan, WorldStatus world, Cat cat)
        {
            //Console.Clear();
            Random rand = new Random(Utils.GetRandom());
            
                float roll = rand.Next(0, 21) + cat.skills.medicine + cat.statBlock.intelligence;
                if(roll >= World.PreyDC(world.season))
                {
                    Herb randomHerb = Herbs.herbs[rand.Next(0, Herbs.herbs.Length)];
                    Console.WriteLine(cat.name + " has managed to find some " + randomHerb.type + "!");
                    patrol.itemsHeld.Add(randomHerb);
                    
                    return true;
                }
            
            return false;
            
        }
        public bool Hunting(Patrol patrol, Biome biome, Clan clan, WorldStatus world, Cat cat)
        {
            bool foundPrey = false;
            //Console.Clear();
            Random rand = new Random(Utils.GetRandom());
            
                float roll = rand.Next(0, 21) + cat.skills.hunting;
                Prey prey = ChooseRandomPrey(biome);
                //float preySearchDc = World.PreyDC(world.season);
                //if (roll >= preySearchDc)
               // {
                    
                    if (prey.statBlock.intelligence >= 1)
                    {
                        Entity[] preyAr = { prey };
                        Cat[] catAr = patrol.patrolCats.ToArray();
                        CombatEncounter combatEncounter = new CombatEncounter(catAr, preyAr, clan);
                        combatEncounter.CombatLoop(patrol);
                        foundPrey = true;

                    }
                    else
                    {
                        PreyEncounter encounter = new PreyEncounter(prey.species, cat, clan, patrol);
                    
                        Console.WriteLine("\n");
                        foundPrey = true;
                    
                    }
                    
                //}
                
            
            
            return foundPrey;

        }

        public Prey ChooseRandomPrey(Biome biome)
        {
            Random rand = new Random(Utils.GetRandom());
            int num = rand.Next(0, biome.preyTypes.Length);
            Prey prey = Prey.prey[biome.preyTypes[num]];
            return prey;
        }

    }
    //5
}
