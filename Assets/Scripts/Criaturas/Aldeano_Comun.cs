using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeano_Comun : MonoBehaviour
{
    private Cerebro miCerebro;

    void Start()
    {
        miCerebro = gameObject.AddComponent<Cerebro>();
        
        // Añadir acciones al cerebro del enemigo
        miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Huir>());
        // miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Atacar>());
        // miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Defender>());
    }
}