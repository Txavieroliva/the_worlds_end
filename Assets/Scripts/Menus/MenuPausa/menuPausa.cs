using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausaUI;
    public GameObject pantallaControles;
    private bool juegoPausado = false;

    private void Start()
    {
        if(menuPausaUI != null)        
        {
            menuPausaUI.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void ReanudarJuego()
    {
        menuPausaUI.SetActive(false); // Ocultar el menú de pausa
        Time.timeScale = 1f; // Reanudar el tiempo
        juegoPausado = false;

        Cursor.lockState = CursorLockMode.Locked; // Ocultar el cursor
        Cursor.visible = false;
    }

    public void PausarJuego()
    {
        menuPausaUI.SetActive(true); // Mostrar el menú de pausa
        Time.timeScale = 0f; // Pausar el tiempo
        juegoPausado = true;

        Cursor.lockState = CursorLockMode.None; // Mostrar el cursor
        Cursor.visible = true;
    }

    public void Controles()
    {
        menuPausaUI.SetActive(false);
        pantallaControles.SetActive(true);
    }

    public void volverMenuPausa()
    {
        pantallaControles.SetActive(false);
        menuPausaUI.SetActive(true);
    }
}
