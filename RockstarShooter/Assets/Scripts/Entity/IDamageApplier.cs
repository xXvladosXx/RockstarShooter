using Bonuses;
using UnityEngine;

namespace Entity
{
    public interface IDamageApplier
    {
        RaycastHit? GetRaycastHit(Vector3 offset = new());
        BonusesController DamagerBonusesController();
    }
}