using System;
using RPG.Models.EventArgs;
using RPG.Models.Shared;
using RPG.Core;

namespace RPG.Models
{
    public class Battle : IDisposable
    {
        private enum Combatant
        {
            Player,
            Opponent
        }
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly Monster _opponent;
        public event EventHandler<CombatVictoryEventArgs> OnCombatVictory;
        public Battle(Player player, Monster opponent)
        {
            _player = player;
            _opponent = opponent;
            _player.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnKilled += OnOpponentKilled;
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You see a {_opponent.Name} here!");
            if(FirstAttacker(_player, _opponent) == Combatant.Opponent)
            {
                AttackPlayer();
            }
        }
        private static Combatant FirstAttacker(Player player, Monster opponent)
        {
            // Formula is: ((Dex(player)^2 - Dex(monster)^2)/10) + Random(-10/10)
            // dexterity values from 3 to 18 --> offset of +/- 41.5
            int playerDexterity = player.GetAttribute("DEX").ModifiedValue * player.GetAttribute("DEX").ModifiedValue;
            int opponentDexterity = opponent.GetAttribute("DEX").ModifiedValue * opponent.GetAttribute("DEX").ModifiedValue;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = RandomGenerate.NumberBetween(-10, 10);
            decimal totalOffset = dexterityOffset + randomOffset;
            return RandomGenerate.NumberBetween(0, 100) <= 50 + totalOffset
                       ? Combatant.Player
                       : Combatant.Opponent;
        }
        public void AttackOpponent()
        {
            if(_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon, to attack.");
                return;
            }
            _player.UseCurrentWeaponOn(_opponent);
            if(_opponent.IsAlive)
            {
                AttackPlayer();
            }
        }
        public void Dispose()
        {
            _player.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnKilled -= OnOpponentKilled;
        }
        private void OnOpponentKilled(object? sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You defeated the {_opponent.Name}!");
            _messageBroker.RaiseMessage($"You receive {_opponent.RewardExperiencePoints} experience points.");
            _player.AddExperience(_opponent.RewardExperiencePoints);
            _messageBroker.RaiseMessage($"You receive {_opponent.Assets} gold.");
            _player.ReceiveAssets(_opponent.Assets);
            foreach(GameItem gameItem in _opponent.Inventory.Items)
            {
                _messageBroker.RaiseMessage($"You receive one {gameItem.Name}.");
                _player.AddItemToInventory(gameItem);
            }
            OnCombatVictory?.Invoke(this, new CombatVictoryEventArgs());
        }
        private void AttackPlayer()
        {
            _opponent.UseCurrentWeaponOn(_player);
        }
        private void OnCombatantActionPerformed(object? sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }
    }
}