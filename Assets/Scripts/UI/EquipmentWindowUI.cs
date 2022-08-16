using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        public HandEquipementSlotUI[] handEquipementSlotUI;


        public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for(int i = 0; i < handEquipementSlotUI.Length; i++)
            {
                if (handEquipementSlotUI[i].rightHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                } else if (handEquipementSlotUI[i].rightHandSlot02)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                } else if (handEquipementSlotUI[i].leftHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }
    }
}