using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Souls
{
    public class PlayerManager : CharacterManager
    {
        public GameObject deadWindowUI;

        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerLocomotionManager playerLocomotion;
        PlayerStatsManager playerStatsManager;
        PlayerAnimatorManager playerAnimatorManager;

        InteractableUI interactableUI;

        float interactTimer = 0;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        public float bowFireTimer = 0;
        public float bowFireCooldown = 1f;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        void Start()
        {
            Cursor.visible = false;
            cameraHandler = CameraHandler.singleton;

        }

        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStatsManager.isDead);
            

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerStatsManager.RegenerateStamina();

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.lm_Input = false;
            //inputHandler.rm_Input = false;
            inputHandler.quick_1 = false;
            inputHandler.quick_2 = false;
            inputHandler.quick_3 = false;
            inputHandler.quick_4 = false;
            inputHandler.drop_Input = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }

        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.4f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {

                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            interactTimer = 2f;
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(interactTimer > 0)
                {
                    interactTimer -= Time.deltaTime;
                }
                else
                {
                    itemInteractableGameObject.SetActive(false);
                }

                if(interactableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }

        }

        public void HandlePlayerDeath()
        {
            deadWindowUI.SetActive(true);
            StartCoroutine(RestartGame());
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(7);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void ExitGame()
        {
            Application.Quit();
        }
    }
}