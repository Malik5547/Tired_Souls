using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        public CharacterManager characterManager;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        public DamageCollider leftDamageCollider;
        public DamageCollider rightDamageCollider;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftDamageCollider.characterManager = characterManager;
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightDamageCollider.characterManager = characterManager;
        }
    }
}
