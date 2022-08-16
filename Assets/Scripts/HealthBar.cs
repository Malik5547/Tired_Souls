using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public class HealthBar : MonoBehaviour
    {
        Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            Debug.Log("Set Max Health");

            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealth(int currentHealth) {
            slider.value = currentHealth;
        }
        
    }

}