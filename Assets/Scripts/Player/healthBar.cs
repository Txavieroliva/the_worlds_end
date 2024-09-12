using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
   public Slider healthSlider;
   public Slider transHealthSlider;
   public float maxHealth = 100f;
   public float health;
   private float lerpSpeed = 0.005f;

   private void Start()
   {
        health = maxHealth;
        transHealthSlider.value = maxHealth;
   } 

   private void Update()
   {
        updateHealth();
        healthTransition();
   }

   private void updateHealth()
   {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(10);
        }

   }

   private void healthTransition()
   {
        if(healthSlider.value != transHealthSlider.value)
        {
          transHealthSlider.value = Mathf.Lerp(transHealthSlider.value, health, lerpSpeed);
        }
   }

   private void takeDamage(float damage)
   {
        health -= damage;
   }
}
