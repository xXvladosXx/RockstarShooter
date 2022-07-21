using System;
using Inventory.Core;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Collider))]
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private Core.Inventory _inventory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PickableItem pickableItem))
            {
                Debug.Log("ItemPicked");
                if (_inventory.ItemContainer.TryAddItem(new ItemSlot(pickableItem.Item, 1))
                    || GetComponent<ItemEquipper>().TryToEquipItem(pickableItem.Item))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}