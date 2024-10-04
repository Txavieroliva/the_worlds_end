using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpear : AbilityBase
{
    public GameObject lanzaPrefab;
    public Transform lanzaSpawnPoint;
    public float velocidadLanza = 50f;
    private Animator animator;
    private Camera mainCamera;
    [SerializeField]private Player player;

    private void Start()
    {
        animator = GetComponent<Animator> ();
        mainCamera = Camera.main;
    }

    public override void UseAbility()
    {
        if(!isOnCooldown)
        {
            animator.SetTrigger("Ability1");

            StartCoroutine(GenerarTirarLanza());

            StartCoroutine(StartCooldown());

        }
    }

    private IEnumerator GenerarTirarLanza()
    {
        yield return new WaitForSeconds(1f);
        GameObject lanza = Instantiate(lanzaPrefab, lanzaSpawnPoint.position, lanzaSpawnPoint.rotation); // Instancia la lanza

        Vector3 direction = ObtenerDirMouse();

        Spear lanzaScript = lanza.GetComponent<Spear>(); // Verifica si el prefab tiene el componente Spear

        if(lanzaScript != null)
        {
            lanzaScript.Lanzar(direction, player);
            player.TakeDamage(10);
        }
    }

    private Vector3 ObtenerDirMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            return(hit.point - lanzaSpawnPoint.position).normalized;
        }

        return lanzaSpawnPoint.up;
    }
}
