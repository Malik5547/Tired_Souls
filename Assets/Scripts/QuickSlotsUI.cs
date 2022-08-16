using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Souls
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public Image[] slotsIcons;
        public GameObject ammoSlot;
        public Image ammoIcon;
        public TMP_Text ammoText;

        public void UpdateWeaponQuickSlotsUI(int index, WeaponItem weapon)
        {
            if(index >= 0 && index < slotsIcons.Length)
            {
                if (weapon.itemIcon != null)
                {
                    slotsIcons[index].sprite = weapon.itemIcon;
                    slotsIcons[index].enabled = true;
                }
                else
                {
                    slotsIcons[index].enabled = false;
                }
            }
        }

        public void EnableAmmoSlot()
        {
            ammoSlot.SetActive(true);
        }

        public void DisableAmmoSlot()
        {
            ammoSlot.SetActive(false);
            ammoIcon.enabled = false;
        }

        public void SetAmmoAmount(int index)
        {
            //EnableAmmoSlot();
            ammoText.text = index.ToString();
        }

        public void SetAmmoType(RangedAmoItem ammoItem)
        {
            ammoIcon.enabled = true;
            ammoIcon.sprite = ammoItem.itemIcon;
        }

        /*public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }*/
    }
}