using System;
using Characters.Player.Data.States.Grounded.Landing;
using Characters.Player.Data.States.Grounded.Moving;
using Characters.Player.Data.States.Grounded.Stopping;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.Data.States.Grounded
{
    [Serializable]
    public class PlayerGroundedData
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] [field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] public AliveEntityRotationData BaseRotationData { get; private set; }
        [field: SerializeField] public AliveEntityDashData DashData { get; private set; }
        [field: SerializeField] public AliveEntityWalkData WalkData { get; private set; }
        [field: SerializeField] public AliveEntityRunData RunData { get; private set; }
        [field: SerializeField] public AliveEntitySprintData SprintData { get; private set; }
        [field: SerializeField] public AliveEntityStopData StopData { get; private set; }
        [field: SerializeField] public AliveEntityRollData RollData { get; private set; }
    }
}