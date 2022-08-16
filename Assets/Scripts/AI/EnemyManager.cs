using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStatsManager enemyStatsManager;

        public State currentState;
        public CharacterStatsManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public bool isPerformingAction;
        public bool isInteracing;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;
        public float maximumDetectionAngle = 50;

        public float currentRecoveryTime = 0;

        [Header("A.I. Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navMeshAgent.enabled = false;

        }

        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracing = enemyAnimatorManager.animator.GetBool("isInteracting");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

        }

        private void HandleStateMachine()
        {
            if (!enemyStatsManager.isDead)
            {
                if (currentState != null)
                {
                    State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

                    if (nextState != null)
                    {
                        SwitchToNextState(nextState);
                    }
                }
            }
            
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTime()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }
            else
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

    }
}