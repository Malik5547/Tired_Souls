using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        public UIEnemyHealthBar enemyHealthBar;
        public EnemyManager enemyManager;
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        public override void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
        {
            if (isDead)
                return;

            currentHealth = currentHealth - physicalDamage;

            animator.Play(damageAnimation);
            enemyHealthBar.SetHealth(currentHealth);
            Debug.Log("Enemy take damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead");
                isDead = true;
                //Handle player death
            }

            PlayerStatsManager player = FindObjectOfType<PlayerStatsManager>();

            if (player != null) {

                enemyManager.currentTarget = player;
            }
        }
    }
}