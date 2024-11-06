using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Win_Lose_Condition : MonoBehaviour
{
    public Player Mi_Player;
    public EventHandler muerteJugador;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public void Start()
    {
        StartCoroutine(DoCheck());

        if(winScreen != null)
        {
            winScreen.SetActive(false);
        }

        if(loseScreen != null)
        {
            loseScreen.SetActive(false);
        }
    }

    private IEnumerator DoCheck()
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
        if (Mi_Player.Vida >= 300 && winScreen != null)
        {
            winScreen.SetActive(true);  // Muestra la pantalla de victoria
            Time.timeScale = 0;  // Pausa el juego
            MostrarCursor();  // Muestra el cursor
        }
    }

    private void Lose_Condition()
    {
        if (Mi_Player.Vida <= 0 && loseScreen != null)
        {
            loseScreen.SetActive(true);  // Muestra la pantalla de derrota
            Time.timeScale = 0;  // Pausa el juego
            MostrarCursor();  // Muestra el cursor
        }
    }

    private void MostrarCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
        Cursor.visible = true;  // Hace visible el cursor
    }
}
