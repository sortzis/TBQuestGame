using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private static GameItem GameItemById(int id)
        {
            return StandardGameItems().FirstOrDefault(i => i.Id == id);
        }

        private static Npc NpcById(int id)
        {
            return Npcs().FirstOrDefault(i => i.Id == id);
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

            gameMap.StandardGameItems = StandardGameItems();

            //
            // level 1
            //
            gameMap.MapLocations[0] = new Location()
            {
                Id = 1,
                Name = "Dungeon Level 1",
                Description = "This is level one of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5,
                GameItems = new ObservableCollection<GameItem>
                {
                    GameItemById(1001)
                }
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
                ModifyHealth = -10,
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(2001)
                }
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
                ModifyExperiencePoints = 5,
                GameItems = new ObservableCollection<GameItem>
                {
                    GameItemById(1004)
                }
            };

            gameMap.MapLocations[5] = new Location()
            {
                Id = 6,
                Name = "Dungeon Level 6",
                Description = "This is level six of the dungeon",
                Accessible = true,
                ModifyExperiencePoints = 5,
                GameItems = new ObservableCollection<GameItem>
                {
                    GameItemById(1002),
                    GameItemById(1003)
                },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(1001)
                }
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
                ModifyExperiencePoints = 5,
                GameItems = new ObservableCollection<GameItem>
                {
                    GameItemById(1003)
                }
            };

            gameMap.MapLocations[9] = new Location()
            {
                Id = 10,
                Name = "Dungeon Level 10",
                Description = "This is the final level of the dungeon",
                Accessible = false,
                RequiredIdolId = 1004,
                ModifyExperiencePoints = 5,
                Npcs = new ObservableCollection<Npc>
                {
                    NpcById(2002)
                }
            };

            return gameMap;
        }

        public static List<GameItem> StandardGameItems()
        {
            return new List<GameItem>()
            {
                new Weapon(1001, "Dagger", 5, 10, 25, "The dagger is a lightweight weapon used for basic combat.", 20),
                new Weapon(1002, "Throwing Spear", 5, 30, 50, "The throwing spear is a long range weapon used for intense combat.", 50),
                new Health(1003, "Health Pot", 5, 50, 1, "A basic health potion used for health and life regen.", 10),
                new Idol(1004, "Eagle Crest", 5, "An Eagle Crest that seems like it may be used to unlock something", 10, "You inserted the Eagle Crest into the locked chamber door", Idol.UseActionType.OPENLOCATION)
            };
        }

        public static List<Npc> Npcs()
        {
            return new List<Npc>()
            {
                new Enemy()
                {
                    Id = 2001,
                    Name = "Gargoyle",
                    Job = Character.JobType.Hunter,
                    Description = "A ferocious beast",
                    Messages = new List<string>()
                    {
                        "So you thought you could just get by me?",
                        "I will end you"
                    },
                    SkillLevel = 10,
                    CurrentWeapon = GameItemById(1001) as Weapon
                },

                new Enemy()
                {
                    Id = 2002,
                    Name = "Giant",
                    Job = Character.JobType.Looter,
                    Description = "An angry giant that's guarding the exit",
                    Messages = new List<string>()
                    {
                        "What are you doing in this dungeon?",
                        "So you think you can get by me?"
                    },
                    SkillLevel = 100,
                    CurrentWeapon = GameItemById(1002) as Weapon
                },

                new Citizen()
                {
                    Id = 1001,
                    Name = "Indiana",
                    Job = Character.JobType.Adventurer,
                    Description = "A kind man looking to help",
                    Messages = new List<string>()
                    {
                        "Looks like you're in a bit of trouble",
                        "The item you need is on level 4 of this dungeon"
                    }
                }
            };
        }
    }
}
