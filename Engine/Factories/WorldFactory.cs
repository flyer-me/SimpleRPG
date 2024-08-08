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
            newWorld.AddLocation(0, -1, "Home", "This is your home.", "pack://application:,,,/Engine;component/Images/Locations/Home.png");

            newWorld.AddLocation(-2, -1, "农田",
               "There are rows of corn growing here, with giant rats hiding between them.",
               "pack://application:,,,/Engine;component/Images/Locations/FarmFields.png");

            newWorld.LocationAt(-2, -1).UpdateMonsterEncounter(2, 100);

            newWorld.AddLocation(-1, -1, "Farmer's House",
                "This is the house of your neighbor, Farmer Ted.",
                "pack://application:,,,/Engine;component/Images/Locations/Farmhouse.png");

            newWorld.AddLocation(-1, 0, "Trading Shop",
                "The shop of Susan, the trader.",
                "pack://application:,,,/Engine;component/Images/Locations/Trader.png");

            newWorld.AddLocation(0, 0, "Town square",
                "You see a fountain here.",
                "pack://application:,,,/Engine;component/Images/Locations/TownSquare.png");

            newWorld.AddLocation(1, 0, "Town Gate",
                "There is a gate here, protecting the town from giant spiders.",
                "pack://application:,,,/Engine;component/Images/Locations/TownGate.png");

            newWorld.AddLocation(2, 0, "蜘蛛森林",
                "The trees in this forest are covered with spider webs.",
                "pack://application:,,,/Engine;component/Images/Locations/SpiderForest.png");

            newWorld.LocationAt(2, 0).UpdateMonsterEncounter(3, 100);

            newWorld.AddLocation(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                "pack://application:,,,/Engine;component/Images/Locations/HerbalistsHut.png");

            newWorld.AddLocation(0, 2, "草药师的花园",
                "草木茂盛，有蛇盘踞其中。",
                "pack://application:,,,/Engine;component/Images/Locations/HerbalistsGarden.png");

            newWorld.LocationAt(0, 2).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));
            newWorld.LocationAt(0, 2).UpdateMonsterEncounter(1, 100);

            return newWorld;
        }
    }
}
