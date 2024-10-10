using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerebro : MonoBehaviour
{
    public Huir[] Lista_Acciones;
    public Accion Sig_Accion;
    public Accion Accion_Actual;
    private bool Accionando = false;
    private void Calcular_Acciones(Accion[] Lista_Compara)
    {
        float Mayor_Puntaje = 0f;
        int Num_Accion = 0;

            for (int i = 0; i < Lista_Compara.Length; i++)
            {
                if (Lista_Compara[i].Calculo_Puntaje() > Mayor_Puntaje)
                {
                    Num_Accion = i;
                    Mayor_Puntaje = Lista_Compara[i].Puntos;
                }
            }

        Sig_Accion = Lista_Acciones[Num_Accion];
    }
    private void Ejecutar_Accion(Accion Acc)
        {
            Acc.Ejecutar();
        }
    void LateUpdate()
    {
        if (Accionando == false)
        {
            Calcular_Acciones(Lista_Acciones);
            Ejecutar_Accion(Sig_Accion);
            Accion_Actual = Sig_Accion;
            Accionando = true;
        } else 
            {
                Accion_Actual.Revisar();
            }
    }
}
