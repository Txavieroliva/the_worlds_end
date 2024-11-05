using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Lose_Condition : MonoBehaviour
{
   public Player Mi_Player;

   void Start()
   {
    StartCoroutine(DoCheck());
   }
   IEnumerator DoCheck()
    {
        for(;;)
        {
            Win_Condition();
            Lose_Condition();
            yield return new WaitForSeconds(.3f);
        }
    }
   private void Win_Condition()
   {
    if (Mi_Player.Vida >= 500)
    {
        //Mostrar_Pantalla_Win; 
        Time.timeScale = 0;

    }
   }
   private void Lose_Condition()
   {
    if (Mi_Player.Vida <= 0)
    {
        // Mostrar_Pantalla_Lose;
        Time.timeScale = 0;
    }
   }
}
