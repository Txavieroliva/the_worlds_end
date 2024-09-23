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
protected float Velocidad;
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
protected float Multiplicador_De_Daño = 0.5f;  // Factor para ajustar el daño calculado
protected float Velocidad_Min_Daño = 10.0f; // Velocidad mínima para que el daño comience

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
float volumen = CalcularVolumen();
return volumen * Densidad;
}
 
protected void FixedUpdate()
    {
        Velocidad = rb.velocity.magnitude; // Almacena la magnitud de la velocidad
    }

private void OnCollisionEnter(Collision collision)
    {
        // Calcula el daño en base a la velocidad y la masa en el momento del impacto
        float DañoFloat = Calculadora_Daño();

        // Convertimos el daño a un valor entero (redondeando)
        int DañoInt = Mathf.RoundToInt(DañoFloat);

        Golpeado(DañoInt);
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

protected float	Calculadora_Daño()
    {
        Debug.Log(Velocidad);
        // Si la velocidad es menor que la mínima, aplicamos cero daño
        if (Velocidad < Velocidad_Min_Daño)
        {
            return 0f; // O ajusta un daño mínimo si prefieres
        }

        // Calculamos el daño en función de la velocidad, sin exponenciación
        float Energia_kinetica = 0.5f * rb.mass * Velocidad; // Daño proporcional a la velocidad
        float Daño = Energia_kinetica * Multiplicador_De_Daño;


        return Daño;

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
