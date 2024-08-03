using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : GameItem
    {
        public int MinimumDamage { get; private set; }
        public int MaximumDamage { get; private set; }
        public Weapon(int itemTypeID, string name, int price, int minimumDamage, int maximumDamage) 
            : base(itemTypeID, name, price)
        {
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
        }

        public new Weapon Clone()
        {
            return new Weapon(ItemTypeID, Name, Price, MinimumDamage, MaximumDamage);
        }
    }
}
