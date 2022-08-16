using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class HealPlayer : MonoBehaviour
    {
        public int healSpeed = 100;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Character")
            {
                PlayerStatsManager playerStats = other.gameObject.GetComponent<PlayerStatsManager>();

                if (playerStats != null)
                {
                    playerStats.RegenerateHP(Mathf.FloorToInt(healSpeed * Time.deltaTime));
                }

            }
        }

    }
}
