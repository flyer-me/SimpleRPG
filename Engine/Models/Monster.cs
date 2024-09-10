using System;
using System.Collections.Generic;
using Engine.Factories;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();
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
            _lootTable.RemoveAll(item => item.ID == id);
            _lootTable.Add(new ItemPercentage(id, percentage));
        }
        public Monster GetNewInstance()
        {
            Monster newMonster =
                    new Monster(ID, Name, ImageName, MaximumHitPoints, Attributes,
                    CurrentWeapon, RewardExperiencePoints, Assets);
            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                // Clone the loot table - even though we probably won't need it
                newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);

                if(RandomGenerate.NumberBetween(1, 100) <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }
            return newMonster;
        }
    }
}
