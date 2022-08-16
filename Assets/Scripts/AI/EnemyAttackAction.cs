using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Actions")]
    public class EnemyAttackAction : EnemyActions
    {
        public bool canCombo;
        public EnemyAttackAction comboAction;

        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAtatckAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 0;
    }
}