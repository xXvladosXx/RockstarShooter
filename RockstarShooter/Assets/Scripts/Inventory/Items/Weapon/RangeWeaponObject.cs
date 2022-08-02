using System;
using System.Linq;
using Characters.Player.Utilities.Colliders;
using Combat;
using UnityEngine;
using UnityEngine.Pool;
using Utilities.Extensions;

namespace Inventory.Items.Weapon
{
    public class RangeWeaponObject : WeaponObject
    {
        private ObjectPool<ParticleSystem> _emissionPool;
        private ObjectPool<ParticleSystem> _hitPool;
        private WeaponRecoil _weaponRecoil;
        private PlayerCameraSwitcher _playerCameraSwitcher;

        public int BulletsLeft { get; private set; }
        public bool ReadyToAttack { get; private set; }
        public float CurrentSpread { get; private set; }
        private float _attackDataAccuracyModifier;
        public bool Reloading { get; private set; }

        public override event Action<int> OnMagazineChanged;

        public override event Action OnReload;

        public override void Init(PlayerCameraSwitcher playerCameraSwitcher)
        {
            _weaponRecoil = GetComponent<WeaponRecoil>();

            BulletsLeft = WeaponItem.MagazineSize;
            ReadyToAttack = true;
            
            _playerCameraSwitcher = playerCameraSwitcher;
            _weaponRecoil.Init(_playerCameraSwitcher);

            CurrentMagazine = Instantiate(Magazine, transform);
        }
        private void Start()
        {
            CurrentSpread = WeaponItem.MinSpread;
            
            _emissionPool = new ObjectPool<ParticleSystem>(() => 
                Instantiate(SmokeEmission, gameObject.transform), prefab =>
            {
                prefab.gameObject.SetActive(true);
            }, prefab =>
            {
                prefab.gameObject.SetActive(false);
            }, prefab =>
            {
                Destroy(prefab.gameObject);
            }, false, 50,50);
            
            _hitPool = new ObjectPool<ParticleSystem>(() => 
                Instantiate(WeaponItem.HitParticles.First(), gameObject.transform), prefab =>
            {
                prefab.gameObject.SetActive(true);
            }, prefab =>
            {
                prefab.gameObject.SetActive(false);
            }, prefab =>
            {
                Destroy(prefab.gameObject);
            }, false, 30,50);
        }

        public override void MakeAttack(AttackData attackData)
        {
            ReadyToAttack = false;
            
            var distanceHit = CalculateDistanceHit(attackData);

            PlayEmissions();

            if (distanceHit.HasValue)
            {
                var hit = _hitPool.Get();
                hit.transform.position = distanceHit.Value.point;
                hit.transform.forward = distanceHit.Value.normal;
                
                this.CallWithDelay(ReleaseHit(hit), TimeToEmissionRelease);
            }

            _weaponRecoil.GenerateRecoil(attackData);

            BulletsLeft--;
            OnMagazineChanged?.Invoke(BulletsLeft);
            
            this.CallWithDelay(ResetShot, WeaponItem.TimeBetweenShooting);
        }
        
        public override bool CanMakeAttack()
        {
            if (BulletsLeft <= 0)
            {
                OnReload?.Invoke();
                return false;
            }
            
            if(Reloading) return false;
            if(!ReadyToAttack) return false;

            if (BulletsLeft > 0)
                return true;

            return true;
        }
        
        protected override void Update()
        {
            base.Update();
            
            Mathf.Clamp(AimSpread + WeaponItem.SpreadPerFire * _attackDataAccuracyModifier,
                WeaponItem.MinSpread * _attackDataAccuracyModifier, WeaponItem.MaxSpread * _attackDataAccuracyModifier);
            
            ReduceSpread();
        }

        private RaycastHit? CalculateDistanceHit(AttackData attackData)
        {
            _attackDataAccuracyModifier = 1;
            if (attackData.AccuracyModifier / 100 != 0)
            {
                _attackDataAccuracyModifier = attackData.AccuracyModifier / 100;
            }

            _attackDataAccuracyModifier = 1 / _attackDataAccuracyModifier;

            CurrentSpread = Mathf.Clamp(CurrentSpread + WeaponItem.SpreadPerFire * _attackDataAccuracyModifier,
                WeaponItem.MinSpread * _attackDataAccuracyModifier, WeaponItem.MaxSpread * _attackDataAccuracyModifier);

            AimSpread = CurrentSpread;

            float spreadX = UnityEngine.Random.Range(-CurrentSpread / SpreadModifier, CurrentSpread / SpreadModifier);
            float spreadY = UnityEngine.Random.Range(-CurrentSpread / SpreadModifier, CurrentSpread / SpreadModifier);
            var distanceHit = attackData.DamageApplier.GetRaycastHit(new Vector3(spreadX, spreadY, 0));
            return distanceHit;
        }

        private void PlayEmissions()
        {
            var smokeEmission = _emissionPool.Get();

            smokeEmission.transform.parent = EmissionPosition.transform;
            
            smokeEmission.transform.localPosition = Vector3.zero;
            smokeEmission.transform.localScale = new Vector3(.6f,.6f,.6f);

            smokeEmission.transform.parent = null;

            smokeEmission.Play();
            smokeEmission.Emit(1);
            
            this.CallWithDelay(ReleaseEmission(smokeEmission), TimeToEmissionRelease);
            
            foreach (var emission in Emissions)
            {
                emission.Emit(1);
                emission.Play();
            }
        }

        private Action ReleaseEmission(ParticleSystem smokeEmission) => () => _emissionPool.Release(smokeEmission);
        private Action ReleaseHit(ParticleSystem hit) => () => _hitPool.Release(hit);

        private void ResetShot()
        {
            ReadyToAttack = true;
        }

        private void ReduceSpread()
        {
            if (CurrentSpread == 0) return;

            CurrentSpread = Mathf.Clamp(CurrentSpread - Time.deltaTime * WeaponItem.SpreadReduceModifier,
                WeaponItem.MinSpread,
                WeaponItem.MaxSpread);
        }

        private void Reload()
        {
            ReadyToAttack = false;
            BulletsLeft = 0;
            OnMagazineChanged?.Invoke(BulletsLeft);
            DropMagazine();
            
            Reloading = true;
        }
        
        public override void ReloadFinished()
        {
            BulletsLeft = WeaponItem.MagazineSize;
            OnMagazineChanged?.Invoke(BulletsLeft);

            ReadyToAttack = true;
            Reloading = false;
        }

        public override void DropMagazine()
        {
            if(CurrentMagazine == null) return;
            
            CurrentMagazine.transform.parent = null;
            CurrentMagazine.GetComponent<Rigidbody>().useGravity = true;
            CurrentMagazine.GetComponent<Rigidbody>().isKinematic = false;
            
            Destroy(CurrentMagazine, TimeToDestroyMagazine);
        }

        public override void SetMagazine(GameObject magazine)
        {
            Reload();

            CurrentMagazine = magazine;

            WasLoaded = false;
        }

        public override void LoadMagazine()
        {
            WasLoaded = true;

            CurrentMagazine.transform.parent = MagazinePosition.transform;
            
            CurrentMagazine.transform.localPosition = new Vector3(0,.2f,0);
            CurrentMagazine.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}