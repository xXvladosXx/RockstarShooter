using UnityEngine;

namespace Combat
{
    public class AttackData
    {
        public float AccuracyModifier { get; set; }
        public RaycastHit? RaycastHit { get; set; }
    }
}