using System.Collections;
using UnityEngine;

public class RockSpear : AbilityBase
{
    public GameObject lanzaPrefab;  // Prefab de la lanza
    public Transform lanzaSpawnPoint;  // Punto desde donde se spawnea la lanza
    public float velocidadLanza = 50f;  // Velocidad de la lanza
    private Animator animator;
    private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private Collider golemCollider;
    [SerializeField] private Collider meleeCollider;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        mainCamera = Camera.main;
        golemCollider = GetComponentInParent<Collider>();
        
    }

    public override void UseAbility()
    {
        if (!isOnCooldown)
        {
            // Activar la animación de la habilidad
            animator.SetTrigger("Ability1");

            // Generar y lanzar la lanza
            StartCoroutine(GenerarTirarLanza());

            // Iniciar el cooldown
            StartCoroutine(StartCooldown());
        }
    }

    // Método para generar y lanzar la lanza
    private IEnumerator GenerarTirarLanza()
    {
        // Esperar a que la animación se reproduzca parcialmente
        yield return new WaitForSeconds(0.2f);

        // Obtener la dirección hacia la que se lanzará la lanza
        Vector3 direccion = ObtenerDireccionDelMouse();

        // Instanciar la lanza en la posición del lanzaSpawnPoint con la rotación hacia la dirección
        GameObject lanza = Instantiate(lanzaPrefab, lanzaSpawnPoint.position, Quaternion.LookRotation(direccion));
        Collider lanzaCollider = lanza.GetComponentInChildren<Collider>();


        Physics.IgnoreCollision(lanzaCollider, golemCollider, true);
        Physics.IgnoreCollision(lanzaCollider, meleeCollider, true);
        //Debug.Log("Desactivada Collider");

        StartCoroutine(ReactivarColision(lanzaCollider));

        // Aplicar la dirección y velocidad a la lanza
        Rigidbody lanzaRb = lanza.GetComponent<Rigidbody>();
        if (lanzaRb != null)
        {
            lanzaRb.velocity = direccion * velocidadLanza;
            player.TakeDamage(10);
        }
        else
        {
            Debug.LogError("El Rigidbody no está configurado en la lanza.");
        }
    }

    // Método para obtener la dirección desde el spawnpoint hacia el mouse
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
            Vector3 direccion = (hit.point - lanzaSpawnPoint.position).normalized;
            return direccion;
        }
        else
        {
            // Si el raycast no golpea nada, devolver la dirección hacia adelante
            return mainCamera.transform.forward;
        }
    }

    private IEnumerator ReactivarColision(Collider lanzaCollider)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Activado Collider");

        Physics.IgnoreCollision(lanzaCollider, golemCollider, false);
        Physics.IgnoreCollision(lanzaCollider, meleeCollider, false);
    }
}
