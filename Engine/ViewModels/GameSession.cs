// encoding: utf-8
using System.Linq;
using Engine.Models;
using Engine.Factories;
using Engine.Services;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Engine.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        #region Properties
        private Player _currentPlayer;
        private Location _currentLocation;
        private Battle? _currentBattle;
        private Monster? _currentMonster;
        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonIgnore]
        public GameDetails GameDetails { get; private set; }
        [JsonIgnore]
        public World CurrentWorld { get; }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLevelUp;
                    _currentPlayer.OnKilled -= OnPlayerKilled;
                }
                _currentPlayer = value;
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLevelUp;
                    _currentPlayer.OnKilled += OnPlayerKilled;
                }
            }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                CompleteQuestsAtLocation();
                GivePlayerQuestAtLocation();
                CurrentMonster = CurrentLocation.GetMonster();
                CurrentTrader = CurrentLocation.TraderHere;
            }
        }
        [JsonIgnore]
        public Monster? CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                    _currentBattle = null;
                }
                _currentMonster = value;
                if (_currentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster!);
                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
                }
            }
        }
        [JsonIgnore]
        public Trader? CurrentTrader { get; private set; }
        [JsonIgnore]
        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        [JsonIgnore]
        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        [JsonIgnore]
        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasMonster => CurrentMonster != null;
        [JsonIgnore]
        public bool HasTrader => CurrentTrader != null;
        #endregion
        public GameSession(Player player, int xCoordinate, int yCoordinate)
        {
            GameDetails = GameDetailsService.ReadGameDetails();
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = player;
            CurrentLocation = CurrentWorld.LocationAt(xCoordinate, yCoordinate);
        }
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }
        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus? questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID &&
                                                             !q.IsCompleted);
                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);
                        _messageBroker.RaiseMessage("");
                        _messageBroker.RaiseMessage($"You completed the '{quest.Name}' quest");
                        _messageBroker.RaiseMessage($"+{quest.RewardExperiencePoints}XP");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        _messageBroker.RaiseMessage($"+{quest.RewardAssets}Gold");
                        CurrentPlayer.ReceiveAssets(quest.RewardAssets);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            _messageBroker.RaiseMessage($"Receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }
        private void GivePlayerQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"Quest:'{quest.Name}'");
                    _messageBroker.RaiseMessage(quest.Description);
                    _messageBroker.RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    _messageBroker.RaiseMessage("And you will receive:");
                    _messageBroker.RaiseMessage($"   {quest.RewardExperiencePoints}XP");
                    _messageBroker.RaiseMessage($"   {quest.RewardAssets}Gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }
        public void AttackCurrentMonster()
        {
            _currentBattle?.AttackOpponent();
        }
        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed += OnConsumableActionPerformed;
                }
                CurrentPlayer.UseCurrentConsumable();
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed -= OnConsumableActionPerformed;
                }
            }
        }
        private void OnConsumableActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }
        public void CraftItemUsing(Recipe recipe)
        {
            List<ItemQuantity> RequiredItems = recipe.Ingredients;
            if (CurrentPlayer.Inventory.HasAllTheseItems(RequiredItems))
            {
                CurrentPlayer.RemoveItemsFromInventory(RequiredItems);
                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                _messageBroker.RaiseMessage("You do not have the required ingredients:");
                foreach (ItemQuantity requiredItems in RequiredItems)
                {
                    _messageBroker.RaiseMessage($"  {requiredItems.QuantityItemDescription}");
                }
            }
        }
        private void OnPlayerKilled(object? sender, System.EventArgs eventArgs)
        {
            // 确保此次移动后清除battle
            if (_currentBattle != null)
            {
                _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                _currentBattle.Dispose();
                _currentBattle = null;
            }
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You have been killed");
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }
        private void OnCurrentMonsterKilled(object? sender, System.EventArgs eventArgs)
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }
        private void OnCurrentPlayerLevelUp(object? sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"Level up! You are now level {CurrentPlayer.Level}");
        }
    }
}