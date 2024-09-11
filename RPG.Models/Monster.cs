using System;
using System.Collections.Generic;

namespace RPG.Models
{
    public class Monster : LivingEntity
    {
        public List<ItemPercentage> LootTable {get; } = [];
        public int ID { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }
        public Monster(int id, string name, string imageName,
                        int maximumHitPoints, IEnumerable<PlayerAttribute> attributes,
                        GameItem currentWeapon,
                        int rewardExperiencePoints, int rewardAssets):
            base(name, maximumHitPoints, maximumHitPoints, attributes, rewardAssets)
        {
            ID = id;
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }
        public void AddItemToLootTable(int id, int percentage)
        {
            LootTable.RemoveAll(item => item.ID == id);
            LootTable.Add(new ItemPercentage(id, percentage));
        }
        public Monster Clone()
        {
            Monster monster = new(ID, Name, ImageName, MaximumHitPoints,
                Attributes, CurrentWeapon, RewardExperiencePoints, Assets);
            monster.LootTable.AddRange(LootTable);
            return monster;
        }
    }
}
