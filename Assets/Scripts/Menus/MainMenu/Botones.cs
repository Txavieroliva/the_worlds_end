using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    
    // Variable para almacenar la imagen
    public UnityEngine.UI.Image imagen;
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
    }
    public void Jugar()
    {
        Tutorial();
        StartCoroutine(Esperar());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        Time.timeScale = 1;
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



    private void Tutorial()
    {
        if (imagen != null)
        {
            // Obtiene el color actual de la imagen
            Color color = imagen.color;

            // Cambia solo el valor de alfa (transparencia) del color
            color.a = 255f;
            
            // Asigna el nuevo color a la imagen
            imagen.color = color;
        }
    }

    private IEnumerator Esperar()
    {
        // Espera 8 segundos
        yield return new WaitForSeconds(15);
    }
}
