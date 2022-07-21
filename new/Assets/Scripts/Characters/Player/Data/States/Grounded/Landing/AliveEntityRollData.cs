using System;
using UnityEngine;

namespace Characters.Player.Data.States.Grounded.Landing
{
    [Serializable]
    public class AliveEntityRollData
    {
        [field: SerializeField] [field: Range(0f, 3f)] public float SpeedModifier { get; private set; } = 1f;
    }
}