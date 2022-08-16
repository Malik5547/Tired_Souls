using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        QuickSlotsUI quickSlotsUI;

        Animator animator;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;

        [Header("Attacking Weapon")]
        public WeaponItem attackingWeapon;

        protected override void Awake()
        {
            characterManager = GetComponent<PlayerManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            LoadWeaponHolderSlots();
        }

        private void Start()
        {
            CloseDamageCollider();
        }

        private void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

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

        public void LoadWeaponOnSlot(WeaponItem weaponItem)
        {
            if(weaponItem != null)
            {
                if(weaponItem.weaponType == WeaponType.Bow)
                {
                    quickSlotsUI.EnableAmmoSlot();
                    quickSlotsUI.SetAmmoType(playerInventoryManager.currentAmmo);
                    quickSlotsUI.SetAmmoAmount(playerInventoryManager.currentAmmo.currentAmount);
                }
                else
                {
                    quickSlotsUI.DisableAmmoSlot();
                }

                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(playerInventoryManager.currentWeaponIndex, weaponItem);
                animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
            }
        }

        /*public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {

                if (isLeft)
                {
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);

                }
                else
                {
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
            }
            else
            {
                if (isLeft)
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, unarmedWeapon);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, unarmedWeapon);
                }
            }
        }*/

        #region Handle Weapon's Damage Collider 

        protected override void LoadLeftWeaponDamageCollider()
        {
            base.LoadLeftWeaponDamageCollider();
        }

        protected override void LoadRightWeaponDamageCollider()
        {
            if (playerInventoryManager.currentWeapon.weaponType != WeaponType.Bow)
            {
                base.LoadRightWeaponDamageCollider();
            };
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.isUsingLeftHand)
            {
                leftDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if(playerManager.isUsingRightHand)  
                rightDamageCollider.DisableDamageCollider();

            if(playerManager.isUsingLeftHand)
                leftDamageCollider.DisableDamageCollider();
        }


        #endregion

        #region Handle Weapon's Stamina Drainage
        public void DrainStaminaLitghtAtatck()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAtatck()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }

}