using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        #region Properties

        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;

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

        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameItem> Inventory { get; }

        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public List<GameItem> Weapons => Inventory.Where(i => i is Weapon).ToList();

        public bool IsDead => CurrentHitPoints <= 0;  // Checking to see if the player or monster is dead

        #endregion

        public event EventHandler OnKilled; //Whenever the player or monster is killed, this class raises this event for any listeners

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void TakeDamage(int hitPointsOfDamage) // Check to see if the player is killed after taking damage
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if(IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal) // Function to make sure the entity isn't healed over their maximum HP
        {
            CurrentHitPoints += hitPointsToHeal;

            if(CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        public void CompletelyHeal() // Called whenever an entity needs to be fully healed
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold) // Check to keep the player from trying to spend gold that they do not have
        {
            if(amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spent {amountOfGold} gold.");
            }

            Gold -= amountOfGold;
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if(item.IsUnique)
            {
                // If the game item is unique, it is added in
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            } else
            {
                if(!GroupedInventory.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    //if the game item isn't unique, it's added in too with a quantity of 0
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                // non-uniques have their quantity increased by one
                GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }

            OnPropertyChanged(nameof(Weapons));
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);

            //Checking to see if the item is unique or not
            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);

            if(groupedInventoryItemToRemove != null)
            {
                // Check to see if the item to remove from player's inventory isn't null
                if(groupedInventoryItemToRemove.Quantity == 1)
                {
                    // If the item in question has a quantity of one, remove it from the screen
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                } else
                {
                    // Otherwise just decrease the quantity by one
                    groupedInventoryItemToRemove.Quantity--;
                }
            }

            OnPropertyChanged(nameof(Weapons));
        }

        private void RaiseOnKilledEvent() // Function to notify other classes that the entity died
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
    }
}
