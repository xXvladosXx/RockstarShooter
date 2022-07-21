using Characters.Player.Data.Colliders;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.Utilities.Colliders
{
    public class PlayerResizableCapsuleCollider : ResizableCapsuleCollider
    {
        [field: SerializeField] public PlayerTriggerColliderData TriggerColliderData { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            TriggerColliderData.Initialize();
        }
    }
}