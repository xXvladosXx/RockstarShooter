using UnityEngine;

namespace Inventory.Core
{
    [RequireComponent(typeof(Collider))]
    public sealed class PickableItem : MonoBehaviour
    {
        [field: SerializeField] public Item Item { get; private set; }
    }
}