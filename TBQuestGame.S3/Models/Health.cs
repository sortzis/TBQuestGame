using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Health : GameItem
    {
        public int HealthChange { get; set; }
        public int LivesChange { get; set; }

        public Health(int id, string name, int value, int healthChange, int livesChange, string description, int experiencePoints) : 
            base(id, name, value, description, experiencePoints)
        {
            HealthChange = healthChange;
            LivesChange = livesChange;
        }

        public override string InformationString()
        {
            if (HealthChange != 0)
            {
                return $"{Name}: {Description}\nHealth: {HealthChange}";
            }
            else if(LivesChange != 0)
            {
                return $"{Name}: {Description}\nLives: {LivesChange}";
            }
            else
            {
                return $"{Name}: {Description}";
            }
        }
    }
}
