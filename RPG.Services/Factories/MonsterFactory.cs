using System.Xml;
using RPG.Core;
using RPG.Models;
using RPG.Models.Shared;
namespace RPG.Services.Factories
{
    public static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";
        private static readonly GameDetails _gameDetails;
        private static readonly List<Monster> _baseMonsters = [];
        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                _gameDetails = GameDetailsService.ReadGameDetails();
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath =
                    data.SelectSingleNode("/Monsters").AttributeAsString("RootImagePath");
                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        public static Monster GetMonsterAtLocation(Location location)
        {
            if (location.MonstersHere.Count == 0)
            {
                return null;
            }
            int totalChances = location.MonstersHere.Sum(m => m.ChanceOfEncounter);
            int randomNumber = RandomGenerate.NumberBetween(1, totalChances);
            // 当累加数 ≥ 随机数(1，所有概率总和)时，返回
            int currentSum = 0;
            foreach (var monsterEncounter in location.MonstersHere)
            {
                currentSum += monsterEncounter.ChanceOfEncounter;
                if (randomNumber <= currentSum)
                {
                    return GetMonster(monsterEncounter.MonsterID);
                }
            }
            // 保证产生
            return GetMonster(location.MonstersHere.Last().MonsterID);
        }
        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }
            foreach (XmlNode node in nodes)
            {
                var attributes = _gameDetails.PlayerAttributes;
                attributes.First(a => a.Key.Equals("DEX")).BaseValue =
                    Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);
                attributes.First(a => a.Key.Equals("DEX")).ModifiedValue =
                    Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);
                Monster monster =
                    new Monster(node.AttributeAsInt("ID"),
                                node.AttributeAsString("Name"),
                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                node.AttributeAsInt("MaximumHitPoints"),
                                attributes,
                                ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
                                node.AttributeAsInt("RewardXP"),
                                node.AttributeAsInt("Gold"));
                XmlNodeList? lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"),
                                                   lootItemNode.AttributeAsInt("Percentage"));
                    }
                }
                _baseMonsters.Add(monster);
            }
        }
        public static Monster GetMonster(int id)
        {
            Monster monster = _baseMonsters.FirstOrDefault(m => m.ID == id).Clone();
            foreach (var itemPercentage in monster.LootTable)
            {
                if (RandomGenerate.NumberBetween(1, 100) <= itemPercentage.Percentage)
                {
                    monster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }
            return monster;
        }
    }
}