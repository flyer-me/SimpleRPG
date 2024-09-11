using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Engine.Services
{
    #pragma warning disable CS8600, CS8602, CS8604
    public class SaveService
    {
        public static void Save(GameState gameState ,string fileName)
        {
            var json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
        public static GameState LoadSaveOrCreate(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"File not found: {fileName}");
            }
            try
            {
                JObject data = JObject.Parse(File.ReadAllText(fileName));
                Player player = CreatePlayer(data);
                int x = (int)data[nameof(GameState.XCoordinate)];
                int y = (int)data[nameof(GameState.YCoordinate)];
                return new GameState(player, x, y);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Invalid data file: ", ex);
            }
        }
        private static Player CreatePlayer(JObject data)
        {
            Player player =
                new Player((string)data[nameof(GameState.Player)][nameof(Player.Name)],
                            (int)data[nameof(GameState.Player)][nameof(Player.ExperiencePoints)],
                            (int)data[nameof(GameState.Player)][nameof(Player.MaximumHitPoints)],
                            (int)data[nameof(GameState.Player)][nameof(Player.CurrentHitPoints)],
                            GetPlayerAttributes(data),
                            (int)data[nameof(GameState.Player)][nameof(Player.Assets)]);
            PopulatePlayerInventory(data, player);
            PopulatePlayerQuests(data, player);
            PopulatePlayerRecipes(data, player);
            return player;
        }
        private static IEnumerable<PlayerAttribute> GetPlayerAttributes(JObject data)
        {
            List<PlayerAttribute> playerAttributes = [];
            foreach (JToken item in (JArray)data[nameof(GameState.Player)]
                [nameof(Player.Attributes)])
            {
                playerAttributes.Add(new PlayerAttribute(
                                (string)item[nameof(PlayerAttribute.Key)],
                                (string)item[nameof(PlayerAttribute.DisplayName)],
                                (string)item[nameof(PlayerAttribute.ValueRange)],
                                (int)item[nameof(PlayerAttribute.BaseValue)],
                                (int)item[nameof(PlayerAttribute.ModifiedValue)]));
            }
            return playerAttributes;
        }
        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            foreach(JToken itemToken in (JArray)data[nameof(GameState.Player)]
                [nameof(Player.Inventory)]
                [nameof(Inventory.Items)])
            {
                int itemId = (int)itemToken[nameof(GameItem.ItemTypeID)];
                player.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }
        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            foreach(JToken questToken in (JArray)data[nameof(GameState.Player)]
                [nameof(Player.Quests)])
            {
                int questId =
                    (int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.ID)];
                Quest quest = QuestFactory.GetQuestByID(questId);
                QuestStatus questStatus = new QuestStatus(quest);
                questStatus.IsCompleted = (bool)questToken[nameof(QuestStatus.IsCompleted)];
                player.Quests.Add(questStatus);
            }
        }
        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            foreach(JToken recipeToken in
                (JArray)data[nameof(GameState.Player)][nameof(Player.Recipes)])
            {
                int recipeId = (int)recipeToken[nameof(Recipe.ID)];
                Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                player.Recipes.Add(recipe);
            }
        }
    }
}