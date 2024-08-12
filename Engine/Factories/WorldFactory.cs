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
               "There are rows of corn growing here, with giant rats hiding between them.",
               "FarmFields.png");

            newWorld.LocationAt(-2, -1).UpdateMonsterEncounter(2, 100);

            newWorld.AddLocation(-1, -1, "Farmer's House",
                "This is the house of your neighbor, Farmer Ted.",
                "Farmhouse.png");

            newWorld.AddLocation(-1, 0, "Trading Shop",
                "The shop of Susan, the trader.",
                "Trader.png");

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

            newWorld.AddLocation(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                "HerbalistsHut.png");

            newWorld.AddLocation(0, 2, "草药师的花园",
                "草木茂盛，有蛇盘踞其中。",
                "HerbalistsGarden.png");

            newWorld.LocationAt(0, 2).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));
            newWorld.LocationAt(0, 2).UpdateMonsterEncounter(1, 100);

            return newWorld;
        }
    }
}
