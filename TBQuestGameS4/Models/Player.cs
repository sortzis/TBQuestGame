using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TBQuestGame.Models
{
    public class Player : Character, IBattle
    {
        #region ENUMS
        public enum NPCTitle { Friend, Enemy }

        #endregion

        private const int DEFENDER_DAMAGE_ADJUSTMENT = 5;
        private const int MAXIMUM_RETREAT_DAMAGE = 10;

        #region FIELDS

        private int _lives;
        private int _health;
        private int _experiencePoints;
        private NPCTitle _title;
        private int _skillLevel;
        private Weapon _currentWeapon;
        private BattleModeName _battleMode;
        private List<Location> _locationsVisited;
        private ObservableCollection<GameItem> _inventory;
        private ObservableCollection<GameItem> _healthObjects;
        private ObservableCollection<GameItem> _weapons;
        private ObservableCollection<GameItem> _idols;

        #endregion

        #region PROPERITES

        public NPCTitle Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        #endregion

        #region PROPERITES

        public void SetNPCTitle(NPCTitle value)
        { _title = value; }

        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                OnPropertyChanged(nameof(Lives));
            }
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                
                if (_health > 100)
                {
                    _health = 100;
                }
                else if (_health <= 0)
                {
                    _health = 100;
                    _lives--;
                }

                OnPropertyChanged(nameof(Health));
            }
        }

        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged(nameof(ExperiencePoints));
            }
        }

        public int SkillLevel
        {
            get { return _skillLevel; }
            set { _skillLevel = value; }
        }

        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set { _currentWeapon = value; }
        }

        public BattleModeName BattleMode
        {
            get { return _battleMode; }
            set { _battleMode = value; }
        }

        public List<Location> LocationsVisited
        {
            get { return _locationsVisited; }
            set { _locationsVisited = value; }
        }

        public ObservableCollection<GameItem> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public ObservableCollection<GameItem> Weapons
        {
            get { return _weapons; }
            set { _weapons = value; }
        }

        public ObservableCollection<GameItem> HealthObjects
        {
            get { return _healthObjects; }
            set { _healthObjects = value; }
        }

        public ObservableCollection<GameItem> Idols
        {
            get { return _idols; }
            set { _idols = value; }
        }


        #endregion

        #region CONSTRUCTORS

        public Player()
        {
            _locationsVisited = new List<Location>();
            _weapons = new ObservableCollection<GameItem>();
            _healthObjects = new ObservableCollection<GameItem>();
            _idols = new ObservableCollection<GameItem>();
            _inventory = new ObservableCollection<GameItem>();
            _skillLevel = 10;
        }

        #endregion

        #region METHODS

        public bool HasVisited(Location location)
        {
            return _locationsVisited.Contains(location);
        }

        public void UpdateInventoryCategories()
        {
            Weapons.Clear();
            HealthObjects.Clear();
            Idols.Clear();

            foreach (var gameItem in _inventory)
            {
                if (gameItem is Health) HealthObjects.Add(gameItem);
                if (gameItem is Weapon) Weapons.Add(gameItem);
                if (gameItem is Idol) Idols.Add(gameItem);
            }
        }

        public void AddGameItemToInventory(GameItem selectedGameItem)
        {
            if (selectedGameItem != null)
            {
                _inventory.Add(selectedGameItem);
            }
        }

        public void RemoveGameItemFromInventory(GameItem selectedGameItem)
        {
            if (selectedGameItem != null)
            {
                _inventory.Remove(selectedGameItem);
            }
        }

        /// <summary>
        /// override the default greeting in the Character class to include the job title
        /// set the proper article based on the job title
        /// </summary>
        /// <returns>default greeting</returns>
        public override string DefaultGreeting()
        {
            string article = "a";

            List<string> vowels = new List<string>() { "A", "E", "I", "O", "U" };

            if (vowels.Contains(_title.ToString().Substring(0, 1)))
            {
                article = "an";
            }

            return $"Hello, my name is {_name} and I am {article} {_title} of you in these dungeons.";
        }

        #endregion

        #region BATTLE METHODS

        /// <summary>
        /// return hit points [0 - 100] based on the player's weapon and skill level
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Attack()
        {
            int hitPoints = random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * _skillLevel;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// return hit points [0 - 100] based on the player's weapon and skill level
        /// adjusted to deliver more damage when first attacked
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Defend()
        {
            int hitPoints = (random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * _skillLevel) - DEFENDER_DAMAGE_ADJUSTMENT;

            if (hitPoints >= 0 && hitPoints <= 100)
            {
                return hitPoints;
            }
            else if (hitPoints > 100)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// return hit points [0 - 100] based on the player's skill level
        /// </summary>
        /// <returns>hit points 0-100</returns>
        public int Retreat()
        {
            int hitPoints = _skillLevel * MAXIMUM_RETREAT_DAMAGE;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        #endregion
    }
}
