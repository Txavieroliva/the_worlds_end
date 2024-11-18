using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrecimientoElemental : AbilityBase
{
    [Header("Configuración de Crecimiento")]
    public float materialRequerido = 25f;
    [SerializeField] private float materialAcumulado = 0f;
    [SerializeField] private Image iconoHabilidad;

    private Player player;
    [SerializeField] private UI playerUI;
    private bool estaCreciendo = false;
    public Material characterMaterial;
    private float originalAlpha;
    private Rigidbody playerRigidbody;

    // Factor de corrección de altura
    private float factorAltura = 5f; // Ajusta este valor según sea necesario

    private void Start()
    {
        iconoHabilidad.fillAmount = 1;
        iconoHabilidad.color = Color.red;
        player = GetComponentInParent<Player>();
        playerRigidbody = player.GetComponent<Rigidbody>();
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
            MakeTransparent(100f, 2);
            CrecerGolem();
            StartCoroutine(StartCooldown());
        }
    }

    private void CrecerGolem()
    {
        estaCreciendo = true;

        // Desactivar temporalmente la gravedad
        playerRigidbody.useGravity = false;
        playerRigidbody.velocity = Vector3.zero;

        // Tamaño inicial y tamaño final calculados en base al material acumulado
        Vector3 tamañoInicial = player.transform.localScale;
        float escalaObjetivo = tamañoInicial.y + Mathf.Min(materialAcumulado, materialRequerido) / 25f;
        Vector3 tamañoFinal = new Vector3(escalaObjetivo, escalaObjetivo, escalaObjetivo);

        // Posición inicial y posición final ajustada
        Vector3 posicionInicial = player.transform.position;
        float diferenciaAltura = (escalaObjetivo - tamañoInicial.y) * factorAltura;
        Vector3 posicionFinal = posicionInicial + new Vector3(0, diferenciaAltura, 0);

        //float tiempoCrecimiento = 5f;
        //float tiempoTranscurrido = 0f;
        

    // for (float t = 0; t < tiempoCrecimiento; t += Time.deltaTime)
    // {
    //     float factorProgreso = t / tiempoCrecimiento;

        // Interpolar tanto el tamaño como la posición
       
        // player.transform.localScale = Vector3.Lerp(tamañoInicial, tamañoFinal, factorProgreso);

          // Espera al siguiente frame
    // }
    //     yield return null;
        // Asegurarse de que el Golem alcance exactamente el tamaño y la posición final
        // player.transform.localScale = tamañoFinal;
        // player.transform.position = posicionFinal;

        // Reactivar la gravedad
        playerRigidbody.useGravity = true;
 
        player.transform.position =posicionFinal;
        playerUI.maxHealth = playerUI.health + materialAcumulado;
        playerUI.health += materialAcumulado;

        materialAcumulado = 0f;
        ActualizarBarraHabilidad();
        RestoreMaterial();

        estaCreciendo = false;
    }

    public void ColeccionarMaterial(float cantidad)
    {
        materialAcumulado += cantidad;
        if (materialAcumulado > materialRequerido)
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

            if (materialAcumulado >= materialRequerido)
            {
                iconoHabilidad.color = Color.green;
                iconoHabilidad.fillAmount = 1;
            }
            else
            {
                iconoHabilidad.color = Color.red;
            }
        }
    }

    public void MakeTransparent(float transparencyAmount, float duration)
    {
        SetMaterialRenderingModeTransparent();
        Color color = characterMaterial.color;
        color.a = transparencyAmount;
        characterMaterial.color = color;
        Invoke(nameof(RestoreMaterial), duration);
    }

    private void RestoreMaterial()
    {
        SetMaterialRenderingModeOpaque();
        Color color = characterMaterial.color;
        color.a = originalAlpha;
        characterMaterial.color = color;
    }

    private void SetMaterialRenderingModeTransparent()
    {
        characterMaterial.SetFloat("_Mode", 3);
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
        characterMaterial.SetFloat("_Mode", 0);
        characterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        characterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        characterMaterial.SetInt("_ZWrite", 1);
        characterMaterial.DisableKeyword("_ALPHATEST_ON");
        characterMaterial.DisableKeyword("_ALPHABLEND_ON");
        characterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        characterMaterial.renderQueue = -1;
    }
}
