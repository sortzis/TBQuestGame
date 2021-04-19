using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;

namespace TBQuestGame.Models
{
    public interface IBattle
    {
        int SkillLevel { get; set; }

        Weapon CurrentWeapon { get; set; }

        BattleModeName BattleMode { get; set; }

        int Attack();
        int Defend();
        int Retreat();
    }
}
