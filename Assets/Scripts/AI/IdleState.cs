using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class IdleState : State
    {
        public PursueTergetState pursueTergetState;
        public LayerMask detectionLayer;


        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //Look for potential target
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

                if (characterStats != null)
                {
                    //Check for team ID

                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        return pursueTergetState;
                    }
                }
            }

            #endregion

            #region Handle Switching To Next Statae
            if (enemyManager.currentTarget != null)
            {
                return pursueTergetState;
            }
            else
            {
                return this;
            }

            #endregion

        }

    }
}