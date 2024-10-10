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

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        mainCamera = Camera.main;
    }

    public override void UseAbility()
    {
        if(!isOnCooldown)
        {
            animator.SetTrigger("Ability1");

            StartCoroutine(GenerarLanzarRoca());

            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator GenerarLanzarRoca()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject roca = Instantiate(rocaPrefab, spawnPointRoca.position, spawnPointRoca.rotation);

        Vector3 direction = obtenerDirMouse();
        Roca rocaScript = roca.GetComponent<Roca>();

        if(rocaScript != null)
        {
            rocaScript.Lanzar(direction, player);
            player.TakeDamage(25);
        }
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
}
