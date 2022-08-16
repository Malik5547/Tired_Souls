using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        public RangedAmoItem ammoItem;
        protected bool hasAlreadyPenetratedSurface = false;
        protected GameObject penetratedProjectile;

        protected override void OnTriggerEnter(Collider collision)
        {
            Debug.Log("Arrow trigger: " + collision.gameObject);

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

                if (enemyStats != null)
                {
                    if (shieldHasBeenHit)
                        return;

                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);

                    enemyStats.TakeDamage(physicalDamage, currentDamageAnimation);
                }
            }

            if (!hasAlreadyPenetratedSurface && penetratedProjectile == null)
            {
                Debug.Log("Arrow collision with surface");

                hasAlreadyPenetratedSurface = true;

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));

                Vector3 childScale = penetratedArrow.transform.localScale;
                Vector3 parentScale = collision.transform.localScale;
                
                penetratedProjectile = penetratedArrow;
                penetratedArrow.transform.parent = collision.transform;
                penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);

                penetratedArrow.transform.localScale = new Vector3(childScale.x / parentScale.x, childScale.y / parentScale.y, childScale.z / parentScale.z);

            }

            Destroy(transform.root.gameObject);
        }

    }
}
