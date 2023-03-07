using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanCreatorApp
{
    public class World
    {
        public static string[] springWeather =
    {
        "Clear",
        "Sunny",
        "Cloudy",
        "Partially Cloudy",
        "Drizzly",
        "Foggy",
        "Misty",
        "Light Rain",
        "Light Rain",
        "Heavy Rain"
    };

        public static string[] summerWeather =
        {
            "Clear",
            "Sunny",
            "Clear",
            "Sunny",
            "Clear",
            "Sunny",
            "Cloudy",
            "Partially Cloudy",
            "Partially Cloudy",
            "Partially Cloudy",
            "Drizzly",
            "Foggy",
            "Misty",
            "Light Rain",
            "Heavy Rain"
        };

        public static string[] fallWeather =
        {
            "Clear",
            "Sunny",
            "Cloudy",
            "Clear",
            "Sunny",
            "Cloudy",
            "Partially Cloudy",
            "Drizzly",
            "Foggy",
            "Misty",
            "Light Rain",
            "Heavy Rain",
            "Frosty"
        };

        public static string[] winterWeather =
        {
            "Clear",
            "Sunny",
            "Cloudy",
            "Cloudy",
            "Partially Cloudy",
            "Light Snow",
            "Heavy Snow",
            "Frosty",
            "Misty",
            "Light Snow",
            "Heavy Snow",
            "Frosty"
        };

        public enum Season
        {
            Newleaf,
            Greenleaf,
            LeafFall,
            Leafbare
        };

        public static Dictionary<Season, string> seasons = new Dictionary<Season, string>()
        {
            {Season.Newleaf, "Newleaf" },
            {Season.Greenleaf, "Greenleaf" },
            {Season.LeafFall, "Leaf-fall" },
            {Season.Leafbare, "Leafbare" }
        };

        public static Season GetSeason(int day)
        {
            while(day > 360)
            {
                day -= 360;
            }
            if(day <= 90)
            {
                return Season.Newleaf;
            }
            else if(day <= 180)
            {
                return Season.Greenleaf;
            }
            else if(day <= 270)
            {
                return Season.LeafFall;
            }
            else
            {
                return Season.Leafbare;
            };
        }

        public static float PreyChance(Season s)
        {
            switch(s)
            {
                case Season.Newleaf:
                    return 1.5f;
                case Season.Greenleaf:
                    return 1f;
                case Season.LeafFall:
                    return .75f;
                case Season.Leafbare:
                    return .25f;
                default:
                    return -1f;
            }
           
        }
        public static float PreyDC(Season s)
        {
            switch (s)
            {
                case Season.Newleaf:
                    return 4;
                case Season.Greenleaf:
                    return 5;
                case Season.LeafFall:
                    return 9;
                case Season.Leafbare:
                    return 12f;
                default:
                    return -1f;
            }

        }
        public static string GetRandomWeather(Season s)
        {
            Random rand = new Random(Utils.GetRandom());
            switch (s)
            {
                case Season.Newleaf:
                    return springWeather[rand.Next(0, springWeather.Length)];
                case Season.Greenleaf:
                    return summerWeather[rand.Next(0, summerWeather.Length)];
                case Season.LeafFall:
                    return fallWeather[rand.Next(0, fallWeather.Length)];
                case Season.Leafbare:
                    return winterWeather[rand.Next(0, winterWeather.Length)];
                default:
                    return "Error";
            }
            
        }
    }

    

    public struct WorldStatus
    {
        public string saveName;
        public int day;
        public World.Season season;
        public string weather;

        public WorldStatus(string name)
        {
            saveName = name;
            day = 1;
            season = World.GetSeason(day);
            weather = World.GetRandomWeather(season);
        }

        public WorldStatus(string name, int day1, string w)
        {
            saveName = name;
            day = day1;
            season = World.GetSeason(day);
            weather = w;
        }
    }

    
}
