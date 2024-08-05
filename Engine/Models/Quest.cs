using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ItemQuantity> ItemsToComplete { get; set; }

        public int RewardExperiencePoints { get; set; }
        public int RewardAssets { get; set; }
        public List<ItemQuantity> RewardItems { get; set; }

        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete, int rewardExperiencePoints, int rewardAssets, List<ItemQuantity> rewardItems)
        {
            Id = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardAssets = rewardAssets;
            RewardItems = rewardItems;
        }
    }
}
