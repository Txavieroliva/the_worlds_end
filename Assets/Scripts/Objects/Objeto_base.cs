using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto_base : Base
{
protected string Identificador;
protected string Nombre;
public string Lugar_De_Pertenencia;
protected string Idle;
protected int Firmeza_Al_Suelo;
protected float Masa_Aguantada = 2.0f;
public float Velocidad;
public int Vida = 2;
protected int NumeroDeDebris;
protected int Threshold_Daño = 0;
public GameObject debrisPrefab;
public float spawnRadius;
protected float explosionForce = 0f;
protected float Masa;
public float Densidad = 100.0f;
public Rigidbody rb;
protected float Multiplicador_De_Daño = 0.5f;  // Factor para ajustar el daño calculado
protected float Velocidad_Min_Daño = 10.0f; // Velocidad mínima para que el daño comience

//Sistema de particulas
[SerializeField] private ParticleSystem Sistema_de_particulas;
 private ParticleSystem Particulas_instancia;




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

        Sistema_de_particulas = GameObject.Find("Golpeado_particulas").GetComponentInChildren<ParticleSystem>();


        CalcularVida();
    }

protected float CalcularVolumen()
{
    Vector3 scale = transform.localScale; // Obtenemos la escala del objeto en la escena
    return scale.x * scale.y * scale.z;
}

protected void CalcularVida()
{
    Vida = Mathf.RoundToInt(rb.mass);
}

protected float CalcularMasa()
{
    float volumen = CalcularVolumen();
    return volumen * Densidad;
}
 
protected void FixedUpdate()
    {
        Velocidad = rb.velocity.magnitude; // Almacena la magnitud de la velocidad
        if (Vida<=0)
            {
            Colapsar();
            }
    }

protected virtual void Colapsar()
    {
        GenerarDebris(); //Generar escombros
        Destroy(gameObject);
    }

protected virtual void GenerarDebris()
    {
        NumeroDeDebris = Mathf.FloorToInt(rb.mass / 50f);

        for(int i = 0; i < NumeroDeDebris; i++)
        {
            //Genera escombros en posiciones aleatorias
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y);

            Vector3 spawnPosition = transform.position + randomOffset;

            //Instancia del escombro
            GameObject debris = Instantiate(debrisPrefab, spawnPosition, Quaternion.identity);

            Rigidbody debrisRb = debris.GetComponent<Rigidbody>();

            if(debrisRb != null)
            {
                Vector3 explosionDirection = (debris.transform.position - transform.position).normalized;
                debrisRb.AddForce(explosionDirection * explosionForce);
            }
        }
    }

protected void OnCollisionEnter(Collision collision)
    {
    

    float Velocidad2 = 0.0f;
        // Obtenemos la velocidad del objeto colisionado
        if (collision.gameObject.GetComponent<Objeto_base>() != null)
            {
            Objeto_base otroObjeto = collision.gameObject.GetComponent<Objeto_base>();

                if (collision.transform.position.y > transform.position.y)
                {
                    Debug.Log("El objeto está encima.");

                    // Verificar si la masa del objeto colisionado es al menos el doble
                    if (otroObjeto.rb.mass>= rb.mass * Masa_Aguantada)
                    {
                    // Aquí colapsa el objeto
                        Colapsar();
                    }
                }
                Velocidad2 = otroObjeto.Velocidad  * otroObjeto.rb.mass;
            }

        // Calcula el daño en base a la velocidad y la masa en el momento del impacto
        float DañoFloat = Calculadora_Daño(Velocidad2);

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

    public override void	Golpeado(int daño)
    {
        if (daño > Threshold_Daño)
        {
            Spawn_Particulas_Daño();
            Vida -= daño;
            Destruccion_Porcentual();
        }
    }

protected virtual void	Destruccion_Porcentual()
    {

    }

protected float	Calculadora_Daño(float velocidad2)
    {
        // Si la velocidad es menor que la mínima, aplicamos cero daño
        if (Velocidad + velocidad2 < Velocidad_Min_Daño)
        {
            return 0f; // O ajusta un daño mínimo si prefieres
        }

        // Calculamos el daño en función de la velocidad, sin exponenciación
        float Energia_kinetica = ((0.5f * (Velocidad * rb.mass)) + (0.5f * velocidad2)); // Daño proporcional a la velocidad de ambos objetos
        float Daño = Energia_kinetica * Multiplicador_De_Daño;
        return Daño;

    }

private void Spawn_Particulas_Daño()
{
    Particulas_instancia = Instantiate(Sistema_de_particulas, transform.position, Quaternion.identity);
}
    
   

    // Update is called once per frame

    void Update()
    {
        
    }
}
