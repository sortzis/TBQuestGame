using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class MissionGather : Mission
    {
        private int _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;
        private int _experiencePoints;
        private List<GameItem> _requiredGameItem;

        public List<GameItem> RequiredGameItem
        {
            get { return _requiredGameItem; }
            set { _requiredGameItem = value; }
        }


        public MissionGather()
        {

        }

        public MissionGather(int id, string name, MissionStatus status, List<GameItem> requiredGameItem)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _requiredGameItem = requiredGameItem;
        }

        public List<GameItem> GameItemNotCompleted(List<GameItem> inventory)
        {
            List<GameItem> gameItemToComplete = new List<GameItem>();

            foreach (var missionGameItem in _requiredGameItem)
            {
                GameItem inventoryItemMatch = inventory.FirstOrDefault();
                if (inventoryItemMatch == null)
                {
                    gameItemToComplete.Add(missionGameItem);
                }
            }

            return gameItemToComplete;
        }
    }
}
