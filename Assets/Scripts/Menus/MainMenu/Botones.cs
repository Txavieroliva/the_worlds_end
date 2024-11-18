using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    
    // Variable para almacenar la imagen
    public UnityEngine.UI.Image imagen;
    public GameObject panelExplicacion; 
    public GameObject panelTutorial;
    public Color color;
    

    void Start()
    {
        // Obtiene el color actual de la imagen
        Color color = imagen.color;

        // Cambia solo el valor de alfa (transparencia) del color
        color.a = 0f;

        // Asigna el nuevo color a la imagen
        imagen.color = color;


        if (imagen == null)
        {
            Debug.LogError("No se encontró el componente Image en el GameObject.");
        }

        if(panelExplicacion != null)
        {
            panelExplicacion.SetActive(false);
        }

        if(panelTutorial != null)
        {
            panelTutorial.SetActive(false);
        }
    }
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        if(panelTutorial != null)
        {
            MostrarCursor();
            Time.timeScale = 0;
            panelTutorial.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    public void Play()
    {
        if(panelExplicacion != null)
        {
            Time.timeScale = 1;
            panelExplicacion.SetActive(false);
            OcultarCursor();
        }
    }

    public void MostrarExplicacion()
    {
        if(panelExplicacion != null && panelTutorial != null)
        {
            panelTutorial.SetActive(false);
            panelExplicacion.SetActive(true);
        }
    }

    public void MostrarTutorial()
    {
        if(panelTutorial != null && panelExplicacion != null)
        {
            panelExplicacion.SetActive(false);
            panelTutorial.SetActive(true);
        }
    }

    private void MostrarCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
        Cursor.visible = true;  // Hace visible el cursor
    }

    private void OcultarCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}
