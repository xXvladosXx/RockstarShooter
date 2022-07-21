using System;
using Combat;
using Inventory.Items.Bullet;
using UnityEngine;
using UnityTimer;
using Random = UnityEngine.Random;

namespace Inventory.Items.Weapon
{
    public class WeaponObject : MonoBehaviour
    {
        [field: SerializeField] public Transform PointToSpawnEmission { get; private set; }
        [field: SerializeField] public ParticleSystem[] Emissions { get; private set; }
        [field: SerializeField] public WeaponItem WeaponItem { get; private set; }

        private Timer _timer;
        private float _currentSpread;

        private void Start()
        {
            _currentSpread = WeaponItem.MinSpread;
        }

        public void SpawnEmission(AttackData attackData)
        {
            Timer.Cancel(_timer);

            float attackDataAccuracyModifier = 1;
            if (attackData.AccuracyModifier / 100 != 0)
            {
                attackDataAccuracyModifier = attackData.AccuracyModifier / 100;
            }
            
            attackDataAccuracyModifier = 1 / attackDataAccuracyModifier;
            
            _currentSpread = Mathf.Clamp( _currentSpread+WeaponItem.SpreadPerFire * attackDataAccuracyModifier, 
                WeaponItem.MinSpread * attackDataAccuracyModifier, WeaponItem.MaxSpread * attackDataAccuracyModifier);
            
            foreach (var emission in Emissions)
            {
                emission.transform.position = PointToSpawnEmission.position;
                emission.transform.parent = null;
                emission.Play();
                emission.Emit(1);
            }

            var directionWithSpread = SpawnProjectile(attackData, out var currentBullet);

            AddForce(currentBullet, directionWithSpread);

            ReduceSpread();
        }

        private void AddForce(BulletObject currentBullet, Vector3 directionWithSpread)
        {
            currentBullet.transform.forward = directionWithSpread.normalized;
            currentBullet.GetComponent<Rigidbody>()
                .AddForce(directionWithSpread.normalized * WeaponItem.ShootForce, ForceMode.Impulse);
        }

        private Vector3 SpawnProjectile(AttackData attackData, out BulletObject currentBullet)
        {
            Vector3 aimDirection = Vector3.zero;
            if (attackData.RaycastHit.HasValue)
            {
                aimDirection = (attackData.RaycastHit.Value.point - PointToSpawnEmission.position).normalized;
            }

            float spreadX = Random.Range(-_currentSpread, _currentSpread);
            float spreadY = Random.Range(-_currentSpread, _currentSpread);

            Vector3 directionWithSpread = aimDirection + new Vector3(spreadX, spreadY, 0);
            currentBullet = Instantiate(WeaponItem.BulletObject, PointToSpawnEmission.position,
                Quaternion.LookRotation(aimDirection, Vector3.up));
            
            currentBullet.SetRightAngleDependingOnRaycast(attackData.RaycastHit);
            return directionWithSpread;
        }

        private void ReduceSpread()
        {
            _timer = Timer.Register(WeaponItem.TimeToReduceSpread, () => { _currentSpread = WeaponItem.MinSpread; }, (time) =>
            {
                var someVal = Mathf.Clamp(_currentSpread - time * WeaponItem.SpreadReduceModifier, WeaponItem.MinSpread,
                    WeaponItem.MaxSpread);
            });
        }
    }
}