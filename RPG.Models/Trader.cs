using System.Collections.Generic;

namespace RPG.Models
{
    public class Trader : LivingEntity
    {
        public int ID { get; }
        public Trader(int id, string name): base(name, 999, 999, new List<PlayerAttribute>(), 999)
        {
            ID = id;
        }
    }
}