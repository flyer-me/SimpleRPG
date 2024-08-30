using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        #region Properties
        private string _name = string.Empty;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _assets;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Assets
        {
            get { return _assets; }
            private set
            {
                _assets = value;
                OnPropertyChanged();
            }
        }
        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        public ObservableCollection<GameItem> Inventory { get; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventories { get; }
        public List<GameItem> Weapons =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();
        public List<GameItem> Consumables =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();
        public bool HasConsumable => Consumables.Any();
        public bool IsDead => CurrentHitPoints <= 0;
        #endregion
        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int assets, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Assets = assets;
            Level = level;
            Inventory = new ObservableCollection<GameItem>();
            GroupedInventories = new ObservableCollection<GroupedInventoryItem>();
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
            Inventory.Add(item);
            if(item.IsUnique)
            {
                GroupedInventories.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if(!GroupedInventories.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    GroupedInventories.Add(new GroupedInventoryItem(item, 0));
                }
                GroupedInventories.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);
            GroupedInventoryItem? groupedInventoryItemNeedToReduce = item.IsUnique ?
            GroupedInventories.FirstOrDefault(gi => gi.Item == item) :
            GroupedInventories.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);
            if(groupedInventoryItemNeedToReduce != null)
            {
                if(groupedInventoryItemNeedToReduce.Quantity == 1)
                {
                    GroupedInventories.Remove(groupedInventoryItemNeedToReduce);
                }
                else
                {
                    groupedInventoryItemNeedToReduce.Quantity--;
                }
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for(int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }
        }
        public bool HasAllTheseItems(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                if (Inventory.Count(item => item.ItemTypeID == itemQuantity.ItemID) < itemQuantity.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
        #region private methods
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        #endregion
    }
}