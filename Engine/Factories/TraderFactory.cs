using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();
        static TraderFactory()
        {
            Trader susan = new Trader("琳娜");
            susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader farmerTed = new Trader("农夫陶德");
            farmerTed.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader peteTheHerbalist = new Trader("草药师特曼");
            peteTheHerbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            AddTraderToList(susan);
            AddTraderToList(farmerTed);
            AddTraderToList(peteTheHerbalist);
        }
        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.Name == name);
        }
        private static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException($"重复错误：名为'{trader.Name}'的Trader已存在.");
            }
            _traders.Add(trader);
        }
    }
}
