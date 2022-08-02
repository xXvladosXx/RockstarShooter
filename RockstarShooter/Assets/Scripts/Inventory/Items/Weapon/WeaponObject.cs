using System;
using System.Collections.Generic;
using System.Linq;
using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.Utilities.Colliders;
using Combat;
using Inventory.Core;
using Inventory.Items.Bullet;
using StateMachine.Player.StateMachines.Combat;
using UnityEngine;
using UnityEngine.Pool;
using UnityTimer;
using Utilities;
using Utilities.Extensions;
using Random = UnityEngine.Random;

namespace Inventory.Items.Weapon
{
    public abstract class WeaponObject : ItemObject
    {
        [field: SerializeField] public ParticleSystem[] Emissions { get; private set; }
        [field: SerializeField] public ParticleSystem SmokeEmission { get; private set; }
        [field: SerializeField] public WeaponItem WeaponItem { get; private set; }
        [field: SerializeField] public float SpreadModifier { get; private set; } = 25;
        [field: SerializeField] public GameObject Magazine { get; private set; } 
        [field: SerializeField] public Transform MagazinePosition { get; private set; }
        [field: SerializeField] public Transform EmissionPosition { get; private set; }
        [field: SerializeField] public float TimeToDestroyMagazine { get; private set; } = 3;
        [field: SerializeField] public float TimeToEmissionRelease { get; private set; } = 1;
        
        public bool WasLoaded { get; protected set; }
        public float AimSpread { get; protected set; }

        protected GameObject CurrentMagazine;
        
        public abstract event Action OnReload;
        public abstract event Action<int> OnMagazineChanged;

        public abstract void Init(PlayerCameraSwitcher playerCameraSwitcher);

        protected virtual void Update()
        {
           
        }

        public abstract bool CanMakeAttack();
        public abstract void MakeAttack(AttackData attackData);


        public virtual void ReloadFinished()
        {
        }

        public virtual void DropMagazine()
        {
        }

        public virtual void SetMagazine(GameObject magazine)
        {
        }

        public virtual void LoadMagazine()
        {
        }

       
    }
}