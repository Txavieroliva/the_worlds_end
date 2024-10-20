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

        Vector3 direction = obtenerDirMouse();
        
        GameObject roca = Instantiate(rocaPrefab, spawnPointRoca.position, spawnPointRoca.rotation);
        Collider rocaCollider = roca.GetComponentInChildren<Collider>();

        Physics.IgnoreCollision(rocaCollider, golemCollider, true);
        Physics.IgnoreCollision(rocaCollider, meleeCollider, true);




        Roca rocaScript = roca.GetComponent<Roca>();

        if(rocaScript != null)
        {
            rocaScript.Lanzar(direction, player);
            player.TakeDamage(25);
        }

        ReanudarMovimientoPlayer();
    }

    private Vector3 obtenerDirMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            return(hit.point - spawnPointRoca.position).normalized;
        }

        return spawnPointRoca.up;
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
    
}
