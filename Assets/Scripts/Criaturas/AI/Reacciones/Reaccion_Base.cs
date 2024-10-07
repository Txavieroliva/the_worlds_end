using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Reaccion_Base : Base
{
    //. Detecta algo, se lo manda a filtrado
    protected void OnCollisionEnter(Collision collision)
    {
    Filtrado(collision);
    }
    
    //Revisa si el objeto es una criatura o un proyectil, en caso de ser un proyectil, consiguen una forma de llamar a su padre y lo mandan a la reaccion. En caso de ser una criatura, manda a la reacción
    protected void Filtrado (Collision collision) 
    {
        if (collision.gameObject.GetComponent<Proyectil>() != null)
        {
            Reaccionar(collision.gameObject.GetComponent<Proyectil>().Lanzado_Por());
        } 
        else if (collision.gameObject.GetComponent<Criatura_Base>() != null)
            {
                Reaccionar(collision.gameObject.GetComponent<Base_Con_Vida>());
            }
    }
        
        //Consigue un monobehaivour, revisa su comportamiento, si es agresivo, llama a una reaccion, si es neutral, llama a otra y si es pasivo, llama a otra.
    protected void Reaccionar(Base_Con_Vida Z)
    {
        if (Z.Comportamiento == "Agresivo")
        {
            Reaccion_Agresivo(Z);
            Debug.Log("a");
        } 
            else if (Z.Comportamiento == "Neutral")
            {
                Reaccion_Neutral(Z);
                Debug.Log("b");

            }
                else if (Z.Comportamiento == "Pasivo")
                {
                    Reaccion_Pasivo(Z);
                    Debug.Log("C");
                }
    Debug.Log("D");

    }

    //reacción contra neutral

    protected virtual void Reaccion_Agresivo(Base Z)
    {}
        
    //reacción contra pasivo
    protected virtual void Reaccion_Neutral(Base Z)
    {}

    
    //reacción contra agresivo
    protected virtual void Reaccion_Pasivo(Base Z)
    {}

}
