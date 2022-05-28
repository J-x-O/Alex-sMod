using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Utility;

namespace AlexMod.HealthSystem {
    public class Health : NetworkBehaviour {
        
        /// <summary>Gets called whenever taking damage. The parameter is used to return the new amount of health.</summary>
        public event Action<HealthUpdateInfo> OnHealthChanged;

        /// <summary>Gets called when dying.</summary>
        public event Action<HealthUpdateInfo> OnDeath;

        /// <summary> The current value of the entity's health points. </summary>
        public float CurrentHp { get; private set; }

        /// <summary> The max value of the entity's health points. </summary>
        public float MaxHp => _maxHp;

        // shared variable used to determine max hp
        [SerializeField] protected float _maxHp;
        
        public bool Dead { get; private set; }
        
        /// <summary> Damages the entity for a given amount of hp. The entity might die when falling below zero</summary>
        /// <param name="damageHp"> Amount of health points the entity will be damaged by. </param>
        /// <param name="causer"> The entity that caused the damage. </param>
        /// <param name="originPosition"> the position the damage was issued from </param>
        /// <param name="ticking"> If the Damage is TickingDamage </param>
        [Server]
        public void Damage(float damageHp, object causer, Dictionary<string, object> additionalInformation) {

            if (damageHp < 0) return;

            Debug.Log($"Damage Server: {damageHp}");
            
            float oldHp = CurrentHp;
            CurrentHp -= damageHp;

            HealthUpdateInfo updateInfo = new HealthUpdateInfo(CurrentHp, oldHp - CurrentHp, causer, this, additionalInformation);
            OnHealthChanged?.TryInvoke(updateInfo);
            
            if (CurrentHp <= 0) HandleDeath(oldHp, causer, additionalInformation);
            
            UpdateClient(CurrentHp, causer, additionalInformation);
        }
        
        /// <summary> Heals the entity for a given amount of hp. The health points will not exceed the maximum health points. </summary>
        /// <param name="healHp"> Amount of health points the entity will be healed by. </param>
        /// <param name="healer"> The entity that caused the damage. </param>
        [Server]
        public void Heal(float healHp, object healer, Dictionary<string, object> additionalInformation) {
            
            if (healHp < 0) return;

            float oldHp = CurrentHp;
            CurrentHp += healHp;
            if (CurrentHp > _maxHp) CurrentHp = _maxHp;
            
            HealthUpdateInfo updateInfo = new HealthUpdateInfo(CurrentHp, oldHp - CurrentHp, healer, this, additionalInformation);
            OnHealthChanged?.TryInvoke(updateInfo);
            
            UpdateClient(CurrentHp, healer, additionalInformation);
        }

        [ObserversRpc(IncludeOwner = true)]
        private void UpdateClient(float newHp, object causer, Dictionary<string, object> additionalInformation) {
            float oldHp = CurrentHp;
            CurrentHp = newHp;
            
            HealthUpdateInfo updateInfo = new HealthUpdateInfo(CurrentHp, oldHp - CurrentHp, causer, this, additionalInformation);
            OnHealthChanged?.TryInvoke(updateInfo);

            if (CurrentHp < 0) HandleDeath(oldHp, causer, additionalInformation);
        }

        /// <summary> Method to Invoke OnDeath from outside this class </summary>
        /// <param name="oldHp"> Amount of HP before taking damage </param>
        /// <param name="causer"> object that dealt the damage </param>
        ///  <param name="additionalInformation"> any additional informations </param>
        private void HandleDeath(float oldHp, object causer, Dictionary<string, object> additionalInformation) {
            if (Dead) return;
            
            CurrentHp = 0;
            HealthUpdateInfo damage = new HealthUpdateInfo(0, oldHp - CurrentHp,  causer, this, additionalInformation);
            Dead = true;

            OnDeath?.TryInvoke(damage);
        }
        
    }
}