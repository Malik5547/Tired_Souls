using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CharacterManager : MonoBehaviour
    {
        public Transform lockOnTransform;

        [Header("Interaction")]
        public bool isInteracting;

        [Header("Combat Flags")]
        public bool isBlocking;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
        public bool isHoldingArrow;
        public bool isAiming;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
    }
}