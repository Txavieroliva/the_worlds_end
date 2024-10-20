using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Casa : Objeto_base
{
    public GameObject debrisPrefab1;

    public int Debris_Madera;
    public int Debris_Piedra;
    
    protected override void	Destruccion_Porcentual()
    {
        if (Vida < Mathf.RoundToInt(rb.mass) / 2){
        rb.isKinematic = false;
        }
    }
protected override void GenerarDebris()
    {
        NumeroDeDebris = Mathf.FloorToInt(rb.mass / 50f);

        for(int i = 0; i < Debris_Madera; i++)
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
        for(int i = 0; i < Debris_Piedra; i++)
        {
            //Genera escombros en posiciones aleatorias
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y);

            Vector3 spawnPosition = transform.position + randomOffset;

            //Instancia del escombro
            GameObject debris = Instantiate(debrisPrefab1, spawnPosition, Quaternion.identity);

            Rigidbody debrisRb = debris.GetComponent<Rigidbody>();

            if(debrisRb != null)
            {
                Vector3 explosionDirection = (debris.transform.position - transform.position).normalized;
                debrisRb.AddForce(explosionDirection * explosionForce);
            }
        }
    }
    
    public override void Golpeado(int daño)
    {
        if (daño > Threshold_Daño)
        {
            Vida -= daño;
            Destruccion_Porcentual();
        }
    }

}
