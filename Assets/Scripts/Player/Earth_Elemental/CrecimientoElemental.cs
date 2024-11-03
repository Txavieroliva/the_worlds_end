using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrecimientoElemental : AbilityBase
{
    [Header("Configuración de Crecimiento")]
    public float materialRequerido = 50f;
    [SerializeField] private float materialAcumulado = 0f;
    [SerializeField] private UnityEngine.UI.Image iconoHabilidad;

    private Player player;
    [SerializeField] private UI playerUI;
    private bool estaCreciendo = false;

    private void Start()
    {
        iconoHabilidad.fillAmount = 1;
        iconoHabilidad.color = Color.red;
        player = GetComponentInParent<Player>();
        ActualizarBarraHabilidad();
    }

    private void Update()
    {
        ActualizarBarraHabilidad();
    }

    public override void UseAbility()
    {
        if (!isOnCooldown && materialAcumulado >= materialRequerido && !estaCreciendo)
        {
            iconoHabilidad.color = Color.red;
            StartCoroutine(CrecerGolem());
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator CrecerGolem()
    {
        estaCreciendo = true;

        Vector3 tamañoInicial = player.transform.localScale;
        float escalaObjetivo = tamañoInicial.y + (materialAcumulado / 50f);
        Vector3 tamañoFinal = new Vector3(escalaObjetivo, escalaObjetivo, escalaObjetivo);

        float tiempoCrecimiento = 1.5f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoCrecimiento)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Calcular el factor de progreso normalizado
            float factorProgreso = tiempoTranscurrido / tiempoCrecimiento;

            // Utilizar Lerp para interpolar entre el tamaño inicial y el tamaño final
            player.transform.localScale = Vector3.Lerp(tamañoInicial, tamañoFinal, factorProgreso);

            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse de que el Golem alcance exactamente el tamaño final
        player.transform.localScale = tamañoFinal;

        playerUI.maxHealth += materialAcumulado;
        playerUI.health = playerUI.maxHealth;

        materialAcumulado = 0f;
        ActualizarBarraHabilidad();

        estaCreciendo = false;
    }

    public void ColeccionarMaterial(float cantidad)
    {
        materialAcumulado += cantidad;
        ActualizarBarraHabilidad();
    }

    private void ActualizarBarraHabilidad()
    {
        if (iconoHabilidad != null)
        {
            float fillValue = 1 - (materialAcumulado / materialRequerido);
            iconoHabilidad.fillAmount = fillValue;

            // Cambiar color en función del fillAmount
            if (materialAcumulado >= materialRequerido)
            {
                iconoHabilidad.color = Color.green; // Cambiar a verde si está lleno
                iconoHabilidad.fillAmount = 1;
            }
            else
            {
                iconoHabilidad.color = Color.red; // Cambiar a rojo si no está lleno
            }
        }
    }
}
