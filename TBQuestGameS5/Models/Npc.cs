using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public abstract class Npc : Character
    {
        public string Description { get; set; }

        public string Information
        {
            get
            {
                return InformationText();
            }
            set
            {

            }
        }

        public Npc()
        {

        }

        public Npc(int id, string name, JobType job, string description) : base(name, job, id)
        {
            Id = id;
            Name = name;
            Job = job;
            Description = description;
        }

        protected abstract string InformationText();
    }
}
