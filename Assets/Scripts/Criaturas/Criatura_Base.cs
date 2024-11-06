using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Criatura_Base : Base_Con_Vida
{
//Strings
protected string Identificador;
public string Nombre;
public string Lugar_De_Pertenencia;
public string Spawner_Designado;
public string Area_Designada;
protected string Idle;

//Floats
protected float Masa_Aguantada = 2.0f;
protected float Velocidad;
protected float spawnRadius;
protected float explosionForce = 150f;
protected float Masa;
public float Densidad = 1.0f;
protected float Multiplicador_De_Daño = 0.5f;  // Factor para ajustar el daño calculado
protected float Velocidad_Min_Daño = 10.0f; // Velocidad mínima para que el daño comience

//Ints
public int Vida = 2;
public int Nivel;
public int NumeroDeDrop;
protected int Threshold_Daño = 0;
public GameObject DropPrefab;

//booleanos
public bool Agarrable;

protected int Vida_Anual;
protected bool Diferente_Noche;
protected bool Diferente_Climas;
protected bool Crece;

//vectores
protected Vector3 size = Vector3.one;

//Variados
protected Rigidbody rb;

//Start is called before the first frame update
void Start()
    {
     // Obtenemos el Rigidbody asociado
    rb = GetComponent<Rigidbody>();

//Calculamos la masa
    float Masa = CalcularMasa();

//Asignamos la masa al Rigidbody
    rb.mass = Masa; 

    CalcularVida();
    }

protected float CalcularVolumen()
{
    Vector3 scale = transform.localScale; // Obtenemos la escala del objeto en la escena
    return scale.x * scale.y * scale.z;
}

protected void CalcularVida()
{
    Vida = Vida * Mathf.RoundToInt(rb.mass);
}

protected float CalcularMasa()
{
    float volumen = CalcularVolumen();
    return volumen * Densidad;
}
 
protected void FixedUpdate()
    {
        Velocidad = rb.velocity.magnitude; // Almacena la magnitud de la velocidad
        if (Vida<=0){Morir();}
    }

protected void Morir()
    {
        // GenerarDrop(); //Generar escombros
        Destroy(gameObject);
    }

// protected void GenerarDrop()
//     {
//         for(int i = 0; i < NumeroDeDrop; i++)
//         {
//           //   Genera escombros en posiciones aleatorias
//             Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
//             randomOffset.y = Mathf.Abs(randomOffset.y);

//             Vector3 spawnPosition = transform.position + randomOffset;

//           //   Instancia del escombro
//             GameObject Drop = Instantiate(DropPrefab, spawnPosition, Quaternion.identity);

//             Rigidbody DropRb = Drop.GetComponent<Rigidbody>();

//             if(DropRb != null)
//             {
//                 Vector3 explosionDirection = (Drop.transform.position - transform.position).normalized;
//                 DropRb.AddForce(explosionDirection * explosionForce);
//             }
//         }
//     }


// protected void OnTriggerEnter(Collider collision)
protected void OnCollisionEnter(Collision collision)
    {

    float Velocidad2 = 0.0f;
     //    Obtenemos la velocidad del objeto colisionado
            if (collision.gameObject.GetComponent<Objeto_base>() != null)
            {
            Objeto_base otroObjeto = collision.gameObject.GetComponent<Objeto_base>();

                if (collision.transform.position.y > transform.position.y)
                {
                    // Debug.Log("El objeto está encima.");

                    // Verificar si la masa del objeto colisionado es al menos el doble
                    if (otroObjeto.rb.mass >= rb.mass * Masa_Aguantada)
                    {
                    // Aquí Muere la criatura
                        Morir();
                    }
                }
                Velocidad2 = otroObjeto.Velocidad  * otroObjeto.rb.mass;
            }

     //    Calcula el daño en base a la velocidad y la masa en el momento del impacto
        float DañoFloat = Calculadora_Daño(Velocidad2);

     //    Convertimos el daño a un valor entero (redondeando)
        int DañoInt = Mathf.RoundToInt(DañoFloat);
        //   Debug.Log(DañoInt);
        Golpeado(DañoInt);
    }

public void	Settear_Localizacion(string Lugar)
    {
        Lugar_De_Pertenencia = Lugar;
    }

public override void	Golpeado(int daño)
    {
if (daño > Threshold_Daño)
{
    // Debug.Log(daño);
    Vida = Vida - daño;
   Destruccion_Porcentual();
}
    }

protected void	Destruccion_Porcentual()
    {

    }

protected float	Calculadora_Daño(float velocidad2)
    {
        //Si la velocidad es menor que la mínima, aplicamos cero daño
        if (Velocidad + velocidad2 < Velocidad_Min_Daño)
        {
            return 0f; // O ajusta un daño mínimo si prefieres
        }

        //Calculamos el daño en función de la velocidad, sin exponenciación
        float Energia_kinetica = ((0.5f * (Velocidad * rb.mass)) + (0.5f * velocidad2)); // Daño proporcional a la velocidad de ambos objetos
        float Daño = Energia_kinetica * Multiplicador_De_Daño;
        return Daño;

    }

    //Update is called once per frame
    void Update()
    {}
        
}

