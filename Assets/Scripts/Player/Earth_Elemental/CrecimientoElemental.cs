using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrecimientoElemental : AbilityBase
{
    [Header("Crecer Config")]
    public float materialRequerido = 50f;
    [SerializeField] private float materialAcumulado = 0f;

    private Player player;
    [SerializeField] private UI playerUI;
    private bool estaCreciendo = false;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public override void UseAbility()
    {
        if (!isOnCooldown && materialAcumulado >= materialRequerido && !estaCreciendo)
        {
            StartCoroutine(CrecerGolem());

            // Iniciar el cooldown
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator CrecerGolem()
    {
        estaCreciendo = true;

        player.rb.velocity = Vector3.zero;
        player.moveSpeed = 0;

        yield return new WaitForSeconds(1f);

        playerUI.maxHealth += materialAcumulado;
        playerUI.health = playerUI.maxHealth;
        
        Vector3 newScale = player.transform.localScale * (1 + materialAcumulado / playerUI.maxHealth);
        player.transform.localScale = newScale;

        yield return new WaitForSeconds(1f);
        player.moveSpeed = 5f;

        materialAcumulado = 0f;

        estaCreciendo = false;
    }

    public void coleccionarMaterial(float cantidad)
    {
        materialAcumulado += cantidad;
    }

}
