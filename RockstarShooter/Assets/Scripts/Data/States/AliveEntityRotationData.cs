using System;
using UnityEngine;

namespace Characters.Player.Data.States
{
    [Serializable]
    public class AliveEntityRotationData
    {
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
    }
}