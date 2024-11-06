using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrecimientoElemental : AbilityBase
{
    [Header("Configuración de Crecimiento")]
    public float materialRequerido = 25f;
    [SerializeField] private float materialAcumulado = 0f;
    [SerializeField] private UnityEngine.UI.Image iconoHabilidad;

    private Player player;
    [SerializeField] private UI playerUI;
    private bool estaCreciendo = false;
    public UnityEngine.Material characterMaterial; // Asigna aquí el material de tu personaje
    private float originalAlpha;

    private void Start()
    {
        iconoHabilidad.fillAmount = 1;
        iconoHabilidad.color = Color.red;
        player = GetComponentInParent<Player>();
        ActualizarBarraHabilidad();
        originalAlpha = characterMaterial.color.a;
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
            MakeTransparent(100, 2);
            StartCoroutine(CrecerGolem());
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator CrecerGolem()
    {
        estaCreciendo = true;

        // Tamaño inicial y tamaño final calculados en base al material acumulado
        Vector3 tamañoInicial = player.transform.localScale;
        float escalaObjetivo = tamañoInicial.y + (materialAcumulado / 25f);  
        Vector3 tamañoFinal = new Vector3(escalaObjetivo, escalaObjetivo, escalaObjetivo);

        float tiempoCrecimiento = 0.4f;  // Duración del crecimiento
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoCrecimiento)
        {
            tiempoTranscurrido += Time.deltaTime;

            float factorProgreso = tiempoTranscurrido / tiempoCrecimiento;
            // Debug.Log(tiempoTranscurrido);
            // Debug.Log(factorProgreso);
            // Debug.Log("");
            // if(factorProgreso > 1)
            // {
            //     factorProgreso = 1;
            // }

            // Interpolación entre el tamaño inicial y final
            player.transform.localScale = Vector3.Lerp(tamañoInicial, tamañoFinal, factorProgreso);

            yield return null;
        }
        
        player.transform.localScale = tamañoFinal;

        playerUI.maxHealth += materialAcumulado;
        playerUI.health = playerUI.maxHealth;

        materialAcumulado = 0f;
        ActualizarBarraHabilidad();
        RestoreMaterial();

        estaCreciendo = false;
    }

    public void ColeccionarMaterial(float cantidad)
    {
        materialAcumulado += cantidad;
        if(materialAcumulado > materialRequerido)
        {
            materialAcumulado = materialRequerido;
        }
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

    public void MakeTransparent(float transparencyAmount, float duration)
    {
        // Cambiar el modo de renderizado a transparente
        SetMaterialRenderingModeTransparent();

        // Ajustar la transparencia
        Color color = characterMaterial.color;
        color.a = transparencyAmount;
        characterMaterial.color = color;

        // Restaurar la opacidad después de la duración especificada
        Invoke(nameof(RestoreMaterial), duration);
    }

    private void RestoreMaterial()
    {
        // Restaurar el modo de renderizado a opaco
        SetMaterialRenderingModeOpaque();

        // Restaurar el alfa original
        Color color = characterMaterial.color;
        color.a = originalAlpha;
        characterMaterial.color = color;
    }

    private void SetMaterialRenderingModeTransparent()
    {
        characterMaterial.SetFloat("_Mode", 3); // 3 es el valor para Transparent en el shader Standard
        characterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        characterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        characterMaterial.SetInt("_ZWrite", 0);
        characterMaterial.DisableKeyword("_ALPHATEST_ON");
        characterMaterial.EnableKeyword("_ALPHABLEND_ON");
        characterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        characterMaterial.renderQueue = 3000;
    }

    private void SetMaterialRenderingModeOpaque()
    {
        characterMaterial.SetFloat("_Mode", 0); // 0 es el valor para Opaque en el shader Standard
        characterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        characterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        characterMaterial.SetInt("_ZWrite", 1);
        characterMaterial.DisableKeyword("_ALPHATEST_ON");
        characterMaterial.DisableKeyword("_ALPHABLEND_ON");
        characterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        characterMaterial.renderQueue = -1;
    }

}
