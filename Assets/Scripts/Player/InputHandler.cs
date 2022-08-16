using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{

    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;

        public bool lm_Input;
        public bool hold_lm_Input;
        public bool rm_Input;
        public bool ctrl_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_input;
        public bool leftLockOn_Input;
        public bool rightLockOn_Input;

        public bool quick_1;
        public bool quick_2;
        public bool quick_3;
        public bool quick_4;
        public bool drop_Input;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool fireFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        BlockingCollider blockingCollider;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        public UIManager uIManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uIManager = FindObjectOfType<UIManager>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        }

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.LM.performed += i => lm_Input = true;
                inputActions.PlayerActions.HoldLM.performed += i => hold_lm_Input = true;
                inputActions.PlayerActions.HoldLM.canceled += i => hold_lm_Input = false;
                inputActions.PlayerActions.HoldLM.canceled += i => fireFlag = true; 
                inputActions.PlayerActions.RM.performed += i => rm_Input = true;
                inputActions.PlayerActions.RM.canceled += i => rm_Input = false;
                inputActions.PlayerActions.Ctrl.performed += i => ctrl_Input = true;
                inputActions.PlayerActions.Ctrl.canceled += i => ctrl_Input = false;
                inputActions.PlayerQuickSlots.Slot1.performed += i => quick_1 = true;
                inputActions.PlayerQuickSlots.Slot2.performed += i => quick_2 = true;
                inputActions.PlayerQuickSlots.Slot3.performed += i => quick_3 = true;
                inputActions.PlayerQuickSlots.Slot4.performed += i => quick_4 = true;
                inputActions.PlayerActions.Interact.performed += i => a_Input = true;
                inputActions.PlayerActions.Drop.performed += i => drop_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_input = true;
                inputActions.PlayerActions.LockOnLeftTarget.performed += i => leftLockOn_Input = true;
                inputActions.PlayerActions.LockOnRightTarget.performed += i => rightLockOn_Input = true;

            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (playerStatsManager.isDead)
                return;

            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleRMInput();
            HandleCombatInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleFireBowInput();
        }

        public void HandleMoveInput(float delta)
        {
            if (playerManager.isHoldingArrow) {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical) / 2);

                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleRollInput(float delta)
        {

            if (b_Input)
            {
                rollInputTimer += delta;

                if (playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if(moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if(rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleCombatInput(float delta)
        {
            HandleLMInput();
            HandleHoldLMInput();
            HandleRMInput();
            HandleCtrlInput();
        }

        private void HandleLMInput()
        {
            if (lm_Input)
            {
                Debug.Log("LM Input");

                if (playerInventoryManager.currentWeapon.weaponType != WeaponType.Bow)
                {
                    if (playerManager.canDoCombo)
                    {
                        comboFlag = true;
                        playerCombatManager.HandleWeaponCombo(playerInventoryManager.currentWeapon);
                        comboFlag = false;
                    }
                    else
                    {
                        if (playerManager.isInteracting)
                            return;

                        if (playerManager.canDoCombo)
                            return;

                        playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
                        playerCombatManager.HandleLightAttack(playerInventoryManager.currentWeapon);

                    }
                }
            }
        }

        private void HandleRMInput()
        {
           
            if (playerManager.isInAir ||
                playerManager.isSprinting)
            {
                rm_Input = false;
                return;
            }

            if (rm_Input)
            {


                playerCombatManager.HandleRMAction();
            }
            else
            {
                if (playerManager.isAiming)
                {
                    playerManager.isAiming = false;
                    uIManager.crosshair.SetActive(false);
                    //Reset the camera rotation
                    cameraHandler.ResetAimCameraRotations();
                    
                }

                if (blockingCollider.blockingCollider.enabled)
                {
                    playerManager.isBlocking = false;
                    blockingCollider.DisableBlockingCollider();
                }

                
            }
        }

        private void HandleHoldLMInput()
        {
            if (hold_lm_Input)
            {
                if(playerInventoryManager.currentWeapon.weaponType == WeaponType.Bow)
                {
                    playerCombatManager.HandleHoldLMAction();
                }
                else
                {
                    hold_lm_Input = false;
                }
            }
        }

        private void HandleCtrlInput()
        {
            if (ctrl_Input)
            {
                playerCombatManager.HandleHeavyAttack(playerInventoryManager.currentWeapon);
            }
        }

        private void HandleFireBowInput()
        {
            if (fireFlag)
            {
                if (playerManager.isHoldingArrow)
                {
                    fireFlag = false;
                    
                    if (playerManager.bowFireTimer <= 0)
                    {
                        playerCombatManager.FireArrowAction();
                        playerManager.bowFireTimer = playerManager.bowFireCooldown;
                        return;
                    }

                }
            }

            if (playerManager.bowFireTimer > 0)
                playerManager.bowFireTimer -= Time.deltaTime;
        }

        private void HandleQuickSlotInput()
        {
            if (drop_Input)
            {
                playerInventoryManager.DropCurrentWeapon();
            }

            if (quick_1)
            {
                playerInventoryManager.ChangeCurrentWeapon(0);
            } else if (quick_2)
            {
                Debug.Log("Slot 2 input");
                playerInventoryManager.ChangeCurrentWeapon(1);
            }
            else if (quick_3)
            {
                playerInventoryManager.ChangeCurrentWeapon(2);
            }
            else if (quick_4)
            {
                playerInventoryManager.ChangeCurrentWeapon(3);
            }


        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uIManager.OpenSelectWindow();
                    //uIManager.UpdateUI();
                    uIManager.hudWindow.SetActive(false);
                    Cursor.visible = true;
                }
                else
                {
                    uIManager.CloseSelectWindow();
                    uIManager.CloseAllInventoryWindows();
                    uIManager.hudWindow.SetActive(true);
                    Cursor.visible = false;
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_input && !lockOnFlag)
            {
                lockOn_input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    Debug.Log("Lock on target");
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }

            }
            else if (lockOn_input && lockOnFlag)
            {
                lockOn_input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && rightLockOn_Input)
            {
                rightLockOn_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            if(lockOnFlag && leftLockOn_Input)
            {
                leftLockOn_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }
        
    }

}