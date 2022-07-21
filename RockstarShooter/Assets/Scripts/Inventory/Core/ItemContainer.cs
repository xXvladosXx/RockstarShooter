using System;
using UnityEngine;

namespace Inventory.Core
{
    [Serializable]
    public class ItemContainer
    {
        [SerializeField] private ItemSlot[] _itemSlots;
        [SerializeField] private ItemDatabase _database;
        public ItemSlot[] GetItemSlots => _itemSlots;
        public ItemDatabase GetDatabase => _database;

        public event Action OnItemUpdate;

        public ItemContainer(int size)
        {
            _itemSlots = new ItemSlot[size];
        }

        public void Init()
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i] ??= new ItemSlot();
            }

            if (_database == null)
            {
                //_database = Resources.Load<ItemDatabase>("Database");
            }

            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if (_itemSlots[i].Item == null)
                {
                    _itemSlots[i].RemoveItem();
                }
                else
                {
                    if (_itemSlots[i].Quantity == 0)
                    {
                        _itemSlots[i].Quantity = 1;
                    }
                }

                _itemSlots[i].Index = i;
            }
        }
        
        public bool TryAddItem(ItemSlot itemSlot)
        {
            foreach (var slot in _itemSlots)
            {
                if (slot.Item == itemSlot.Item && itemSlot.Item != null)
                {
                    int slotRemainingSpace = slot.Item.MaxInStack - slot.Quantity;
                    if (itemSlot.Quantity <= slotRemainingSpace)
                    {
                        slot.Quantity += itemSlot.Quantity;
                        itemSlot.RemoveItem();
                        OnItemUpdate?.Invoke();
                        return true;
                    }

                    if (slotRemainingSpace > 0)
                    {
                        slot.Quantity += slotRemainingSpace;
                        itemSlot.Quantity -= slotRemainingSpace;
                    }
                }
            }

            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if (_itemSlots[i].Item == null && itemSlot.Item != null)
                {
                    if (itemSlot.Quantity < itemSlot.Item.MaxInStack)
                    {
                        SetSlot(itemSlot);
                        OnItemUpdate?.Invoke();
                        return true;
                    }

                    _itemSlots[i] = new ItemSlot(itemSlot.Item, itemSlot.Item.MaxInStack, itemSlot.ID);
                    itemSlot.Quantity -= itemSlot.Item.MaxInStack;
                }
            }

            OnItemUpdate?.Invoke();
            return false;
        }
        
        private void SetSlot(ItemSlot itemSlot)
        {
            foreach (var slot in _itemSlots)
            {
                if (slot.ID > -1 || !slot.CanBeReplaced(_database.GetItemByID(itemSlot.ID))) continue;

                slot.UpdateSlot(itemSlot);
                break;
            }
        }
        
        public ItemSlot GetItemSlot<T>() where T : ItemSlot
        {
            var def = typeof(T);

            foreach (var itemSlot in _itemSlots)
            {
                if (itemSlot.GetType() == def)
                    return itemSlot;
            }

            return null;
        }

        public ItemSlot GetSlotByIndex(int index) => _itemSlots[index];

        public bool HasItems()
        {
            var hasItems = false;
            foreach (var slot in _itemSlots)
            {
                if (slot.Item != null)
                {
                    hasItems = true;
                }
            }

            return hasItems;
        }

    }
}