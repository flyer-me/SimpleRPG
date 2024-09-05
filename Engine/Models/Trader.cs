using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public int ID { get; }
        public Trader(int id, string name): base(name, 999, 999, 18, 999)
        {
            ID = id;
        }
    }
}