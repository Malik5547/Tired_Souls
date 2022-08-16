using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;

        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;

            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }
    }
}