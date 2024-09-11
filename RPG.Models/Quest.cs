using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPG.Models
{
    public class Quest
    {
        public int ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsToComplete { get; }
        [JsonIgnore]
        public int RewardExperiencePoints { get; }
        [JsonIgnore]
        public int RewardAssets { get; }
        [JsonIgnore]
        public List<ItemQuantity> RewardItems { get; }
        [JsonIgnore]
        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
            "===========================" + Environment.NewLine +
            $"{RewardExperiencePoints} experience points" + Environment.NewLine +
            $"{RewardAssets} gold pieces" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));
        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete, int rewardExperiencePoints, int rewardAssets, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardAssets = rewardAssets;
            RewardItems = rewardItems;
        }
    }
}
