using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }

        public Monster(string name, string imageName,
                        int maximumHitPoints, int hitPoints,
                        int rewardExperiencePoints, int rewardAssets):
            base(name, maximumHitPoints, hitPoints, rewardAssets)
        {
            ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}
