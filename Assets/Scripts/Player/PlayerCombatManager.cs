using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerCombatManager : MonoBehaviour
    {
        public QuickSlotsUI quickSlotsUI;

        InputHandler InputHandler;
        CameraHandler cameraHandler;

        PlayerManager playerManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public string lastAttack;

        GameObject loadedArrow;

        private void Awake()
        {
            InputHandler = GetComponent<InputHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HandleLMAction()
        {
            if(playerInventoryManager.currentWeapon.weaponType == WeaponType.StraightSword)
            {
                HandleLightAttack(playerInventoryManager.currentWeapon);
            }
        }

        public void HandleHoldLMAction()
        {
            if(playerInventoryManager.currentWeapon.weaponType == WeaponType.Bow)
            {
                PerformLMRangedAction();
            }
        }

        public void HandleRMAction()
        {
            if(playerInventoryManager.currentWeapon.weaponType == WeaponType.Bow)
            {
                //Aim the bow
                PerformAimingAction();
            }
            else if(playerInventoryManager.currentWeapon.weaponType == WeaponType.StraightSword)
            {
                PerformBlockAction();
            }
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (InputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {

            if (playerInventoryManager.currentWeapon.weaponType != WeaponType.Unarmed)
            {
                if (playerStatsManager.currentStamina <= 0)
                    return;

                playerWeaponSlotManager.attackingWeapon = weapon;

                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }

        private void DrawArrowAction()
        {

            playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            playerAnimatorManager.PlayTargetAnimation("Bow_Draw", false);
            loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);

            //Animate the bow
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Draw");
     
        }

        public void FireArrowAction()
        {
            //Create live arrow
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            //Animate the bow
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Fire");
            Destroy(loadedArrow);

            //Reset the player holding arrow flag
            playerAnimatorManager.PlayTargetAnimation("Bow_Fire", true);
            playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

            //Create and fire the live arrow
            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (playerManager.isAiming)
            {
                Ray ray = cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 100f))
                {
                    liveArrow.transform.LookAt(hit.point);
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraTransform.localEulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                //Give ammo velocity
                if (cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }

            rigidbody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
            rigidbody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);
            rigidbody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidbody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            //Set live arrow damage
            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physicalDamage;
            damageCollider.EnableDamageCollider();

            playerInventoryManager.currentAmmo.currentAmount -= 1;
            quickSlotsUI.SetAmmoAmount(playerInventoryManager.currentAmmo.currentAmount);
        }

        private void PerformLMRangedAction()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;


            if (!playerManager.isHoldingArrow)
            {
                if(playerInventoryManager.currentAmmo != null && playerInventoryManager.currentAmmo.currentAmount > 0)
                {
                    if (playerManager.bowFireTimer <= 0)
                    {
                        DrawArrowAction();
                    }

                    playerManager.bowFireTimer -= Time.deltaTime;

                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }

        }

        private void PerformBlockAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorManager.PlayTargetAnimation("Block", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void PerformAimingAction()
        {
            if (playerManager.isAiming)
                return;

            InputHandler.uIManager.crosshair.SetActive(true);
            playerManager.isAiming = true;
        }

    }

}