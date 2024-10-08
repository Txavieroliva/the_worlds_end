using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ataques/Ataque Normal")]
public class Ataque_Base : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
