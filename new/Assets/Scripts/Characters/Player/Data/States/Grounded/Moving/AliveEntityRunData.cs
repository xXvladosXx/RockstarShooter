using System;
using UnityEngine;

namespace Characters.Player.Data.States.Grounded.Moving
{
    [Serializable]
    public class AliveEntityRunData
    {
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; private set; } = 1f;
        [field: SerializeField] public float SmoothInputSpeed { get; private set; }

    }
}