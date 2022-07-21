using System;
using System.Linq;
using UnityEngine;

namespace Inventory.Core
{
    [Serializable]
    public class ItemSlot
    {
        public Item Item;
        public int ID;
        public int Index;
        [Min(0)] 
        public int Quantity;

        public ItemType[] ItemTypes;
        public bool Changeable = true;
        
        public ItemSlot()
        {
            Item = null;
            ID = -1;
            Quantity = 1;
            ItemTypes = new ItemType[]{};
            Changeable = true;
        }
        public ItemSlot(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
            ID = item.ItemData.ID;
        } 
        public ItemSlot(Item item, int quantity, int index)
        {
            Item = item;
            Quantity = quantity;
            ID = item.ItemData.ID;
            Index = index;
        }
       
        public void RemoveItem()
        {
            Item = null;
            ID = -1;
            Quantity = 0;
        }

        public void UpdateSlot(ItemSlot item)
        {
            Item = item.Item;
            ID = item.ID;
            Quantity = item.Quantity;
        }

        public bool CanBeReplaced(Item item)
        {
            if (item == null)
                return true;
            
            if (ItemTypes.Length <= 0  || item.ItemData.ID < 0)
            {
                return true;
            }

            var anyItem = ItemTypes.Any(t => (t & item.ItemType) == t);

            return anyItem;  
        }

        public bool IsEmpty => Item == null;
        public bool IsFull => Item.MaxInStack == Quantity;
    }
}