using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Core
{
    [CreateAssetMenu(menuName = "InventorySystem/ItemDatabase")]
    public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField] public Item[] InventoryItems { get; private set; }
        private readonly Dictionary<int, Item> _idItems = new Dictionary<int, Item>();

        private void OnValidate()
        {
            UpdateID();
        }

        public Item GetItemByID(int id)
        {
            _idItems.TryGetValue(id, out var item);

            return item;
        }

        public void OnBeforeSerialize()
        {
            UpdateID();
        }

        public void OnAfterDeserialize()
        {
//            UpdateID();
        }
        
        private void UpdateID()
        {
            _idItems.Clear();
            
            for (int i = 0; i < InventoryItems.Length; i++)
            {
                if (InventoryItems[i].ItemData.ID != i)
                {
                    InventoryItems[i].ItemData.ID = i;
                }
                _idItems.Add(i, InventoryItems[i]);
            }
        }
    }
}