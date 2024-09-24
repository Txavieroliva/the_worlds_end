using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    public float maxhealth = 200f;
    private float currentHealth;

    public GameObject debrisPrefab;
    public int numberOfDebris;
    private float spawnRadius = 5f;
    private float explosionForce = 150f;

    private void Start()
    {
        currentHealth = maxhealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Recibio daño");

        if(currentHealth <= 0)
        {
            CollapseStructure();
        }
    }

    private void CollapseStructure()
    {
        Debug.Log("Cayó como las TT");

        GenerateDebris(); //Generar escombros

        Destroy(gameObject); //Destruye la estructura
    }

    private void GenerateDebris()
    {
        for(int i = 0; i < numberOfDebris; i++)
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
}
