using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(0, -1, "Home", "This is your home.", "Home.png");

            newWorld.AddLocation(-2, -1, "农田",
               "此地玉米丰富，一些巨型老鼠藏于其中",
               "FarmFields.png");

            newWorld.LocationAt(-2, -1).UpdateMonsterEncounter(2, 100);

            newWorld.AddLocation(-1, -1, "农夫的家",
                "你的邻居，农夫陶德的家",
                "Farmhouse.png");
            newWorld.LocationAt(-1, -1).TraderHere = TraderFactory.GetTraderByName("农夫陶德");

            newWorld.AddLocation(-1, 0, "琳娜商店",
                "琳娜的商店，交易处",
                "Trader.png");
            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderByName("琳娜");

            newWorld.AddLocation(0, 0, "Town square",
                "You see a fountain here.",
                "TownSquare.png");

            newWorld.AddLocation(1, 0, "Town Gate",
                "There is a gate here, protecting the town from giant spiders.",
                "TownGate.png");

            newWorld.AddLocation(2, 0, "蜘蛛森林",
                "The trees in this forest are covered with spider webs.",
                "SpiderForest.png");
            newWorld.LocationAt(2, 0).UpdateMonsterEncounter(3, 100);

            newWorld.AddLocation(0, 1, "特曼的棚屋",
                "你看到一个房顶上放着干草药的棚屋",
                "HerbalistsHut.png");
            newWorld.LocationAt(0, 1).TraderHere = TraderFactory.GetTraderByName("草药师特曼");

            newWorld.AddLocation(0, 2, "草药师的花园",
                "草木茂盛，有蛇盘踞其中。",
                "HerbalistsGarden.png");
            newWorld.LocationAt(0, 2).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));
            newWorld.LocationAt(0, 2).UpdateMonsterEncounter(1, 100);

            return newWorld;
        }
    }
}
