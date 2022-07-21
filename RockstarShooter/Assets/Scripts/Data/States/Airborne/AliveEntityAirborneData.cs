using System;
using UnityEngine;

namespace Characters.Player.Data.States.Airborne
{
    [Serializable]
    public class AliveEntityAirborneData
    {
        [field: SerializeField] public AliveEntityJumpData JumpData { get; private set; }
        [field: SerializeField] public AliveEntityFallData FallData { get; private set; }
    }
}