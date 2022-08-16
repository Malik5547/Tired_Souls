using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        protected override void Awake()
        {
            characterManager = GetComponent<EnemyManager>();
            WeaponHolderSlot[] weaponHolderSlots = GetComponents<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRighthandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        private void Start()
        {
            LoadWeaponOnBothHands();
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                //leftHandSlot.currentWeapon = weapon;
                //leftHandSlot.LoadWeaponModel(weapon);
                ////load weapons damage collider
                //LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
                //load weapons damage collider
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }

            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                LoadRightWeaponDamageCollider();
            }
        }

        protected override void LoadLeftWeaponDamageCollider()
        {
            base.LoadLeftWeaponDamageCollider();
        }

        protected override void LoadRightWeaponDamageCollider()
        {
            base.LoadRightWeaponDamageCollider();
        }

        public void OpenDamageCollider()
        {
            rightDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightDamageCollider.DisableDamageCollider();
        }

        public void DrainStaminaLitghtAtatck()
        {
        }

        public void DrainStaminaHeavyAtatck()
        {
        }

        public void EnableCombo()
        {
        }

        public void DisableCombo()
        {
        }


    }
}