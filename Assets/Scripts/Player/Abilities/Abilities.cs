using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Ability 1")]
    public Image Ability1Image;
    public Text Ability1Text;
    public KeyCode Ability1Key;
    public float Ability1Cooldown = 5;

    [Header("Ability 2")]
    public Image Ability2Image;
    public Text Ability2Text;
    public KeyCode Ability2Key;
    public float Ability2Cooldown = 7;

    [Header("Ability 3")]
    public Image Ability3Image;
    public Text Ability3Text;
    public KeyCode Ability3Key;
    public float Ability3Cooldown = 15;


    private bool isAbility1Cooldown = false;
    private bool isAbility2Cooldown = false;
    private bool isAbility3Cooldown = false;

    private float currentAbility1Cooldown;
    private float currentAbility2Cooldown;
    private float currentAbility3Cooldown;

    // Start is called before the first frame update
    void Start()
    {
        Ability1Image.fillAmount = 0;
        Ability2Image.fillAmount = 0;
        Ability3Image.fillAmount = 0;

        Ability1Text.text = "";
        Ability2Text.text = "";
        Ability3Text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Ability1Input();
        Ability2Input();
        Ability3Input();

        AbilityCooldown(ref currentAbility1Cooldown, Ability1Cooldown, ref isAbility1Cooldown, Ability1Image, Ability1Text);
        AbilityCooldown(ref currentAbility2Cooldown, Ability2Cooldown, ref isAbility2Cooldown, Ability2Image, Ability2Text);
        AbilityCooldown(ref currentAbility3Cooldown, Ability3Cooldown, ref isAbility3Cooldown, Ability3Image, Ability3Text);
    }

    private void Ability1Input()
    {
        if(Input.GetKeyDown(Ability1Key) && !isAbility1Cooldown)
        {
            isAbility1Cooldown = true;
            currentAbility1Cooldown = Ability1Cooldown;
            Debug.Log("AHHHHHH");
        }
    }
    
    private void Ability2Input()
    {
        if(Input.GetKeyDown(Ability2Key) && !isAbility2Cooldown)
        {
            isAbility2Cooldown = true;
            currentAbility2Cooldown = Ability2Cooldown;
        }
    }
    
    private void Ability3Input()
    {
        if(Input.GetKeyDown(Ability3Key) && !isAbility3Cooldown)
        {
            isAbility3Cooldown = true;
            currentAbility3Cooldown = Ability3Cooldown;
        }
    }

   private void AbilityCooldown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, Image skillImage, Text skillText)
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
