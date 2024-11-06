using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    
    // Variable para almacenar la imagen
    public UnityEngine.UI.Image image;
    public Color color;
    

    void Start()
    {
        // Busca el componente de imagen en el GameObject
        image = GetComponent<Image>();
        
        // Obtiene el color actual de la imagen
        Color color = image.color;

        // Cambia solo el valor de alfa (transparencia) del color
        color.a = 0f;

        // Asigna el nuevo color a la imagen
        image.color = color;


        if (image == null)
        {
            Debug.LogError("No se encontró el componente Image en el GameObject.");
        }
    }
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }



    private void Tutorial()
    {
        if (image != null)
        {
            // Obtiene el color actual de la imagen
            Color color = image.color;

            // Cambia solo el valor de alfa (transparencia) del color
            color.a = 1f;
            
            // Asigna el nuevo color a la imagen
            image.color = color;
        }
    }
}
