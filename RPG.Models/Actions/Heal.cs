using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG.Models;

namespace RPG.Models.Actions
{
    public sealed class Heal : BaseAction, IAction
    {
        private readonly int _hitPointsToHeal;
        public event EventHandler<string> OnActionPerformed;
        public Heal(GameItem itemInUse, int hitPointsToHeal) : base(itemInUse)
        {
            if(itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} is not consumable");
            }
            _hitPointsToHeal = hitPointsToHeal;
        }
        void IAction.Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";
            ReportResult($"{actorName} heal {targetName} for {_hitPointsToHeal} point{(_hitPointsToHeal > 1 ? "s" : "")}.");
            target.Heal(_hitPointsToHeal);
        }
    }
}