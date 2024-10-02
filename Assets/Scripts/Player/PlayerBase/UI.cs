using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
   [SerializeField] Slider healthSlider;
   [SerializeField] Slider transHealthSlider;
   [SerializeField] Slider manaSlider;
   [SerializeField] Slider transManaSlider;
   public float maxHealth;
   public float health;
   public float maxMana = 100f;
   public float mana;
   private float lerpSpeed = 0.005f;

   private void Start()
   {
        health = maxHealth;
        transHealthSlider.value = maxHealth;
        mana = maxMana;
        transManaSlider.value = maxMana;
   } 

   private void Update()
   {
        updateHealth();
        healthTransition();
        manaUpdate();
        manaTransition();
   }

   private void updateHealth()
   {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }
   }

   private void healthTransition()
   {
        if(healthSlider.value != transHealthSlider.value)
        {
          transHealthSlider.value = Mathf.Lerp(transHealthSlider.value, health, lerpSpeed);
        }
   }

   private void manaUpdate()
   {
     if(manaSlider.value != mana)
     {
          manaSlider.value = mana;
     }
   }

   private void manaTransition()
   {
     if(manaSlider.value != transManaSlider.value)
     {
          transManaSlider.value = Mathf.Lerp(transManaSlider.value, mana, lerpSpeed);
     }
   }

   public void useMana(float energy)
   {
     mana -= energy;
     if(mana < 0)
     {
          mana = 0;
     }
   }
}
