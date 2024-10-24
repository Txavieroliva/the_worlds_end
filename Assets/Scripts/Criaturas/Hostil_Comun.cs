using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostil_Comun : MonoBehaviour
{
    private Cerebro miCerebro;

    void Start()
    {
        miCerebro = gameObject.AddComponent<Cerebro>();
        
        // Añadir acciones al cerebro del enemigo
        miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Atacar>());
        miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Patrullaje>());
        // miCerebro.Lista_Acciones.Add(gameObject.AddComponent<Defender>());
    }
}