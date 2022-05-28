using System.Collections.Generic;
using AlexMod.HealthSystem;
using UnityEngine;

namespace AlexMod.Inventory.Items {
    public class ItemBasicAssGun : Item {

        [SerializeField] private float _damage;
        
        protected override void UseInternal(GameObject user, Vector3 direction) {
            RaycastHit[] hits = Physics.RaycastAll(user.transform.position, direction);
            foreach (RaycastHit hit in hits) {
                if(hit.transform.gameObject == user) continue;
                if (!hit.transform.gameObject.TryGetComponent(out Health health)) continue;
                
                // damage the first damageable entity 
                Dictionary<string, object> info = new Dictionary<string, object>() {
                    {"DamageType", DamageType.Bullet},
                    {"OriginPosition", user.transform.position},
                    {"ImpactPosition", hit.point},
                    {"ImpactNormal", hit.normal}
                };
                health.Damage(_damage, user, info);
                break;
            }
        }
    }
}