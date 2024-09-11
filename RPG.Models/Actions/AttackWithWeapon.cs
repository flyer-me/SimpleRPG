using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG.Models;
using RPG.Models.Shared;
using RPG.Core;

namespace RPG.Models.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;
        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }
            if (minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }
            if (maximumDamage < minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";
            if (AttackSucceeded(actor, target))
            {
                int damage = RandomGenerate.NumberBetween(_minimumDamage, _maximumDamage);
                ReportResult($"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }
            else
            {
                ReportResult($"{actorName} missed {targetName}.");
            }
        }
        private static bool AttackSucceeded(LivingEntity attacker, LivingEntity target)
        {
            // the same as FirstAttacker
            int playerDexterity = attacker.GetAttribute("DEX").ModifiedValue * attacker.GetAttribute("DEX").ModifiedValue;
            int opponentDexterity = target.GetAttribute("DEX").ModifiedValue * target.GetAttribute("DEX").ModifiedValue;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = RandomGenerate.NumberBetween(-10, 10);
            decimal totalOffset = dexterityOffset + randomOffset;
            return RandomGenerate.NumberBetween(0, 100) <= 50 + totalOffset;
        }
    }
}