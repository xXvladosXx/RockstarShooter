using Entity;
using UnityEngine;

namespace Combat
{
    public class AttackData
    {
        public IDamageApplier DamageApplier { get; set; }
        public float AccuracyModifier { get; set; }
    }
}