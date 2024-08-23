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
        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _assets;
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            private set
            {
                _maximumHitPoints = value;
                OnPropertyChanged(nameof(MaximumHitPoints));
            }
        }
        public int Assets
        {
            get { return _assets; }
            private set
            {
                _assets = value;
                OnPropertyChanged(nameof(Assets));
            }
        }
        public ObservableCollection<GameItem> Inventory { get; set; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventories { get; set; }
        public List<GameItem> Weapons =>
            Inventory.Where(i => i is Weapon).ToList();
        public bool IsDead => CurrentHitPoints <= 0;
        #endregion
        public event EventHandler OnKilled;
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int assets)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Assets = assets;
            Inventory = new ObservableCollection<GameItem>();
            GroupedInventories = new ObservableCollection<GroupedInventoryItem>();
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
        }
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);
            GroupedInventoryItem? groupedInventoryItemNeedToReduce =
                GroupedInventories.FirstOrDefault(gi => gi.Item == item);
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
        }
        #region private methods
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        #endregion
    }
}