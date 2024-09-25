using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpear : AbilityBase
{
    public GameObject spearPrefab;
    public Transform spearSpawnPoint;
    public float spearSpeed = 50f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator> ();
    }

    public override void UseAbility()
    {
        if(!isOnCooldown)
        {
            animator.SetTrigger("throwSpear");

            GameObject spear = Instantiate(spearPrefab, spearSpawnPoint.position, spearSpawnPoint.rotation);
            Rigidbody rb = spear.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.velocity = spearSpawnPoint.forward * spearSpeed;
            }

            StartCoroutine(StartCooldown());
        }
    }
}
