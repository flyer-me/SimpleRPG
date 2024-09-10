using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Title { get; }
        public string Version { get; }
        public List<PlayerAttribute> PlayerAttributes { get; } = [];
        public List<Race> Races { get; } = [];
        public GameDetails(string title, string version)
        {
            Title = title;
            Version = version;
        }
    }
}