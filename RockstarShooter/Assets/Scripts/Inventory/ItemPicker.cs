using System;
using Inventory.Core;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Collider),
        typeof(ItemEquipper))]
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private Core.Inventory _inventory;

        private ItemEquipper _itemEquipper;
        
        private void Awake()
        {
            _itemEquipper = GetComponent<ItemEquipper>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PickableItem pickableItem))
            {
                Debug.Log("ItemPicked");
                _inventory.ItemContainer.TryAddItem(new ItemSlot(pickableItem.Item, 1));
                
               _itemEquipper.TryToEquipItem(pickableItem.Item, pickableItem.ItemObject);
                Destroy(other.gameObject);
            }
        }
    }
}