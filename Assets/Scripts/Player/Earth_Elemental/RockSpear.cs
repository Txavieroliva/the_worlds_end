using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpear : AbilityBase
{
    public GameObject lanzaPrefab;
    public Transform lanzaSpawnPoint;
    public float velocidadLanza = 50f;
    private Animator animator;
    private Rigidbody rb;
    private Camera mainCamera;
    [SerializeField]private Player player;

    private void Start()
    {
        animator = GetComponentInParent<Animator> ();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
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

    public void Lanzar(Vector3 direccion, Player player)
    {
        rb.velocity = direccion * velocidadLanza; // Asigna la dirección y velocidad a la lanza
    }

    private IEnumerator GenerarTirarLanza()
    {
        yield return new WaitForSeconds(1f);

        // Obtener la dirección calculada en base al mouse con Raycast
        Vector3 direction = ObtenerDirMouse();

        // Spawnear la lanza en la posición del lanzaSpawnPoint
        GameObject lanza = Instantiate(lanzaPrefab, lanzaSpawnPoint.position, Quaternion.LookRotation(direction));

        // Obtener el Rigidbody de la lanza para aplicarle una velocidad
        Rigidbody lanzaRb = lanza.GetComponent<Rigidbody>();
        if (lanzaRb != null)
        {
            // Asignar la dirección y velocidad a la lanza
            lanzaRb.velocity = direction * velocidadLanza;
        }
    }

    private Vector3 ObtenerDirMouse()
    {
        // Crea un rayo desde la cámara hacia la posición del mouse
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Variable para almacenar el punto de impacto del raycast
        RaycastHit hit;

        // Si el raycast golpea algo en el mundo, usamos ese punto como dirección
        if (Physics.Raycast(ray, out hit))
        {
            // Calculamos la dirección hacia el punto de impacto desde el lanzaSpawnPoint
            Vector3 direction = (hit.point - lanzaSpawnPoint.position).normalized;

            return direction;
        }
        else
        {
            // Si no golpea nada, usamos la dirección del rayo normalizada
            return ray.direction;
        }
    }
}
