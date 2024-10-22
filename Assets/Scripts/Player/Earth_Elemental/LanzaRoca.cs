using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaRoca : AbilityBase
{
    public GameObject rocaPrefab;
    public Transform spawnPointRoca;
    public float velocidadRoca = 50f;
    private Animator animator;
    private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private Collider golemCollider;
    [SerializeField] private Collider meleeCollider;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        mainCamera = Camera.main;
    }

    public override void UseAbility()
    {
        if(!isOnCooldown)
        {
            DetenerPlayer();

            animator.SetTrigger("Ability1");

            StartCoroutine(GenerarLanzarRoca());

            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator GenerarLanzarRoca()
    {
        yield return new WaitForSeconds(1f);

        Vector3 direction = ObtenerDireccionDelMouse();
        
        GameObject roca = Instantiate(rocaPrefab, spawnPointRoca.position, spawnPointRoca.rotation);
        Collider rocaCollider = roca.GetComponentInChildren<Collider>();

        Roca rocaScript = roca.GetComponent<Roca>();

        if(rocaScript != null)
        {
            Physics.IgnoreCollision(rocaCollider, golemCollider, true);
            Physics.IgnoreCollision(rocaCollider, meleeCollider, true);

            StartCoroutine(ReactivarColision(rocaCollider));
            rocaScript.Lanzar(direction, player);
            player.TakeDamage(20);
        }

        ReanudarMovimientoPlayer();
    }

    private Vector3 ObtenerDireccionDelMouse()
    {
        // Crear un rayo desde la cámara hacia la posición del mouse
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Variable para almacenar el punto de impacto del raycast
        RaycastHit hit;

        // Intentar hacer un Raycast en el plano del mundo
        if (Physics.Raycast(ray, out hit))
        {
            // Calcular la dirección desde el lanzaSpawnPoint hacia el punto de impacto del rayo
            Vector3 direccion = (hit.point - spawnPointRoca.position).normalized;
            return direccion;
        }
        else
        {
            // Si el raycast no golpea nada, devolver la dirección hacia adelante
            return mainCamera.transform.forward;
        }
    }

    private void DetenerPlayer()
    {
        player.rb.velocity = Vector3.zero;
        player.moveSpeed = 0f;
    }

    private void ReanudarMovimientoPlayer()
    {
        player.moveSpeed = 5f;
    }

    private IEnumerator ReactivarColision(Collider rocaCollider)
    {
        yield return new WaitForSeconds(1f);
        //Debug.Log("Activado Collider");

        Physics.IgnoreCollision(rocaCollider, golemCollider, false);
        Physics.IgnoreCollision(rocaCollider, meleeCollider, false);
    }
    
}
