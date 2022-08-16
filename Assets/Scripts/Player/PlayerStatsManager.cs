using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager playerManager;

        HealthBar healthBar;
        StaminaBar staminaBar;
        PlayerAnimatorManager playerAnimatorManager;

        public float staminaRegenerationAmount = 1;
        public float staminaRenegerationTimer = 0;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            Debug.Log(healthBar);
            staminaBar = FindObjectOfType<StaminaBar>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;

            return maxStamina;
        }

        public override void TakeDamage(int damage, string damageAnimation = "Take_Damage")
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Dead", true);
                isDead = true;

                //Handle player death
                playerManager.HandlePlayerDeath();
            }
        }

        public void RegenerateHP(int amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            healthBar.SetCurrentHealth(currentHealth);

        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRenegerationTimer = 0;
            }
            else
            {
                staminaRenegerationTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRenegerationTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
        }
    }

}