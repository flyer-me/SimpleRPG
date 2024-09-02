using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";
        internal static World CreateWorld()
        {
            World newWorld = new World();
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath = data.SelectSingleNode("/Locations").AttributeAsString("RootImagePath");
                LoadLocationsFromNodes(newWorld, rootImagePath, data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
            return newWorld;
        }
        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }
            foreach (XmlNode node in nodes)
            {
                Location location = new Location(node.AttributeAsInt("X"),
                                                node.AttributeAsInt("Y"),
                                                node.AttributeAsString("Name"),
                                                node.SelectSingleNode("./Description")?.InnerText ?? "",
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}");
                AddMonsters(location, node.SelectNodes("./Monsters/Monster"));
                AddQuests(location, node.SelectNodes("./Quests/Quest"));
                AddTrader(location, node.SelectSingleNode("./Trader"));
                world.AddLocation(location);
            }
        }

        private static void AddMonsters(Location location, XmlNodeList? xmlNodeList)
        {
            if (xmlNodeList == null)
            {
                return;
            }
            foreach (XmlNode node in xmlNodeList)
            {
                location.AddMonsterEncounter(node.AttributeAsInt("ID"),
                                             node.AttributeAsInt("Percent"));
            }
        }

        private static void AddQuests(Location location, XmlNodeList? xmlNodeList)
        {
            if(xmlNodeList == null)
            {
                return;
            }
            foreach(XmlNode node in xmlNodeList)
            {
                location.QuestsAvailableHere
                        .Add(QuestFactory.GetQuestByID(node.AttributeAsInt("ID")));
            }
        }

        private static void AddTrader(Location location, XmlNode? xmlNode)
        {
            if(xmlNode == null)
            {
                return;
            }
            location.TraderHere = TraderFactory.GetTraderByID(xmlNode.AttributeAsInt("ID"));
        }
    }
}
