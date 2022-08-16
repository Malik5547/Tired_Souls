using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public bool isDead;

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01") { }

    }
}