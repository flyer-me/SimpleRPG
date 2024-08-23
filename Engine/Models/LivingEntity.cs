using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _assets;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            set
            {
                _maximumHitPoints = value;
                OnPropertyChanged(nameof(MaximumHitPoints));
            }
        }
        public int Assets
        {
            get { return _assets; }
            set
            {
                _assets = value;
                OnPropertyChanged(nameof(Assets));
            }
        }
        public ObservableCollection<GameItem> Inventory { get; set; }
        public List<GameItem> Weapons =>
            Inventory.Where(i => i is Weapon).ToList();
        public ObservableCollection<GroupedInventoryItem> GroupedInventories { get; set; }
        protected LivingEntity()
        {
            Inventory = new ObservableCollection<GameItem>();
            GroupedInventories = new ObservableCollection<GroupedInventoryItem>();
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
    }
}