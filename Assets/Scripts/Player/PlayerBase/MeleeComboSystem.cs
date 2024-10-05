using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [Header("Configuracion del Combo")]
    [SerializeField] List<string> AnimacionesCombo;
    [SerializeField] float tiempoResetCombo = 1.5f;
    [SerializeField] float velAnimacion = 1.5f;
    public float costeManaAtq = 10f;

    private Animator animator;
    private int comboActual = 0;
    private float ultimoAtq;
    private bool isAttacking = false;
    private PlayerInput input;
    [SerializeField] UI playerUI;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        animator.speed = velAnimacion;
    }

    private void Update()
    {
        ManejoCombo();
    }

    private void ManejoCombo()
    {
        if(input.isAttacking && !isAttacking && playerUI.mana >= costeManaAtq)
        {
            if(ComboHabilitado())
            {
                UsarAtq();
                Debug.Log("next");
            }
        }
    }

    private bool ComboHabilitado()
    {
        Debug.Log("habilitado");
        return comboActual == 0 || (Time.time - ultimoAtq) <= tiempoResetCombo;
    }

    private void UsarAtq()
    {
        comboActual = proxAnimCombo();
        playerUI.useMana(costeManaAtq);
        ComboAnim();
        StartCoroutine(EsperarFinaldeAtq(AnimacionesCombo[comboActual -1]));
    }

    private void ComboAnim()
    {
        string atqActual = AnimacionesCombo[comboActual -1];
        animator.SetTrigger(atqActual);
        isAttacking = true;
        ultimoAtq = Time.time;
        Debug.Log("Animacion SI");
    }

    private int proxAnimCombo()
    {
        comboActual++;
        if(comboActual > AnimacionesCombo.Count)
        {
            comboActual = 1;
        }
        Debug.Log("siguiente");
        return comboActual;
    }

    private IEnumerator EsperarFinaldeAtq(string animAtq)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        isAttacking = false;

        Debug.Log("Final");

        if(Time.time - ultimoAtq > tiempoResetCombo)
        {
            comboActual = 0;
        }
    }

}
