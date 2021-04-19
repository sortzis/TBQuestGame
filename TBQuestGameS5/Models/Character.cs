using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Character : ObservableObject
    {
        #region ENUMERABLES

        public enum JobType
        {
            Adventurer,
            Explorer,
            Hunter,
            Looter
        }

        #endregion

        #region FIELDS

        protected int _id;
        protected string _name;
        protected int _locationId;
        protected JobType _job;

        protected Random random = new Random();

        #endregion

        #region PROPERTIES

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }

        public JobType Job
        {
            get { return _job; }
            set { _job = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Character()
        {

        }

        public Character(string name, JobType job, int locationId)
        {
            _name = name;
            _job = job;
            _locationId = locationId;
        }

        #endregion

        #region METHODS

        public virtual string DefaultGreeting()
        {
            return $"My name is {_name} and I am a {_job} in this dungeon.";
        }

        //Add abstract class here

        #endregion
    }
}