using System;
using UnityEngine;

namespace Inventory.Items.Bullet
{
    public class BulletObject : MonoBehaviour
    {
        [field: SerializeField] public BulletItem WeaponItem { get; private set; }
        [field: SerializeField] public ParticleSystem[] HitParticleSystems { get; private set; }

        public void SetRightAngleDependingOnRaycast(RaycastHit? getRaycastHitOfMiddleScreen)
        {
            if(!getRaycastHitOfMiddleScreen.HasValue) return;
            
            foreach (var hit in HitParticleSystems)
            {
                hit.transform.forward = getRaycastHitOfMiddleScreen.Value.normal;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            foreach (var hit in HitParticleSystems)
            {
                hit.gameObject.SetActive(true);
                hit.Emit(1);
                print("Damaged");
            }
            
            Destroy(gameObject,.5f);
        }
    }
}