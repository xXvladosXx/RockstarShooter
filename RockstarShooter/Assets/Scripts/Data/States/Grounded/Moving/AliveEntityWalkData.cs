using System;
using UnityEngine;

namespace Characters.Player.Data.States.Grounded.Moving
{
    [Serializable]
    public class AliveEntityWalkData
    {
        [field: SerializeField] [field: Range(0f, 1f)] public float SpeedModifier { get; private set; } = 0.225f;
        [field: SerializeField] public float SmoothInputSpeed { get; private set; } = .1f;

    }
}