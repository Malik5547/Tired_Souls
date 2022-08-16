using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Souls
{
    public class RangedAmmoPickup : Interactable
    {
        public RangedAmoItem ammo;
        public int amount = 5;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager playerAnimatorManager;
            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;  //Stop when picking an item
            playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);

            //playerInventory.DropCurrentWeapon();
            //playerInventory.ReplaceWeaponAtCurrentIndex(weapon);
            playerInventory.SetCurrentAmmo(ammo, amount);

            //playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = ammo.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = ammo.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            //Destroy(gameObject);
        }
    }
}
