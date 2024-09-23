using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto_base : MonoBehaviour
{
protected string Identificador;
protected string Nombre;
public string Lugar_De_Pertenencia;
protected string Idle;
protected int Firmeza_Al_Suelo;
protected int Masa_Aguantada;
protected int Velocidad;
public int Vida;
protected int Threshold_Daño = 0;
protected int Vida_Anual;
public bool Agarrable;
protected bool Diferente_Noche;
protected bool Diferente_Climas;
protected bool Crece;
public bool Destructible;
protected float Masa;
protected float Densidad = 1.0f;
protected Rigidbody rb;

protected Vector3 size = Vector3.one;

    
    
    // Start is called before the first frame update
void Start()
    {
     // Obtenemos el Rigidbody asociado
    rb = GetComponent<Rigidbody>();

    // Calculamos la masa
    float Masa = CalcularMasa();

    // Asignamos la masa al Rigidbody
    rb.mass = Masa; 
    }

protected float CalcularVolumen()
{
Vector3 scale = transform.localScale; // Obtenemos la escala del objeto en la escena
return scale.x * scale.y * scale.z;
}

protected float CalcularMasa()
{
float volume = CalcularVolumen();
return volume * Densidad;
}

public void	Settear_Localizacion(string Lugar)
    {
Lugar_De_Pertenencia = Lugar;
    }

// protected string Idles()
//     {

//     }
// public void	Interaccion()
//     {

//     }
// protected string	Destruccion_Completa()
//     {

//     }

protected void	Golpeado(int daño)
    {
if (daño > Threshold_Daño)
{
    Vida = Vida - daño;
   // Destruccion_porcentual();
}
    }

// protected void	Destruccion_Porcentual()
//     {

//     }

protected void	Detector_Daño()
    {

    }

protected void	Calculadora_Daño()
    {

    }

// protected void	Ser_Agarrado()
//     {

//     }
// protected void	Animator()
//     {

//     }
// protected void	Optimizar()
//     {

//     }
// protected void	Informacion_Guardar()
//     {

//     }
// protected void	Informacion_Cargar()
//     {

//     }
// protected void	Algo_Encima()
//     {

//     }
// protected void	Crecer()
//     {

//     }
    
    
   

    // Update is called once per frame

    void Update()
    {
        
    }
}
