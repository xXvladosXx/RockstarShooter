using System;
using UnityEngine;

namespace Characters.Player.Data.States
{
    [Serializable]
    public class AliveEntityCombatStateReusableData
    {
        public bool ShouldContinueAttack { get; set; }
        public Vector2 MovementInput { get; set; }
        public float AimLayerWeight { get; set; }
        public bool ShouldDecreaseAimLayer { get; set; }
        public float FireLayerWeight { get; set; }
        public bool ShouldDecreaseFireLayer { get; set; }
        public bool ShouldDecreaseEquipLayer { get; set; }
        public float EquipLayerWeight { get; set; }
    }
}