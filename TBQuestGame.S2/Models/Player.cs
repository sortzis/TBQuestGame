using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Player : Character
    {
        #region ENUMS
        public enum NPCTitle { Friend, Enemy }
        #endregion

        #region FIELDS

        private int _lives;
        private int _health;
        private int _experiencePoints;
        private NPCTitle _title;
        private List<Location> _locationsVisited;

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

        public List<Location> LocationsVisited
        {
            get { return _locationsVisited; }
            set { _locationsVisited = value; }
        }


        #endregion

        #region CONSTRUCTORS

        public Player()
        {
            _locationsVisited = new List<Location>();
        }

        #endregion

        #region METHODS

        public bool HasVisited(Location location)
        {
            return _locationsVisited.Contains(location);
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
    }
}
