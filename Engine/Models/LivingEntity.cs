using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Engine.Services;
using System.ComponentModel;

namespace Engine.Models
{
    public abstract class LivingEntity : INotifyPropertyChanged
    {
        #region Properties
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;
        public ObservableCollection<PlayerAttribute> Attributes { get; } = [];
        public string Name { get; }
        public int CurrentHitPoints { get; private set; }
        public int MaximumHitPoints { get; protected set; }
        public int Assets { get; private set; }
        public int Level { get; protected set; }
        public Inventory Inventory { get; private set; }
        public GameItem CurrentWeapon
        {
            get => _currentWeapon;
            set
            {
                if(_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
            }
        }
        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if(_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentConsumable = value;
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
            }
        }
        [JsonIgnore]
        public bool IsAlive => CurrentHitPoints > 0;
        [JsonIgnore]
        public bool IsDead => !IsAlive;
        #endregion
        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints,
                                IEnumerable<PlayerAttribute> attributes, int assets,
                                int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Assets = assets;
            Level = level;
            Inventory = new Inventory();
            foreach (var attribute in attributes)
            {
                Attributes.Add(attribute);
            }
        }
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }
        public void TakeDamage(int damageHitPoints)
        {
            CurrentHitPoints -= damageHitPoints;
            if(IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }
        public void Heal(int healHitPoints)
        {
            CurrentHitPoints += healHitPoints;
            if(CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }
        public void ReceiveAssets(int assetsAmount)
        {
            Assets += assetsAmount;
        }
        public void SpendAssets(int assetsAmount)
        {
            if(assetsAmount > Assets)
            {
                throw new ArgumentOutOfRangeException($"{Name} 只持有 {Assets} 钱币，无法消耗 {assetsAmount} 钱币。");
            }
            Assets -= assetsAmount;
        }
        public void AddItemToInventory(GameItem item)
        {
            Inventory = Inventory.AddItem(item);
        }
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory = Inventory.RemoveItem(item);
        }
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            Inventory = Inventory.RemoveItems(itemQuantities);
        }
        #region private methods
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        private void RaiseActionPerformedEvent(object? sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        #endregion
    }
}