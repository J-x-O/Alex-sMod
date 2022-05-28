using System.Collections.Generic;
using UnityEngine;

namespace AlexMod.HealthSystem {

    public enum DamageType { Bullet, BluntObject, SharpObject }
    
    /// <summary>Data carrier for information when an entity's <see cref="Health" /> changed.</summary>
    /// <remarks>Written by Max</remarks>
    public readonly struct HealthUpdateInfo {

        public float MaxHp => Target.MaxHp;
        
        /// <summary>Amount of HP after the update was issued.</summary>
        public float NewHp { get; }

        /// <summary>Difference to the amount that was set before the update.</summary>
        public float HpDifference { get; }

        /// <summary>The object that issued the update.</summary>
        public object Causer { get; }
        
        /// <summary> The health component that was damaged.</summary>
        public Health Target { get; }

        public Dictionary<string, object> AdditionalInformation { get; }


        public HealthUpdateInfo(float newHp, float hpDifference, object causer, Health target, Dictionary<string, object> additionalInformation) {
            NewHp = newHp;
            HpDifference = hpDifference;
            Causer = causer;
            Target = target;
            AdditionalInformation = additionalInformation;
        }
    }
}