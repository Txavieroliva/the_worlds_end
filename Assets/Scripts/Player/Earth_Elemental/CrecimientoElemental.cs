using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrecimientoElemental : MonoBehaviour
{
    [Header("Referencia UI")]
    [SerializeField] private UI playerUI;

    [Header("Habilidad de Crecer")]
    public UnityEngine.UI.Image imagenHabilidadCrecer;
    public Text textoHabilidadCrecer;
    public KeyCode keyHabilidadCrecer;
    public float cooldownCrecer = 10f;

    [Header("Pantalla Negra")]
    public UnityEngine.UI.Image blackScreen;

    [Header("Materiales")]
    public float materialTotalRecolectado = 0f;
    public float materialRequerido = 50f;

    private Player player;
    private PlayerInput input;
    private bool estaCreciendo = false;
    private bool estaCreciendoCooldown = false;
    private float actualCooldownCrecimiento;


    void Start()
    {
        player = GetComponent<Player>();
        input = GetComponent<PlayerInput>();
    }


    // Update is called once per frame
    void Update()
    {
        AbilityCooldown(ref actualCooldownCrecimiento, cooldownCrecer, ref estaCreciendoCooldown, imagenHabilidadCrecer, textoHabilidadCrecer);
    }

    private void iniciarVar()
    {
        imagenHabilidadCrecer.fillAmount = 0;
        textoHabilidadCrecer.text = "";
        blackScreen.color = new Color(0,0,0,0);
    }

    private void HabilidadCrecer()
    {
        if(Input.GetKeyDown(keyHabilidadCrecer) && !estaCreciendoCooldown && materialTotalRecolectado >= materialRequerido && !estaCreciendo)
        {
            estaCreciendo = true;
            actualCooldownCrecimiento = cooldownCrecer;

            materialTotalRecolectado = 0;

            StartCoroutine(CrecerGolem());
        }
    }

    private IEnumerator CrecerGolem()
    {
        estaCreciendo = true;

        StartCoroutine(OscurecerPantalla(true));

        player.rb.velocity = Vector3.zero;
        player.moveSpeed = 0f;

        yield return new WaitForSeconds(1f);

        playerUI.maxHealth += materialTotalRecolectado;
        player.CalcularTamaño();

        StartCoroutine(OscurecerPantalla(false));

        yield return new WaitForSeconds(1f);
        player.moveSpeed = 5f;

        estaCreciendo = false;
    }

    private IEnumerator OscurecerPantalla (bool oscurecer)
    {
        float duracion = 1f;
        float tiempoTranscurrido = 0f;
        Color colorInicio = blackScreen.color;
        Color colorFinal = oscurecer ? new Color(0,0,0,1) : new Color(0,0,0,0);

        while(tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            blackScreen.color = Color.Lerp(colorInicio, colorFinal, tiempoTranscurrido / duracion);
            yield return null;
        }
    }

    private void AbilityCooldown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, UnityEngine.UI.Image skillImage, Text skillText)
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            // Actualizar la barra de cooldown
            if (skillImage != null)
            {
                skillImage.fillAmount = currentCooldown / maxCooldown;
            }

            // Mostrar el tiempo restante en el texto
            if (skillText != null)
            {
                skillText.text = Mathf.Ceil(currentCooldown).ToString();
            }

            // Verificar si el cooldown ha terminado
            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0f;

                // Restablecer la barra y el texto
                if (skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }

                if (skillText != null)
                {
                    skillText.text = "";
                }
            }
        }
    }
    
}
