using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;

namespace TBQuestGame.DataLayer
{
    public class GameData
    {
        public static Player PlayerData()
        {
            return new Player()
            {
                Id = 1,
                Name = "Janice",
                Job = Character.JobType.Adventurer,
                Health = 100,
                Lives = 3,
                ExperiencePoints = 0,
                LocationId = 0
            };
        }

        public static List<string> InitializeMessages()
        {
            return new List<string>()
            {
                "\tYou found yourself in a desolate cave with different levels."
            };
        }

        public static GameMapCoordinates InitialGameMapLocation()
        {
            return new GameMapCoordinates() { Row = 0};
        }

        public static Map GameMap()
        {
            int rows = 10;

            Map gameMap = new Map(rows);

            //
            // level 1
            //
            gameMap.MapLocations[0] = new Location()
            {
                Id = 1,
                Name = "Dungeon Level 1",
                Description = "This is level one of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[1] = new Location()
            {
                Id = 2,
                Name = "Dungeon Level 2",
                Description = "This is level two of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 15,
                RequiredExperiencePoints = 5
            };

            gameMap.MapLocations[2] = new Location()
            {
                Id = 3,
                Name = "Dungeon Level 3",
                Description = "This is level three of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 10,
                RequiredExperiencePoints = 10,
                ModifyHealth = -10
            };

            gameMap.MapLocations[3] = new Location()
            {
                Id = 4,
                Name = "Dungeon Level 4",
                Description = "This is level four of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[4] = new Location()
            {
                Id = 5,
                Name = "Dungeon Level 5",
                Description = "This is level five of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[5] = new Location()
            {
                Id = 6,
                Name = "Dungeon Level 6",
                Description = "This is level six of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[6] = new Location()
            {
                Id = 7,
                Name = "Dungeon Level 7",
                Description = "This is level seven of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[7] = new Location()
            {
                Id = 8,
                Name = "Dungeon Level 8",
                Description = "This is level eight of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[8] = new Location()
            {
                Id = 9,
                Name = "Dungeon Level 9",
                Description = "This is level nine of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            gameMap.MapLocations[9] = new Location()
            {
                Id = 10,
                Name = "Dungeon Level 10",
                Description = "This is the final level of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5
            };

            return gameMap;
        }
    }
}
