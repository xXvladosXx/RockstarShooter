using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Core
{
    [CreateAssetMenu (menuName = "InventorySystem/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public ItemContainer ItemContainer { get; private set; } = new ItemContainer(7);

        private void OnEnable()
        {
            ItemContainer.Init();
            
            foreach (var itemSlot in ItemContainer.GetItemSlots)
            {
                if (itemSlot.Item != null)
                {
                    itemSlot.ID = itemSlot.Item.ItemData.ID;
                }
            }
        }
    }
}