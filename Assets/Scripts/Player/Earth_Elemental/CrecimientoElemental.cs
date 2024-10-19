using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrecimientoElemental : AbilityBase
{
    [Header("Crecer Config")]
    public float materialRequerido = 50f;
    [SerializeField] private float materialAcumulado = 0f;
    public UnityEngine.UI.Image blackScreen;
    public Text MaterialRecolectadoText;

    private Player player;
    [SerializeField] private UI playerUI;
    private bool estaCreciendo = false;

    private void Start()
    {
        player = GetComponentInParent<Player>();

        iniciarBlackScreen();

        ActualizarMaterial();
    }

    private void Update()
    {
        ActualizarMaterial();
    }

    public override void UseAbility()
    {
        if (!isOnCooldown && materialAcumulado >= materialRequerido && !estaCreciendo)
        {
            StartCoroutine(CrecerGolem());

            // Iniciar el cooldown
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator CrecerGolem()
    {
        estaCreciendo = true;
        
        yield return StartCoroutine(TransicionNegro(true));
        

        player.rb.velocity = Vector3.zero;
        player.moveSpeed = 0;

        yield return new WaitForSeconds(1f);

        playerUI.maxHealth += materialAcumulado;
        playerUI.health = playerUI.maxHealth;
        
        Vector3 newScale = player.transform.localScale * (1 + materialAcumulado / playerUI.maxHealth);
        player.transform.localScale = newScale;
        materialAcumulado = 0f;
        player.moveSpeed = 5f;
        //yield return new WaitForSeconds(1f);
        
        yield return StartCoroutine(TransicionNegro(false));

        estaCreciendo = false;
    }

    public void coleccionarMaterial(float cantidad)
    {
        materialAcumulado += cantidad;
    }

    private void iniciarBlackScreen()
    {
        Color colorInicial = blackScreen.color;
        colorInicial.a = 0f; // Lo hace transparente
        blackScreen.color = colorInicial;
    }

    private IEnumerator TransicionNegro(bool TransicionNegro)
    {
        float duracion = 2f;
        float tiempoTranscurrido = 0f;
        float colorObjetivo = TransicionNegro ? 1f : 0f;
        Color colorPantalla = blackScreen.color;

        while(tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            colorPantalla.a = Mathf.Lerp(colorPantalla.a, colorObjetivo, tiempoTranscurrido/ duracion);
            blackScreen.color = colorPantalla;
            yield return null;
        }

        colorPantalla.a = colorObjetivo;
        blackScreen.color = colorPantalla;
    }
    
    private void ActualizarMaterial()
    {
        if(MaterialRecolectadoText != null)
        {
            MaterialRecolectadoText.text = "Material Recolectado: " + materialAcumulado.ToString("F1") + " / " + materialRequerido.ToString("F1");
        }
    }

}
