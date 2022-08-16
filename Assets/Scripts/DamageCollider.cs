using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;

        public int physicalDamage = 25;

        protected bool shieldHasBeenHit = false;
        protected string currentDamageAnimation;

        protected virtual void Awake()
        {
            //characterManager = GetComponent<CharacterManager>();
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {

            Debug.Log("Open Damage Collider");
            damageCollider.enabled = true;
        } 

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            Debug.Log("Trigger enter");

            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;

                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                if (enemyManager != null)
                {
                    CheckForBlock(enemyManager, enemyStats, shield);
                }

                if(enemyStats != null)
                {
                    if (shieldHasBeenHit)
                        return;

                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);

                    enemyStats.TakeDamage(physicalDamage, currentDamageAnimation);
                }
            }
        }

        protected void CheckForBlock(CharacterManager enemyManager, CharacterStatsManager enemyStats, BlockingCollider shield)
        {
            if (shield != null && enemyManager.isBlocking)
            {
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block React");
                    return;
                }
            }
        }

        protected void ChooseWichDirectionDamageCameFrom(float direction)
        {
            if(direction >= 145 && direction <= 180)
            {
                currentDamageAnimation = "Take_Damage";
            }else if(direction <= -145 && direction >= -180)
            {
                currentDamageAnimation = "Take_Damage";
            }
            else if(direction >= -45 && direction <= 45)
            {
                currentDamageAnimation = "Take_Damage";
            }
            else if(direction >= 144 && direction <= -45)
            {
                currentDamageAnimation = "Take_Damage";
            }
            else if(direction >= 45 && direction <= 144)
            {
                currentDamageAnimation = "Take_Damage";
            }
            else
            {
                currentDamageAnimation = "Take_Damage";
            }
        }
    }

}