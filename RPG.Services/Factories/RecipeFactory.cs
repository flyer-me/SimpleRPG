using System.Xml;
using RPG.Models;
using RPG.Models.Shared;

namespace RPG.Services.Factories
{
    public class RecipeFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Recipes.xml";
        private static readonly List<Recipe> _recipes = new List<Recipe>();
        static RecipeFactory()
        {
            if(File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadRecipesFromNodes(data.SelectNodes("/Recipes/Recipe"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadRecipesFromNodes(XmlNodeList? nodes)
        {
            if (nodes == null)
            {
                return;
            }
            foreach(XmlNode node in nodes)
            {
                List<ItemQuantity> ingredients = [];
                foreach(XmlNode childNode in node.SelectNodes("./Ingredients/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    ingredients.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }
                List<ItemQuantity> outputItems = [];
                foreach(XmlNode childNode in node.SelectNodes("./OutputItems/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    outputItems.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }
                Recipe recipe =
                    new Recipe(node.AttributeAsInt("ID"),
                               node.SelectSingleNode("./Name")?.InnerText ?? "",
                               ingredients, outputItems);
                _recipes.Add(recipe);
            }
        }
        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }
}