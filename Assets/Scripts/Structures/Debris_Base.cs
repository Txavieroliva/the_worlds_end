using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debris : MonoBehaviour
{
   [SerializeField] private float healthAmount;

   protected virtual void Awake()
   {
    Vector3 vecty = new Vector3(Random.Range(1,3), Random.Range(1,3), Random.Range(1,3));
    transform.localScale = vecty; // Obtenemos la escala del objeto en la escena
   }
   public void Dar_Material(Material mater)
   {
    GetComponent<Renderer>().material = mater;
   }

   protected float CalcularVolumen()
{
    Vector3 scale = transform.localScale; // Obtenemos la escala del objeto en la escena
    return scale.x * scale.y * scale.z;
}
   private void OnCollisionEnter(Collision other)
   {
        
        Player player = other.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.cureWounds(CalcularVolumen() * 2f);
            Destroy(gameObject);
        }
   }
}