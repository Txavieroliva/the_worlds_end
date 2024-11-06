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
   [SerializeField] Slider WC_Slider;
   [SerializeField] Slider transWC_Slider;


   public float maxHealth;
   public float health;
   public float maxMana = 100f;
   public float mana;
   private float lerpSpeed = 0.005f;
   private float WinCondition = 300;

   private void Start()
   {
        health = maxHealth;
        transHealthSlider.value = maxHealth;
        mana = maxMana;
        transManaSlider.value = maxMana;
        transWC_Slider.value = WinCondition;
   } 

   private void Update()
   {
        updateHealth();
        healthTransition();
        manaUpdate();
        manaTransition();
        update_wc_slide();
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
   private void update_wc_slide()
     {
          if(WC_Slider.value != health)
          {
               WC_Slider.value = health;
          }
     }
   private void WC_Transition()
   {
        if(WC_Slider.value != transWC_Slider.value)
        {
          transWC_Slider.value = Mathf.Lerp(transWC_Slider.value, health, lerpSpeed);
        }
   }
}
