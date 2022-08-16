using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public GameObject pickupPrefab;
        public bool isUnarmed;

        public int damage = 25;

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Absorbtion")]
        public float physicalDamageAbsorbtion;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;

        public string OH_Heavy_Attack_1;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

    }
}
