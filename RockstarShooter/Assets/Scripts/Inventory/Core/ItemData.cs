using System;
using UnityEngine;

namespace Inventory.Core
{
    [Serializable]
    public class ItemData
    {
        [field: SerializeField] public int ID { get; set; }

        public ItemData()
        {
            ID = -1;
        }

        public ItemData(int id)
        {
            ID = id;
        }
    }
}