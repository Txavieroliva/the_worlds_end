using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public float cooldown; // Cooldown de la habilidad
    protected bool isOnCooldown; // Verifica si la habilidad esta en cooldown

    public abstract void UseAbility(); // Metodo abstracto implementado en cada habilidad especifica.

    public IEnumerator StartCooldown() // Metodo que inicia el cooldown
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
