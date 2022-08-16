using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public QuickSlotsUI quickSlotsUI;

        PlayerWeaponSlotManager playerWeaponSlotManager;

        //public WeaponItem rightWeapon;
        //public WeaponItem leftWeapon;
        public WeaponItem currentWeapon;
        public RangedAmoItem currentAmmo;

        public int currentWeaponIndex = 0;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        public List<WeaponItem> weaponsInventory = new List<WeaponItem>();

        //public WeaponItem[] weaponsInventory = new WeaponItem[4];

        private void Awake()
        {
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

            weaponsInventory.Add(playerWeaponSlotManager.unarmedWeapon);
            weaponsInventory.Add(playerWeaponSlotManager.unarmedWeapon);
            weaponsInventory.Add(playerWeaponSlotManager.unarmedWeapon);
            weaponsInventory.Add(playerWeaponSlotManager.unarmedWeapon);
        }

        private void Start()
        {
            ChangeCurrentWeapon(0);
            //ightWeapon = weaponsInRightHandSlots[0];
            //leftWeapon = weaponsInLeftHandSlots[0];
            //playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeCurrentWeapon(int index)
        {
            if(index >= 0 && index < weaponsInventory.Count)
            {
                currentWeaponIndex = index;

                if(weaponsInventory[index] != null)
                {
                    currentWeapon = weaponsInventory[index];
                    playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInventory[index]);
                }
                else
                {
                    currentWeapon = playerWeaponSlotManager.unarmedWeapon;
                    playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon);
                }

            }
        }

        public void ReplaceWeaponAtCurrentIndex(WeaponItem weapon)
        {
            Debug.Log("Current weapon index " + currentWeaponIndex);
            Debug.Log("Inventory size " + weaponsInventory.Count);

            weaponsInventory[currentWeaponIndex] = weapon;
            ChangeCurrentWeapon(currentWeaponIndex);
        }

        public void DropCurrentWeapon()
        {
            if (weaponsInventory[currentWeaponIndex] != null && weaponsInventory[currentWeaponIndex].weaponType != WeaponType.Unarmed)
            {
                GameObject pickUpObject = Instantiate(weaponsInventory[currentWeaponIndex].pickupPrefab);
                pickUpObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                weaponsInventory[currentWeaponIndex] = playerWeaponSlotManager.unarmedWeapon;
                ChangeCurrentWeapon(currentWeaponIndex);
            }
        }

        public void SetCurrentAmmo(RangedAmoItem ammoItem, int amount)
        {
            if(currentAmmo == ammoItem)
            {
                currentAmmo.currentAmount += amount;
                if (currentAmmo.currentAmount > currentAmmo.carryLimit)
                    currentAmmo.currentAmount = currentAmmo.carryLimit;

            }
            else
            {
                currentAmmo = ammoItem;
                currentAmmo.currentAmount = amount;
                quickSlotsUI.SetAmmoType(ammoItem);
            }
            
            quickSlotsUI.SetAmmoAmount(currentAmmo.currentAmount);
        }

            /* public void ChangeRightWeapon()
             {
                 currentRightWeaponIndex = currentRightWeaponIndex + 1;

                 if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
                 {
                     rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                     playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
                 }
                 else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
                 {
                     currentRightWeaponIndex = currentRightWeaponIndex + 1;
                 }
                 else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
                 {
                     rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                     playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
                 }
                 else
                 {
                     currentRightWeaponIndex = currentRightWeaponIndex + 1;
                 }

                 if (currentRightWeaponIndex > weaponsInRightHandSlots.Length)
                 {
                     currentRightWeaponIndex = -1;
                     rightWeapon = playerWeaponSlotManager.unarmedWeapon;
                     playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, false);
                 }
             }*/

        }
    }