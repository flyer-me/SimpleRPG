﻿using System.Xml;
using RPG.Models;
using RPG.Models.Shared;

namespace RPG.Services.Factories
{
    internal static class QuestFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";
        private static readonly List<Quest> _quests = new List<Quest>();
        static QuestFactory()
        {
            if(File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadQuestsFromNodes(XmlNodeList? nodes)
        {
            if (nodes == null)
            {
                return;
            }
            foreach(XmlNode node in nodes)
            {
                List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
                List<ItemQuantity> rewardItems = new List<ItemQuantity>();
                foreach(XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item")!)
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    itemsToComplete.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }
                foreach(XmlNode childNode in node.SelectNodes("./RewardItems/Item")!)
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    rewardItems.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }
                _quests.Add(new Quest(node.AttributeAsInt("ID"),
                                    node.SelectSingleNode("./Name")?.InnerText ?? "",
                                    node.SelectSingleNode("./Description")?.InnerText ?? "",
                                    itemsToComplete,
                                    node.AttributeAsInt("RewardExperiencePoints"),
                                    node.AttributeAsInt("RewardGold"),
                                    rewardItems));
            }
        }
        internal static Quest? GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
